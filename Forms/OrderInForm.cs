using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;  // ✅ Должен быть!
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace ChurchBudget.Forms
{
    public partial class OrderInForm : Form
    {
        private int _orderId;
        private ListOfDocsService _service;

        private string _orgName = "";
        private string _dateDay = "";
        private string _dateMonth = "";
        private string _dateYear = "";
        private string _personNameFull = "";
        private string _personLastName = "";
        private string _personFirstMiddle = "";
        private string _orderNumber = "";
        private string _appendix = "";
        private double _orderAmount = 0;
        private string _orderBase = "";
        private string _rectorName = "";
        private string _treasurerName = "";
        private string _currentHtml = "";

        public OrderInForm(int orderId, ListOfDocsService service)
        {
            InitializeComponent();
            _orderId = orderId;
            _service = service;

            FillRecipientCombo();
            LoadOrderData();
            LoadPkoRegistry();

            this.cmbRecipient.SelectedIndexChanged += new EventHandler(cmbRecipient_SelectedIndexChanged);

            // Генерируем HTML и показываем в превью
            try
            {
                _currentHtml = GeneratePkoHtml();
                wbPreview.DocumentText = _currentHtml;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка генерации HTML: " + ex.Message);
            }

            ImageHelper.ApplyToButtons(this, 24);
        }

        private void FillRecipientCombo()
        {
            DataTable dt = _service.GetRecipients();
            cmbRecipient.DataSource = dt;
            cmbRecipient.DisplayMember = "full_name";
            cmbRecipient.ValueMember = "id";
            cmbRecipient.SelectedValue = 2;
        }

        private void cmbRecipient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRecipient.SelectedValue == null ||
                cmbRecipient.SelectedValue == DBNull.Value ||
                !int.TryParse(cmbRecipient.SelectedValue.ToString(), out int personId))
            {
                return;
            }

            if (personId == -1) return;

            try
            {
                string fullFio = cmbRecipient.Text;
                string[] parts = fullFio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string last = (parts.Length > 0) ? parts[0] : "";
                string first = (parts.Length > 1) ? parts[1] : "";
                string middle = (parts.Length > 2) ? parts[2] : "";

                _personNameFull = _service.GetPersonGenitive(last, first, middle);
                _personLastName = last;
                _personFirstMiddle = (first + " " + middle).Trim();

                UpdatePreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выборе получателя: " + ex.Message);
            }
        }

        private void LoadOrderData()
        {
            try
            {
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
                    _orderAmount = orderRow["amount"] != DBNull.Value ? Convert.ToDouble(orderRow["amount"]) : 0;
                    _orderBase = "Рапортичка";

                    string dbPerson = (orderRow["person"] ?? "").ToString();

                    if (string.IsNullOrEmpty(dbPerson))
                    {
                        string treasurerFull = "";
                        string treasurerLast = "";
                        string treasurerFM = "";

                        foreach (DataRow row in personal.Rows)
                        {
                            string role = (row["role"] ?? "").ToString().ToLower();
                            if (role.Contains("казначей"))
                            {
                                treasurerLast = row["last_name"].ToString();
                                string fName = row["first_name"].ToString();
                                string mName = row["middle_name"].ToString();
                                treasurerFull = $"{treasurerLast} {fName} {mName}".Trim();
                                treasurerFM = $"{fName} {mName}".Trim();
                                break;
                            }
                        }

                        _personNameFull = treasurerFull;
                        _personLastName = treasurerLast;
                        _personFirstMiddle = treasurerFM;
                    }
                    else
                    {
                        _personNameFull = dbPerson;
                        _personLastName = dbPerson;
                        _personFirstMiddle = "";
                    }

                    if (orderRow["date"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(orderRow["date"]);
                        _dateDay = dt.Day.ToString("D2");
                        _dateMonth = dt.ToString("MMMM", new System.Globalization.CultureInfo("ru-RU"));
                        _dateYear = dt.Year.ToString();
                    }

                    if (orderRow["appendix"] != DBNull.Value)
                        _appendix = orderRow["appendix"].ToString();
                    // Всегда используем "Рапортичка" как основание
                    _orderBase = "Рапортичка";
                }

                txtEditBasis.Text = _orderBase;
                txtEditAppendix.Text = _appendix;

                UpdatePreview();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки ПКО: " + ex.Message); }
        }

        private void LoadPkoRegistry()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("1"); dt.Columns.Add("1а");
                dt.Columns.Add("2"); dt.Columns.Add("2а");
                dt.Columns.Add("3");

                dt.Rows.Add("1", "1а", "2", "2а", "3");

                string treasurerLast = ""; string treasurerFull = "";
                string rectorLast = ""; string rectorFull = "";

                DataTable staff = _service.GetPersonalList();
                foreach (DataRow r in staff.Rows)
                {
                    string role = (r["role"] ?? "").ToString();
                    if (role == "Казначей")
                    {
                        treasurerLast = r["last_name"].ToString();
                        treasurerFull = r["first_name"].ToString() + " " + r["middle_name"].ToString();
                    }
                    if (role == "Настоятель храма")
                    {
                        rectorLast = r["last_name"].ToString();
                        rectorFull = r["first_name"].ToString() + " " + r["middle_name"].ToString();
                    }
                }

                dt.Rows.Add(treasurerLast, treasurerFull, "BYN", "Белорусский рубль", _orderBase);
                dt.Rows.Add(rectorLast, rectorFull, "", "", "");

                for (int i = 0; i < 16; i++)
                {
                    dt.Rows.Add("", "", "", "", "");
                }

                dgvData.DataSource = dt;
                dgvData.RowHeadersVisible = false;
                dgvData.AllowUserToAddRows = false;
                dgvData.GridColor = Color.Black;
                dgvData.BorderStyle = BorderStyle.FixedSingle;
                dgvData.ColumnHeadersDefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);
                dgvData.ColumnHeadersHeight = 85;
                dgvData.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvData.Columns["1"].HeaderText = "Фамилия физ. лица (наименование организации)";
                dgvData.Columns["1"].Width = 160;
                dgvData.Columns["1а"].HeaderText = "Собственное имя и отчество (если таковое имеется)";
                dgvData.Columns["1а"].Width = 230;
                dgvData.Columns["2"].HeaderText = "Код валюты";
                dgvData.Columns["2"].Width = 100;
                dgvData.Columns["2а"].HeaderText = "Наименование валюты";
                dgvData.Columns["2а"].Width = 250;
                dgvData.Columns["3"].HeaderText = "Частоприменяемые формулировки основания";
                dgvData.Columns["3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                dgvData.Rows[0].DefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);
                dgvData.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
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
                _currentHtml = GeneratePkoHtml();
                wbPreview.DocumentText = _currentHtml;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления превью: {ex.Message}");
            }
        }

        #region HTML Generation

        private string GeneratePkoHtml()
        {
            StringBuilder html = new StringBuilder();
            string sumWords = CurrencyToWords(_orderAmount);
            string sumWordsCapitalized = char.ToUpper(sumWords[0]) + sumWords.Substring(1);
            string wordsOnly = sumWordsCapitalized.Replace(" руб. 00 коп.", "").Trim();
            string rublesDigits = ((int)_orderAmount).ToString();
            string kopeksDigits = ((int)((_orderAmount % 1) * 100)).ToString("00");

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
            line-height: 1.3;
        }
        table.main-table {
            width: 190mm;
            border-collapse: collapse;
        }
        table.main-table td {
            vertical-align: top;
            padding: 0;
        }
        .left-part {
            width: 100mm;
            border-right: 2px dashed #000;
            padding-right: 2mm;
        }
        .right-part {
            width: 80mm;
            padding-left: 10mm;  /* Увеличили отступ от пунктира */
        }
        .title { font-weight: bold; font-size: 11pt; margin-bottom: 5px; }
        .line { 
            display: inline-block; 
            border-bottom: 1px solid #000; 
            min-width: 50px; 
            vertical-align: bottom;
        }
        .line-short { min-width: 25px; }
        .line-medium { min-width: 80px; }
        .line-long { min-width: 150px; }
        table.inner { border-collapse: collapse; width: 100%; margin: 5px 0; }
        table.inner td { border: 1px solid #000; padding: 3px 5px; vertical-align: middle; }
        .amount-cell { text-align: right; font-weight: bold; }
        .small { font-size: 8pt; }
        .signature-line { 
            display: inline-block; 
            border-bottom: 1px solid #000; 
            min-width: 80px; 
            margin: 0 5px;
        }
        @media print {
            body { margin: 0; padding: 10mm; }
            .no-print { display: none; }
        }
    </style>
</head>
<body>
<table class='main-table'>
<tr>
<td class='left-part'>");

            // === ЛЕВАЯ ЧАСТЬ: ПКО ===
            html.AppendLine(string.Format("<div class='title'>ПРИХОДНЫЙ КАССОВЫЙ ОРДЕР № {0}</div>", _orderNumber));

            html.AppendLine(string.Format("<div>Дата: <span class='line line-short'>{0}</span> <span class='line line-medium'>{1}</span> <span class='line line-short'>{2} г.</span></div>",
                _dateDay, _dateMonth, _dateYear));

            html.AppendLine("<table class='inner'>");
            html.AppendLine("<tr>");
            html.AppendLine("<td style='width:60%; vertical-align:top;'>");
            html.AppendLine("<div class='small'>Корреспондирующий счет, субсчет</div>");
            html.AppendLine("</td>");
            html.AppendLine("<td style='width:40%; vertical-align:top;'>");
            html.AppendLine("<div class='small'>Сумма, руб. коп.</div>");
            html.AppendLine(string.Format("<div class='amount-cell'>{0}</div>", _orderAmount.ToString("N2")));
            html.AppendLine("</td>");
            html.AppendLine("</tr>");
            html.AppendLine("</table>");

            html.AppendLine(string.Format("<div style='margin-top:10px;'>Принято от: <span class='line line-long'>{0}</span></div>", _personNameFull));
            html.AppendLine("<div class='small' style='margin-left:80px;'>(фамилия, собственное имя и отчество)</div>");

            html.AppendLine(string.Format("<div style='margin-top:10px;'>Основание: <span class='line line-long'>{0}</span></div>", _orderBase));

            html.AppendLine("<div class='small' style='margin-top:10px;'>Ставка НДС _______ %  Сумма НДС ___________ руб. ___ коп.</div>");

            html.AppendLine(string.Format("<div style='margin-top:10px;'>Сумма с НДС: <span class='line line-long'>{0}</span></div>", wordsOnly));
            html.AppendLine("<div class='small' style='margin-left:100px;'>(прописью)</div>");

            html.AppendLine(string.Format("<div style='margin-top:10px;'><span class='line line-short'>{0}</span> руб. <span class='line line-short'>{1}</span> коп.</div>",
                rublesDigits, kopeksDigits));
            html.AppendLine("<div class='small' style='margin-left:80px;'>(цифрами)</div>");

            html.AppendLine(string.Format("<div style='margin-top:10px;'>Приложение: <span class='line line-long'>{0}</span></div>", _appendix ?? ""));

            html.AppendLine(string.Format("<div style='margin-top:20px;'>Настоятель храма <span class='signature-line'></span> {0}</div>", _rectorName));
            html.AppendLine(string.Format("<div style='margin-top:10px;'>Получил казначей прихода <span class='signature-line'></span> {0}</div>", _treasurerName));

            html.AppendLine("</td>"); // end left-part

            // === ПРАВАЯ ЧАСТЬ: КВИТАНЦИЯ ===
            html.AppendLine("<td class='right-part'>");
            html.AppendLine(string.Format("<div class='small' style='border-bottom:1px solid #000; padding-bottom:3px;'>{0}</div>", _orgName));
            html.AppendLine("<div class='title' style='margin-top:10px;'>КВИТАНЦИЯ</div>");
            html.AppendLine("<div class='small'>к приходному кассовому ордеру</div>");
            html.AppendLine(string.Format("<div class='small'>№ <span class='line line-medium'>{0}</span></div>", _orderNumber));

            html.AppendLine(string.Format("<div style='margin-top:10px;'><span class='line line-short'>{0}</span> <span class='line line-medium'>{1}</span> <span class='line line-short'>{2} г.</span></div>",
                _dateDay, _dateMonth, _dateYear.Substring(2)));

            html.AppendLine(string.Format("<div style='margin-top:10px;'>Принято от: <span class='line line-long'>{0}</span></div>", _personLastName));
            if (!string.IsNullOrEmpty(_personFirstMiddle))
            {
                html.AppendLine(string.Format("<div style='margin-top:5px;'><span class='line line-long'>{0}</span></div>", _personFirstMiddle));
            }

            html.AppendLine(string.Format("<div style='margin-top:10px;'><span class='small'>Основание:</span> <span class='line line-long'>{0}</span></div>", _orderBase));

            html.AppendLine("<div style='margin-top:10px;'><span class='small'>Ставка НДС:</span> <span class='line line-short'>-</span> %</div>");
            html.AppendLine("<div style='margin-top:5px;'><span class='small'>Сумма НДС:</span> <span class='line line-medium'>-</span> руб. <span class='line line-short'>-</span> коп.</div>");

            html.AppendLine(string.Format("<div style='margin-top:10px;'><span class='small'>Сумма с НДС:</span> <span class='line line-long'>{0}</span></div>", wordsOnly));
            html.AppendLine("<div class='small' style='margin-left:80px;'>(прописью)</div>");
            html.AppendLine(string.Format("<div style='margin-top:5px;'><span class='line line-short'>{0}</span> руб. <span class='line line-short'>{1}</span> коп.</div>",
                rublesDigits, kopeksDigits));

            html.AppendLine(string.Format("<div style='margin-top:15px;'><span class='small'>Настоятель храма</span> <span class='signature-line'></span> {0}</div>", _rectorName));
            html.AppendLine(string.Format("<div style='margin-top:10px;'><span class='small'>Получил кассир</span> <span class='signature-line'></span> {0}</div>", _treasurerName));

            html.AppendLine("</td>"); // end right-part
            html.AppendLine("</tr>");
            html.AppendLine("</table>"); // end main-table

            html.AppendLine("</body></html>");

            return html.ToString();
        }

        public string CurrencyToWords(double amount)
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

        #endregion

        private void btnView_Click(object sender, EventArgs e)
        {
            tabPKO.SelectedIndex = 0;
            UpdatePreview();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument pd = new PrintDocument();

                if (tabPKO.SelectedTab.Name == "tabPrintForm")
                {
                    // Печать бланка ПКО через WebBrowser
                    wbPreview.Print();
                }
                else if (tabPKO.SelectedTab.Name == "tabData")
                {
                    // ✅ Печать реестра ПКО
                    pd.DefaultPageSettings.Landscape = true;
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

        private int currentRegistryRowIndex = 0;

        private void PrintRegistryPage(object sender, PrintPageEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[ПКО Печать] Всего строк: {dgvData.Rows.Count}");

            e.PageSettings.Landscape = true;
            Graphics g = e.Graphics;

            Font fHeader = new Font("Arial", 9, FontStyle.Bold);
            Font fCell = new Font("Arial", 8);
            Font fSmall = new Font("Arial", 7, FontStyle.Bold);
            Pen pen = new Pen(Color.Black, 1);

            int x = 30;
            int y = 30;

            // ✅ ПРАВИЛЬНЫЕ ширины колонок (из LoadPkoRegistry)
            int[] colWidths = { 160, 230, 100, 250, 200 };
            int headerHeight = 60;

            System.Diagnostics.Debug.WriteLine($"[ПКО Печать] Колонок: {dgvData.Columns.Count}");

            // 1. ШАПКА ТАБЛИЦЫ (из ColumnHeaders)
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
                System.Diagnostics.Debug.WriteLine($"[ПКО Печать] Печатаем строку с цифрами (индекс 0)");
                for (int i = 0; i < dgvData.Columns.Count; i++)
                {
                    Rectangle rect = new Rectangle(x, y, colWidths[i], 25);
                    g.DrawRectangle(pen, rect);

                    string cellValue = "";
                    if (dgvData.Rows[0].Cells[i].Value != null)
                    {
                        cellValue = dgvData.Rows[0].Cells[i].Value.ToString();
                    }

                    System.Diagnostics.Debug.WriteLine($"[ПКО Печать] Колонка {i}: '{cellValue}'");

                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString(cellValue, fSmall, Brushes.Black, rect, sf);
                    }
                    x += colWidths[i];
                }
            }

            y += 25;

            // 3. ДАННЫЕ (начиная со ВТОРОЙ строки, индекс 1)
            currentRegistryRowIndex = 1;  // ✅ НАЧИНАЕМ С ИНДЕКСА 1 (не 2!)
            while (currentRegistryRowIndex < dgvData.Rows.Count)
            {
                DataGridViewRow row = dgvData.Rows[currentRegistryRowIndex];
                if (row.IsNewRow) { currentRegistryRowIndex++; continue; }

                // Расчёт высоты строки по последней колонке (индекс 4)
                string basisText = (row.Cells[4].Value ?? "").ToString();
                SizeF size = g.MeasureString(basisText, fCell, colWidths[4]);
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
                currentRegistryRowIndex++;
            }

            currentRegistryRowIndex = 0;
            e.HasMorePages = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fullFio = cmbRecipient.Text;
            string[] parts = fullFio.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string last = parts.Length > 0 ? parts[0] : "";
            string first = parts.Length > 1 ? parts[1] : "";
            string middle = parts.Length > 2 ? parts[2] : "";

            _personNameFull = _service.GetPersonGenitive(last, first, middle);

            _service.UpdatePkoRecord(_orderId, _personNameFull, txtEditBasis.Text, txtEditAppendix.Text, _orderAmount);

            _orderBase = txtEditBasis.Text;
            _appendix = txtEditAppendix.Text;

            LoadPkoRegistry();
            UpdatePreview();
            tabPKO.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}