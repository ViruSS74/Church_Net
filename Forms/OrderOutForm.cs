using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;  // ✅ Должен быть
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class OrderOutForm : Form
    {
        private int _orderId;
        private int _docId;
        private ListOfDocsService _service;

        private string _orgName = "";
        private string _dateDay = "";
        private string _dateMonth = "";
        private string _dateYear = "";
        private string _personNameFull = "";
        private string _personLastName = "";
        private string _personFirstMiddle = "";
        private string _orderAppendix = "";
        private string _personPassportData = "";
        private string _orderNumber = "";
        private decimal _orderAmount = 0;
        private string _orderBase = "";
        private string _rectorName = "";
        private string _treasurerName = "";
        private string _currentHtml = "";

        public OrderOutForm(int orderId, ListOfDocsService service)
        {
            InitializeComponent();

            this._orderId = orderId;
            this._docId = orderId;
            this._service = service ?? new ListOfDocsService(Program.DbPath);

            FillPersonCombo();
            FillRecipients();

            LoadOrderData();
            ApplyRkoGridStyle();
            LoadRkoRegistryTable();
            UpdateTableFromSelectors();

            ImageHelper.ApplyToButtons(this, 24);

            FillRkoBasis();

            if (cmbPerson.Items.Count > 3)
            {
                cmbPerson.SelectedIndex = 3;
                if (int.TryParse(cmbPerson.SelectedValue.ToString(), out int pId))
                {
                    _personPassportData = _service.GetPassportInfo(pId);
                    _personNameFull = cmbPerson.Text;
                }
            }

            UpdatePreview();
        }

        private void FillPersonCombo()
        {
            DataTable dt = _service.GetPersonalListForCmb();
            cmbPerson.DataSource = dt;
            cmbPerson.DisplayMember = "FullName";
            cmbPerson.ValueMember = "id";
        }

        private void FillRkoBasis()
        {
            if (dgvData.DataSource == null) return;
            DataTable dt = dgvData.DataSource as DataTable;

            List<string> basisList = _service.GetRkoBasisItems(_orderId);

            for (int i = 0; i < basisList.Count; i++)
            {
                if (dt.Rows.Count <= i + 1)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                dt.Rows[i + 1]["2"] = basisList[i];
            }
            dgvData.Refresh();
        }

        private void LoadOrderDataForPrint(int id)
        {
            DataTable dt = _service.GetRkoReportData(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                _orderNumber = row["No"].ToString();
                _orderAmount = Convert.ToDecimal(row["Sum"]);
                _orderBase = row["Basis"].ToString();
                _orderAppendix = row["Appendix"].ToString();
                _personPassportData = row["Passport"].ToString();

                DateTime dtDate = Convert.ToDateTime(row["Date"]);
                _dateDay = dtDate.Day.ToString("D2");
                _dateMonth = dtDate.ToString("MMMM");
                _dateYear = dtDate.Year.ToString();
            }
        }

        private void FillRecipients()
        {
            DataTable dt = _service.GetRecipients();
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["full_name"] = "-- Не указан --";
            dt.Rows.InsertAt(dr, 0);
            cmbRecipient.DataSource = dt;
            cmbRecipient.DisplayMember = "full_name";
            cmbRecipient.ValueMember = "id";
        }

        private void LoadOrderData()
        {
            try
            {
                string rectorFull = "";
                string rectorLast = "";

                DataTable personal = _service.GetPersonalList();
                if (personal != null)
                {
                    foreach (DataRow row in personal.Rows)
                    {
                        string role = (row["role"] ?? "").ToString().ToLower();
                        string lName = (row["last_name"] ?? "").ToString();
                        string fName = (row["first_name"] ?? "").ToString();
                        string mName = (row["middle_name"] ?? "").ToString();
                        string sn = GetShortName(lName, fName, mName);

                        if (role.Contains("настоятель"))
                        {
                            _rectorName = sn;
                            rectorFull = _service.GetPersonDative(lName, fName, mName);
                            rectorLast = lName;
                        }
                        if (role.Contains("казначей")) _treasurerName = sn;
                    }
                }

                DataRow orgRow = _service.GetOrganizationData();
                if (orgRow != null)
                    _orgName = (orgRow["name"] ?? "").ToString() + " " + (orgRow["location"] ?? "").ToString();

                DataRow orderRow = _service.GetCashOrderData(_orderId);
                if (orderRow != null)
                {
                    _orderNumber = (orderRow["order_number"] ?? "").ToString();
                    _orderAmount = orderRow["amount"] != DBNull.Value ? Convert.ToDecimal(orderRow["amount"]) : 0;
                    _orderBase = (orderRow["base"] ?? "").ToString();
                    _orderAppendix = (orderRow["appendix"] ?? "").ToString();
                    _personPassportData = (orderRow["full_passport"] ?? "").ToString();

                    if (orderRow["last_name"] != DBNull.Value && !string.IsNullOrEmpty(orderRow["last_name"].ToString()))
                    {
                        string ln = orderRow["last_name"].ToString();
                        string fn = orderRow["first_name"].ToString();
                        string mn = orderRow["middle_name"].ToString();

                        _personNameFull = _service.GetPersonDative(ln, fn, mn);
                        _personLastName = ln;
                        _personFirstMiddle = string.Format("{0} {1}", fn, mn).Trim();
                    }
                    else
                    {
                        string manualPerson = (orderRow["person_name_manual"] ?? "").ToString();
                        if (!string.IsNullOrEmpty(manualPerson))
                        {
                            _personNameFull = manualPerson;
                            _personLastName = manualPerson;
                        }
                        else
                        {
                            _personNameFull = rectorFull;
                            _personLastName = rectorLast;
                        }
                        _personFirstMiddle = "";
                    }

                    if (orderRow["date"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(orderRow["date"]);
                        _dateDay = dt.Day.ToString("D2");
                        _dateMonth = dt.ToString("MMMM", new System.Globalization.CultureInfo("ru-RU"));
                        _dateYear = dt.Year.ToString();
                    }
                }

                txtEditBasis.Text = _orderBase;
                txtEditAppendix.Text = _orderAppendix;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных бланка: " + ex.Message);
            }
        }

        private string GetShortName(string last, string first, string middle)
        {
            string f = (first.Length > 0) ? first.Substring(0, 1) + "." : "";
            string m = (middle.Length > 0) ? middle.Substring(0, 1) + "." : "";
            return string.Format("{0}{1} {2}", f, m, last);
        }

        private void UpdatePreview()
        {
            try
            {
                _currentHtml = GenerateRkoHtml();
                wbPreview.DocumentText = _currentHtml;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления превью: {ex.Message}");
            }
        }

        #region HTML Generation

        private string GenerateRkoHtml()
        {
            StringBuilder html = new StringBuilder();

            int rubles = (int)Math.Floor(_orderAmount);
            int kopecks = (int)Math.Round((_orderAmount - rubles) * 100);
            string rublesText = CurrencyToWordsRubles(rubles);

            html.AppendLine(@"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        @page { size: A4 portrait; margin: 10mm; }
        body { 
            font-family: 'Times New Roman', Times, serif; 
            font-size: 10pt; 
            margin: 0;
            padding: 10mm;
            line-height: 1.4;
        }
        .title { font-weight: bold; font-size: 11pt; margin-bottom: 10px; }
        .line { 
            display: inline-block; 
            border-bottom: 1px solid #000; 
            min-width: 50px; 
            vertical-align: bottom;
        }
        .line-short { min-width: 30px; }
        .line-medium { min-width: 100px; }
        .line-long { min-width: 200px; }
        .line-xlong { min-width: 400px; }
        table.sum-table { border-collapse: collapse; width: 100%; margin: 10px 0; }
        table.sum-table td { border: 1px solid #000; padding: 5px; vertical-align: middle; }
        .small { font-size: 8pt; }
        .signature-line { 
            display: inline-block; 
            border-bottom: 1px solid #000; 
            min-width: 100px; 
            margin: 0 5px;
        }
        .amount-cell { text-align: right; font-weight: bold; font-size: 11pt; }
        @media print {
            body { margin: 0; padding: 10mm; }
            .no-print { display: none; }
        }
    </style>
</head>
<body>");

            // ШАПКА
            html.AppendLine(string.Format("<div class='title'>РАСХОДНЫЙ КАССОВЫЙ ОРДЕР № <span class='line line-medium'>{0}</span> «<span class='line line-short'>{1}</span>» <span class='line line-medium'>{2}</span> 20<span class='line line-short'>{3}</span> г.</div>",
                _orderNumber, _dateDay, _dateMonth, _dateYear.Substring(_dateYear.Length - 2)));

            // ТАБЛИЦА СУММЫ
            html.AppendLine("<table class='sum-table'>");
            html.AppendLine("<tr>");
            html.AppendLine("<td style='width:70%; vertical-align:top;'>");
            html.AppendLine("<div class='small'>Корреспондирующий счет, субсчет</div>");
            html.AppendLine("</td>");
            html.AppendLine("<td style='width:30%; vertical-align:top;'>");
            html.AppendLine("<div class='small'>Сумма, руб. коп.</div>");
            html.AppendLine(string.Format("<div class='amount-cell'>{0}</div>", _orderAmount.ToString("N2")));
            html.AppendLine("</td>");
            html.AppendLine("</tr>");
            html.AppendLine("</table>");

            // ВЫДАТЬ
            html.AppendLine(string.Format("<div style='margin-top:15px;'>Выдать <span class='line line-xlong'>{0}</span></div>", _personNameFull));
            html.AppendLine("<div class='small' style='margin-left:80px;'>(фамилия, собственное имя и отчество (если таковое имеется))</div>");

            // ОСНОВАНИЕ
            html.AppendLine(string.Format("<div style='margin-top:15px;'>Основание <span class='line line-xlong'>{0}</span></div>", _orderBase));

            // СУММА ПРОПИСЬЮ
            html.AppendLine(string.Format("<div style='margin-top:15px;'>Сумма <span class='line line-long'>{0}</span> руб. <span class='line line-short'>{1}</span> коп.</div>",
                rublesText, kopecks.ToString("00")));

            // ПРИЛОЖЕНИЕ
            html.AppendLine(string.Format("<div style='margin-top:15px;'>Приложение <span class='line line-xlong'>{0}</span></div>", _orderAppendix ?? ""));

            // НАСТОЯТЕЛЬ
            html.AppendLine(string.Format("<div style='margin-top:25px;'>Настоятель храма <span class='signature-line'></span> {0}</div>", _rectorName));
            html.AppendLine("<div class='small' style='margin-left:150px;'>(подпись)</div>");
            html.AppendLine("<div class='small' style='margin-left:580px;'>(инициалы, фамилия)</div>");

            // ПОЛУЧИЛ
            html.AppendLine("<div style='margin-top:20px;'>Получил <span class='signature-line'></span></div>");
            html.AppendLine("<div class='small' style='margin-left:100px;'>(подпись получателя)</div>");

            // ПАСПОРТ
            html.AppendLine(string.Format("<div style='margin-top:20px;'>Предъявлен документ <span class='line line-xlong'>{0}</span></div>", _personPassportData ?? ""));
            html.AppendLine("<div class='small' style='margin-left:150px;'>(данные о документе, удостоверяющем личность получателя)</div>");

            // КАЗНАЧЕЙ
            html.AppendLine(string.Format("<div style='margin-top:25px;'>Выдал казначей <span class='signature-line'></span> {0}</div>", _treasurerName));

            html.AppendLine("</body></html>");

            return html.ToString();
        }

        public string CurrencyToWords(decimal amount)
        {
            long rub = (long)Math.Floor(amount);
            int kop = (int)Math.Round((amount - rub) * 100);

            if (rub == 0) return "Ноль рублей " + kop.ToString("D2") + " коп.";

            string[] ones = { "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
            string[] tens = { "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
            string[] teens = { "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
            string[] hundreds = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };

            string result = "";

            if (rub >= 1000)
            {
                long thousands = rub / 1000;
                result += thousands.ToString() + " тыс. ";
                rub %= 1000;
            }

            if (rub >= 100) { result += hundreds[rub / 100] + " "; rub %= 100; }
            if (rub >= 10 && rub < 20) { result += teens[rub - 10] + " "; }
            else
            {
                if (rub >= 20) { result += tens[rub / 10] + " "; rub %= 10; }
                if (rub > 0) { result += ones[rub] + " "; }
            }
            return result.Trim() + " руб. " + kop.ToString("D2") + " коп.";
        }

        public string CurrencyToWordsRubles(long rubles)
        {
            if (rubles == 0) return "Ноль рублей";

            string[] ones = { "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
            string[] tens = { "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
            string[] teens = { "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
            string[] hundreds = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };

            string result = "";

            if (rubles >= 1000)
            {
                long thousands = rubles / 1000;
                result += thousands.ToString() + " тыс. ";
                rubles %= 1000;
            }

            if (rubles >= 100) { result += hundreds[rubles / 100] + " "; rubles %= 100; }
            if (rubles >= 10 && rubles < 20) { result += teens[rubles - 10] + " "; }
            else
            {
                if (rubles >= 20) { result += tens[rubles / 10] + " "; rubles %= 10; }
                if (rubles > 0) { result += ones[rubles] + " "; }
            }

            return result.Trim();
        }

        #endregion

        private void ApplyRkoGridStyle()
        {
            if (dgvData.Columns.Count < 6) return;

            dgvData.RowHeadersVisible = false;
            dgvData.AllowUserToAddRows = false;
            dgvData.GridColor = Color.Black;
            dgvData.BorderStyle = BorderStyle.FixedSingle;  // ✅ Рамка вокруг всей таблицы
            dgvData.ColumnHeadersHeight = 85;
            dgvData.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;  // ✅ Серый фон шапки

            dgvData.Columns["1"].HeaderText = "Фамилия, собственное имя и отчество (если таковое имеется)";
            dgvData.Columns["1"].Width = 180;

            dgvData.Columns["1а"].HeaderText = "Документ, удостоверяющий личность";
            dgvData.Columns["1а"].Width = 200;

            dgvData.Columns["2"].HeaderText = "Основание выдачи денег";
            dgvData.Columns["2"].Width = 150;

            dgvData.Columns["3"].HeaderText = "Наименование документа";
            dgvData.Columns["3"].Width = 130;

            dgvData.Columns["4"].HeaderText = "Код валюты";
            dgvData.Columns["4"].Width = 60;

            dgvData.Columns["4а"].HeaderText = "Наименование валюты";
            dgvData.Columns["4а"].Width = 110;

            dgvData.Columns["2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            if (dgvData.Rows.Count > 0)
            {
                // Первая строка (шапка с текстом) - жирная, в рамке
                dgvData.Rows[0].DefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);
                dgvData.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvData.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;

                // Вторая строка (1, 1а, 2...) - меньшим шрифтом
                if (dgvData.Rows.Count > 1)
                {
                    dgvData.Rows[1].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);  // ✅ Меньший шрифт
                    dgvData.Rows[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvData.Rows[1].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void LoadRkoRegistryTable()
        {
            try
            {
                DataTable dt = new DataTable();
                string[] cols = { "1", "1а", "2", "3", "4", "4а" };
                foreach (var c in cols) dt.Columns.Add(c);

                dt.Rows.Add("1", "1а", "2", "3", "4", "4а");

                DataTable dbData = _service.GetRkoRegistryData();
                string currentDoc = cmbDocs.Text;

                bool firstRow = true;
                foreach (DataRow dr in dbData.Rows)
                {
                    string passport = dr["passport_full"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(passport) || passport.Trim() == ", выдан")
                    {
                        passport = "";
                    }

                    dt.Rows.Add(
                        firstRow ? (dr["last_name"].ToString() + " " + dr["first_mid"].ToString()) : "",
                        firstRow ? passport : "",
                        dr["expense_reason"],
                        firstRow ? currentDoc : "",
                        firstRow ? "BYN" : "",
                        firstRow ? "Белорусский рубль" : ""
                    );
                    firstRow = false;
                }

                for (int i = 0; i < 12; i++) dt.Rows.Add("", "", "", "", "", "");

                dgvData.DataSource = dt;
                ApplyRkoGridStyle();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка отрисовки таблицы: " + ex.Message); }
        }

        private void cmbPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPerson.SelectedValue != null && int.TryParse(cmbPerson.SelectedValue.ToString(), out int personId))
            {
                _personPassportData = _service.GetPassportInfo(personId);
                _personNameFull = cmbPerson.Text;
                UpdateTableFromSelectors();
                UpdatePreview();
            }
        }

        private void cmbDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTableFromSelectors();
        }

        private void UpdateTableFromSelectors()
        {
            if (dgvData.DataSource == null) return;
            DataTable dt = dgvData.DataSource as DataTable;
            if (dt == null || dt.Rows.Count < 2) return;

            dt.BeginLoadData();

            if (dt.Columns.Contains("3"))
                dt.Rows[1]["3"] = cmbDocs.Text;

            if (cmbPerson.SelectedIndex != -1)
            {
                if (dt.Columns.Contains("1")) dt.Rows[1]["1"] = cmbPerson.Text;
                if (dt.Columns.Contains("1а")) dt.Rows[1]["1а"] = _personPassportData;
            }

            for (int i = 2; i < dt.Rows.Count; i++)
            {
                if (dt.Columns.Contains("1")) dt.Rows[i]["1"] = string.Empty;
                if (dt.Columns.Contains("1а")) dt.Rows[i]["1а"] = string.Empty;
                if (dt.Columns.Contains("3")) dt.Rows[i]["3"] = string.Empty;
            }

            dt.EndLoadData();
            dgvData.Refresh();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            tabRKO.SelectedIndex = 0;
            UpdatePreview();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument pd = new PrintDocument();

                if (tabRKO.SelectedTab.Name == "tabPrintForm")
                {
                    // Печать бланка РКО через WebBrowser
                    wbPreview.Print();
                }
                else if (tabRKO.SelectedTab.Name == "tabData")
                {
                    // ✅ Печать реестра через PrintDocument
                    pd.DefaultPageSettings.Landscape = true;  // Альбомная ориентация
                    pd.PrinterSettings.DefaultPageSettings.Landscape = true;
                    pd.PrintPage += new PrintPageEventHandler(PrintRegistryPage);

                    PrintDialog pDialog = new PrintDialog();
                    pDialog.Document = pd;
                    pDialog.UseEXDialog = true;

                    if (pDialog.ShowDialog() == DialogResult.OK)
                    {
                        pd.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при печати: " + ex.Message);
            }
        }

        private int currentRowIndex = 0;

        private void PrintRegistryPage(object sender, PrintPageEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[РКО Печать] Всего строк: {dgvData.Rows.Count}");

            e.PageSettings.Landscape = true;
            Graphics g = e.Graphics;

            Font fHeader = new Font("Arial", 9, FontStyle.Bold);
            Font fCell = new Font("Arial", 8);
            Font fSmall = new Font("Arial", 7, FontStyle.Bold);
            Pen pen = new Pen(Color.Black, 1);

            int x = 30;
            int y = 30;

            // ✅ ПРАВИЛЬНЫЕ ширины колонок (соответствуют ApplyRkoGridStyle)
            int[] colWidths = { 160, 180, 250, 110, 50, 100 };
            int headerHeight = 60;

            System.Diagnostics.Debug.WriteLine($"[РКО Печать] Колонок: {dgvData.Columns.Count}");

            // 1. ШАПКА ТАБЛИЦЫ (с рамкой)
            x = 30;
            for (int i = 0; i < dgvData.Columns.Count; i++)
            {
                Rectangle rect = new Rectangle(x, y, colWidths[i], headerHeight);
                g.DrawRectangle(pen, rect);

                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(dgvData.Columns[i].HeaderText, fHeader, Brushes.Black, rect, sf);
                }
                x += colWidths[i];
            }

            y += headerHeight;

            // 2. СТРОКА С ЦИФРАМИ (ПЕРВАЯ строка данных, индекс 0!)
            x = 30;
            if (dgvData.Rows.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[РКО Печать] Печатаем строку с цифрами (индекс 0)");
                for (int i = 0; i < dgvData.Columns.Count; i++)
                {
                    Rectangle rect = new Rectangle(x, y, colWidths[i], 25);
                    g.DrawRectangle(pen, rect);

                    string cellValue = "";
                    if (dgvData.Rows[0].Cells[i].Value != null)
                    {
                        cellValue = dgvData.Rows[0].Cells[i].Value.ToString();
                    }

                    System.Diagnostics.Debug.WriteLine($"[РКО Печать] Колонка {i}: '{cellValue}'");

                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString(cellValue, fSmall, Brushes.Black, rect, sf);
                    }
                    x += colWidths[i];
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[РКО Печать] Строк нет!");
            }

            y += 25;

            // 3. ДАННЫЕ (начиная со ВТОРОЙ строки, индекс 1)
            currentRowIndex = 1;  // ✅ НАЧИНАЕМ С ИНДЕКСА 1 (не 2!)
            while (currentRowIndex < dgvData.Rows.Count)
            {
                DataGridViewRow row = dgvData.Rows[currentRowIndex];
                if (row.IsNewRow) { currentRowIndex++; continue; }

                // Расчёт высоты строки по колонке "Основание" (индекс 2)
                string basisText = (row.Cells[2].Value ?? "").ToString();
                SizeF size = g.MeasureString(basisText, fCell, colWidths[2]);
                int rowHeight = Math.Max(30, (int)Math.Ceiling(size.Height) + 10);

                if (y + rowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                x = 30;
                for (int i = 0; i < dgvData.Columns.Count; i++)
                {
                    Rectangle rect = new Rectangle(x, y, colWidths[i], rowHeight);
                    g.DrawRectangle(pen, rect);

                    string cellValue = (row.Cells[i].Value ?? "").ToString();
                    using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Near })
                    {
                        sf.Alignment = StringAlignment.Near;
                        sf.FormatFlags = StringFormatFlags.LineLimit;
                        g.DrawString(cellValue, fCell, Brushes.Black, rect, sf);
                    }
                    x += colWidths[i];
                }

                y += rowHeight;
                currentRowIndex++;
            }

            currentRowIndex = 0;
            e.HasMorePages = false;
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int? personId = null;
                if (cmbRecipient.SelectedValue != null && cmbRecipient.SelectedValue != DBNull.Value)
                {
                    int val = Convert.ToInt32(cmbRecipient.SelectedValue);
                    if (val != -1) personId = val;
                }

                bool isUpdated = _service.UpdateCashOrder(this._orderId, personId, txtEditBasis.Text, txtEditAppendix.Text);

                if (isUpdated)
                {
                    MessageBox.Show("Данные РКО успешно изменены!");

                    LoadOrderDataForPrint(_orderId);
                    LoadRkoRegistryTable();

                    if (dgvData.DataSource != null)
                    {
                        DataTable dt = (DataTable)dgvData.DataSource;
                        if (dt.Rows.Count > 2)
                        {
                            dt.Rows[2]["1"] = cmbRecipient.Text;
                            dt.Rows[2]["1а"] = _personPassportData;
                            dt.Rows[2]["3"] = txtEditAppendix.Text;
                        }
                    }

                    UpdatePreview();
                }
                else
                {
                    MessageBox.Show("Не удалось найти запись для обновления.");
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка при сохранении: " + ex.Message); }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}