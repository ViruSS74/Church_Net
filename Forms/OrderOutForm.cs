using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class OrderOutForm : Form
    {
        // 1. ОБЪЯВЛЯЕМ ПЕРЕМЕННЫЕ ЗДЕСЬ (внутри класса, но вне методов)
        private int _orderId;
        private int _docId;
        private ListOfDocsService _service;

        // Эти переменные будут хранить данные из БД для печати
        private string _orgName = "";
        private string _dateDay = "";
        private string _dateMonth = "";
        private string _dateYear = "";

        // Новые переменные для "Принято от"
        private string _personNameFull = "";     // Для Ордера (в одну строку)
        private string _personLastName = "";     // Для Квитанции (Фамилия)
        private string _personFirstMiddle = "";   // Для Квитанции (Имя Отчество)
        private string _orderAppendix = "";      // Для хранения текста "Приложение"
        private string _personPassportData = "";  // Для хранения данных паспорта
        private string _orderNumber = "";
        private string _orderDate = "";
        private double _orderAmount = 0;
        private string _orderBase = "";
        private string _rectorName = "";
        private string _treasurerName = "";

        public OrderOutForm(int orderId, ListOfDocsService service)
        {
            InitializeComponent();

            // Исправляем CS0103: присваиваем переданный orderId нашим полям
            this._orderId = orderId;
            this._docId = orderId; // Предполагаем, что docId и orderId это одно и то же

            // Используем переданный сервис или создаем новый, если пришел null
            this._service = service ?? new ListOfDocsService(Program.DbPath);

            // 2. ЗАГРУЗКА ДАННЫХ
            LoadOrderData();    // Загрузка текстовых полей бланка
            FillRecipients();   // Заполнение списка получателей
            ApplyRkoGridStyle();
            LoadRkoRegistryTable();

            // --- ВАЖНО: ВЫЗОВ НАШЕЙ ТАБЛИЦЫ РКО ---
            // Привязываем событие отрисовки
            this.printRKOTitle.PrintPage += new PrintPageEventHandler(PrintOrderPage);

            // Обновляем превью
            ppControl.InvalidatePreview();
        }

        // 2. Метод загрузки данных
        private void LoadOrderDataForPrint(int id)
        {
            // Получаем ТАБЛИЦУ из сервиса
            DataTable dt = _service.GetRkoReportData(id);

            // Проверяем, что таблица не пуста и в ней есть строки
            if (dt != null && dt.Rows.Count > 0)
            {
                // Берем первую строку из таблицы
                DataRow row = dt.Rows[0];

                _orderNumber = row["No"].ToString();
                _orderAmount = Convert.ToDouble(row["Sum"]);
                _orderBase = row["Basis"].ToString();
                _orderAppendix = row["Appendix"].ToString();
                _personPassportData = row["Passport"].ToString();

                DateTime dtDate = Convert.ToDateTime(row["Date"]);
                _dateDay = dtDate.Day.ToString("D2");
                _dateMonth = dtDate.ToString("MMMM");
                _dateYear = dtDate.Year.ToString();
            }
        }

        // 3. Основной метод верстки GDI+
        private void PrintKO2_Page(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontRegular = new Font("Arial", 9, FontStyle.Regular);
            Pen pen = new Pen(Color.Black, 1);

            // Шапка формы КО-2
            g.DrawString("РАСХОДНЫЙ КАССОВЫЙ ОРДЕР", fontBold, Brushes.Black, 250, 100);
        }

        private void FillRecipients()
        {
            DataTable dt = _service.GetRecipients(); // Ваш метод получения данных

            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["full_name"] = "-- Не указан --";
            dt.Rows.InsertAt(dr, 0); // Вставляем первой строкой

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

                // 1. Загружаем персонал для определения Настоятеля "по умолчанию"
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
                            // Склоняем настоятеля заранее на случай, если в ордере пусто
                            rectorFull = _service.GetPersonDative(lName, fName, mName);
                            rectorLast = lName;
                        }
                        if (role.Contains("казначей")) _treasurerName = sn;
                    }
                }

                // 2. Данные организации
                DataRow orgRow = _service.GetOrganizationData();
                if (orgRow != null)
                    _orgName = (orgRow["name"] ?? "").ToString() + " " + (orgRow["location"] ?? "").ToString();

                // 3. Данные ордера (используем ваш новый метод с JOIN)
                DataRow orderRow = _service.GetCashOrderData(_orderId);
                if (orderRow != null)
                {
                    _orderNumber = (orderRow["order_number"] ?? "").ToString();
                    _orderAmount = orderRow["amount"] != DBNull.Value ? Convert.ToDouble(orderRow["amount"]) : 0;
                    _orderBase = (orderRow["base"] ?? "").ToString();
                    _orderAppendix = (orderRow["appendix"] ?? "").ToString(); // Не забудьте приложение

                    // Паспортные данные из вашего нового SQL (full_passport)
                    _personPassportData = (orderRow["full_passport"] ?? "").ToString();

                    // --- ЛОГИКА ОБНОВЛЕНИЯ ФАМИЛИИ (ВЫДАТЬ) ---
                    // Проверяем, привязан ли сотрудник из справочника (смотрим на колонку last_name из JOIN)
                    if (orderRow["last_name"] != DBNull.Value && !string.IsNullOrEmpty(orderRow["last_name"].ToString()))
                    {
                        // Если фамилия в справочнике изменилась, она придет сюда новой из БД
                        string ln = orderRow["last_name"].ToString();
                        string fn = orderRow["first_name"].ToString();
                        string mn = orderRow["middle_name"].ToString();

                        // Склоняем актуальные данные
                        _personNameFull = _service.GetPersonDative(ln, fn, mn);
                        _personLastName = ln;
                        _personFirstMiddle = string.Format("{0} {1}", fn, mn).Trim();
                    }
                    else
                    {
                        // Если сотрудник не привязан, проверяем ручной ввод (person_name_manual)
                        string manualPerson = (orderRow["person_name_manual"] ?? "").ToString();

                        if (!string.IsNullOrEmpty(manualPerson))
                        {
                            _personNameFull = manualPerson; // Оставляем как есть (уже может быть в падеже)
                            _personLastName = manualPerson;
                        }
                        else
                        {
                            // Если совсем пусто — ставим Настоятеля
                            _personNameFull = rectorFull;
                            _personLastName = rectorLast;
                        }
                        _personFirstMiddle = "";
                    }

                    // Обработка даты
                    if (orderRow["date"] != DBNull.Value)
                    {
                        DateTime dt = Convert.ToDateTime(orderRow["date"]);
                        _dateDay = dt.Day.ToString("D2");
                        _dateMonth = dt.ToString("MMMM", new System.Globalization.CultureInfo("ru-RU"));
                        _dateYear = dt.Year.ToString();
                    }
                }

                // Обновляем визуальную часть
                if (ppControl != null)
                {
                    ppControl.InvalidatePreview();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при обновлении данных бланка: " + ex.Message);
            }
        }

        private string GetShortName(string last, string first, string middle)
        {
            string f = (first.Length > 0) ? first.Substring(0, 1) + "." : "";
            string m = (middle.Length > 0) ? middle.Substring(0, 1) + "." : "";
            return string.Format("{0}{1} {2}", f, m, last); // Результат: С.А. Солодышев
        }

        private void PrintOrderPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fTitle = new Font("Arial", 10, FontStyle.Bold);
            Font fReg = new Font("Arial", 9, FontStyle.Regular);
            Font fSmall = new Font("Arial", 7);
            Pen pThin = new Pen(Color.Black, 0.5f);

            int x = 40;
            int y = 50; // Стартовая точка
            int lineW = 720; // Общая ширина линий

            // 1. ШАПКА
            float curX = x;
            string titlePart = "РАСХОДНЫЙ КАССОВЫЙ ОРДЕР № ";
            g.DrawString(titlePart, fTitle, Brushes.Black, curX, y);
            curX += g.MeasureString(titlePart, fTitle).Width - 5;
            // 1.1 Номер ордера
            g.DrawString(_orderNumber, fReg, Brushes.Black, curX + 5, y - 2);
            g.DrawLine(pThin, curX, y + 14, curX + 110, y + 14);
            curX += 120;
            // 1.2 Число в кавычках: « 16 »
            g.DrawString("«", fTitle, Brushes.Black, curX, y);
            g.DrawString(_dateDay, fReg, Brushes.Black, curX + 15, y - 2);
            g.DrawLine(pThin, curX + 12, y + 14, curX + 35, y + 14); // Линия под числом
            g.DrawString("»", fTitle, Brushes.Black, curX + 38, y);
            curX += 55;
            // 1.3 Месяц
            float monthWidth = 120; // Длинная линия для месяца
            float monthTextWidth = g.MeasureString(_dateMonth, fReg).Width;
            float monthPadding = (monthWidth - monthTextWidth) / 2; // Центровка
            g.DrawString(_dateMonth, fReg, Brushes.Black, curX + monthPadding, y - 2);
            g.DrawLine(pThin, curX, y + 14, curX + monthWidth, y + 14);
            curX += monthWidth + 10;
            // 1.4 Год
            g.DrawString("20", fTitle, Brushes.Black, curX, y);
            g.DrawString(_dateYear.Substring(_dateYear.Length - 2), fReg, Brushes.Black, curX + 22, y - 2);
            g.DrawLine(pThin, curX + 20, y + 14, curX + 45, y + 14);
            g.DrawString(" г.", fTitle, Brushes.Black, curX + 45, y);

            // 2. ТАБЛИЦА (СУММА)
            y += 35;
            g.DrawRectangle(pThin, x, y, lineW, 35);
            g.DrawLine(pThin, x + 500, y, x + 500, y + 35); // Разделитель
            g.DrawLine(pThin, x, y + 15, x + lineW, y + 15);
            g.DrawString("Корреспондирующий счет, субсчет", fSmall, Brushes.Black, x + 150, y + 2);
            g.DrawString("Сумма, руб. коп.", fSmall, Brushes.Black, x + 560, y + 2);
            g.DrawString(_orderAmount.ToString("N2"), fTitle, Brushes.Black, x + 580, y + 18);

            // 3. ВЫДАТЬ
            y += 50;
            g.DrawString("Выдать", fReg, Brushes.Black, x, y);
            g.DrawString(_personNameFull, fReg, Brushes.Black, x + 80, y - 2);
            g.DrawLine(pThin, x + 75, y + 12, x + x + lineW, y + 12);
            g.DrawString("(фамилия, собственное имя и отчество (если таковое имеется))", fSmall, Brushes.Black, x + 250, y + 14);

            // 4. ОСНОВАНИЕ
            y += 40;
            g.DrawString("Основание", fReg, Brushes.Black, x, y);
            g.DrawString(_orderBase, fReg, Brushes.Black, x + 80, y - 2);
            g.DrawLine(pThin, x + 75, y + 12, x + x + lineW, y + 12);

            // 5. СУММА ПРОПИСЬЮ (Исправлено дублирование)
            y += 35;
            string cleanSum = CurrencyToWords(_orderAmount).Replace(" руб. 00 коп.", "").Trim();
            string kopeks = ((int)((_orderAmount % 1) * 100)).ToString("00");

            g.DrawString("Сумма", fReg, Brushes.Black, x, y);
            g.DrawString(char.ToUpper(cleanSum[0]) + cleanSum.Substring(1), fReg, Brushes.Black, x + 80, y - 2);
            g.DrawLine(pThin, x + 75, y + 12, x + 580, y + 12);
            g.DrawString("руб.", fReg, Brushes.Black, x + 585, y);
            g.DrawString(kopeks, fReg, Brushes.Black, x + 640, y - 2);
            g.DrawLine(pThin, x + 625, y + 12, x + 690, y + 12);
            g.DrawString("коп.", fReg, Brushes.Black, x + 695, y);

            // 6. ПРИЛОЖЕНИЕ
            y += 35;
            g.DrawString("Приложение", fReg, Brushes.Black, x, y);
            g.DrawString(_orderAppendix, fReg, Brushes.Black, x + 90, y - 2);
            g.DrawLine(pThin, x + 85, y + 12, x + x + lineW, y + 12);

            // 7. НАСТОЯТЕЛЬ
            y += 45;
            g.DrawString("Настоятель храма  _________________ ", fReg, Brushes.Black, x, y);
            g.DrawString(_rectorName, fReg, Brushes.Black, x + 580, y - 2);
            g.DrawLine(pThin, x + 575, y + 12, x + x + lineW, y + 12);
            g.DrawString("(подпись)", fSmall, Brushes.Black, x + 150, y + 13);
            g.DrawString("(инициалы, фамилия)", fSmall, Brushes.Black, x + 600, y + 13);

            // 8. ПОЛУЧИЛ
            y += 40;
            g.DrawString("Получил  _________________ ", fReg, Brushes.Black, x, y);
            g.DrawString("(подпись получателя)", fSmall, Brushes.Black, x + 100, y + 13);

            // 9. ПАСПОРТ
            y += 35;
            g.DrawString("Предъявлен документ", fReg, Brushes.Black, x, y);
            g.DrawString(_personPassportData, fReg, Brushes.Black, x + 150, y - 2);
            g.DrawLine(pThin, x + 145, y + 12, x + x + lineW, y + 12);
            g.DrawString("(данные о документе, удостоверяющем личность получателя)", fSmall, Brushes.Black, x + 250, y + 13);

            // 10. КАЗНАЧЕЙ
            y += 45;
            g.DrawString("Выдал казначей  _________________ ", fReg, Brushes.Black, x, y);
            g.DrawString(_treasurerName, fReg, Brushes.Black, x + 580, y - 2);
            g.DrawLine(pThin, x + 575, y + 12, x + x + lineW, y + 12);
        }

        public string CurrencyToWords(double amount)
        {
            long rub = (long)Math.Floor(amount);
            int kop = (int)Math.Round((amount - rub) * 100);

            // Если сумма 0
            if (rub == 0) return "Ноль рублей " + kop.ToString("D2") + " коп.";

            string[] ones = { "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
            string[] tens = { "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
            string[] teens = { "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
            string[] hundreds = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };

            // Для простоты возьмем только до миллионов (в церкви больше редко бывает)
            string result = "";

            if (rub >= 1000)
            {
                long thousands = rub / 1000;
                result += thousands.ToString() + " тыс. "; // Упростим тысячи цифрами для надежности
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

        private void ApplyRkoGridStyle()
        {
            if (dgvData.Columns.Count < 6) return;

            dgvData.RowHeadersVisible = false;
            dgvData.AllowUserToAddRows = false;
            dgvData.GridColor = Color.Black;
            dgvData.ColumnHeadersHeight = 85;
            dgvData.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);

            // Названия и ширина (согласно скриншоту)
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

            // Авто-растяжение для основания
            dgvData.Columns["2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Форматирование ячеек
            dgvData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Стиль первой строки (где 1, 1а...)
            if (dgvData.Rows.Count > 0)
            {
                dgvData.Rows[0].DefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);
                dgvData.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvData.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        private void LoadRkoRegistryTable()
        {
            try
            {
                DataTable dt = new DataTable();
                // Создаем структуру как на вашем скриншоте (6 колонок)
                string[] cols = { "1", "1а", "2", "3", "4", "4а" };
                foreach (var c in cols) dt.Columns.Add(c);

                // Строка с цифрами
                dt.Rows.Add("1", "1а", "2", "3", "4", "4а");

                // Получаем чистые данные из сервиса
                DataTable dbData = _service.GetRkoRegistryData();
                string currentDoc = cmbDocs.Text; // Документ из комбобокса сверху

                bool firstRow = true;
                foreach (DataRow dr in dbData.Rows)
                {
                    // 1. Проверяем паспорт: если данных в БД нет, оставляем пустоту (никаких ".выдан")
                    string passport = dr["passport_full"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(passport) || passport.Trim() == ", выдан")
                    {
                        passport = "";
                    }

                    dt.Rows.Add(
                        firstRow ? (dr["last_name"].ToString() + " " + dr["first_mid"].ToString()) : "", // ФИО только в 1-й строке
                        firstRow ? passport : "",      // Паспорт только в 1-й строке
                        dr["expense_reason"],          // Основание (Отопление, Электроэнергия) — будет в каждой строке!
                        firstRow ? currentDoc : "",    // Наименование документа (Чек) — только в 1-й строке
                        firstRow ? "BYN" : "",         // Код валюты — только в 1-й строке
                        firstRow ? "Белорусский рубль" : "" // Название валюты — только в 1-й строке
                    );
                    firstRow = false;
                }

                // Пустые строки для заполнения листа
                for (int i = 0; i < 12; i++) dt.Rows.Add("", "", "", "", "", "");

                dgvData.DataSource = dt;

                // Применяем настройки внешнего вида
                ApplyRkoGridStyle();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка отрисовки таблицы: " + ex.Message); }
        }

        private void cmbDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvData.DataSource == null) return;
            DataTable dt = dgvData.DataSource as DataTable;
            if (dt == null || dt.Rows.Count < 1) return;

            string selectedDoc = cmbDocs.Text;

            // В вашем SQL столбец называется "3"
            if (dt.Columns.Contains("3"))
            {
                // Если строк несколько, обычно документ пишется в первую или во все
                // Для примера пишем в первую строку:
                dt.Rows[0]["3"] = selectedDoc;

                // Если нужно очистить остальные:
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["3"] = string.Empty;
                }
            }
            dgvData.Refresh();
        }

        // Обработка кнопок
        private void btnView_Click(object sender, EventArgs e)
        {
            // Создаем документ для печати
            PrintDocument pd = new PrintDocument();
            // Подписываем его на наш метод рисования
            pd.PrintPage += new PrintPageEventHandler(PrintOrderPage);

            // Создаем окно предпросмотра
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;

            // На Windows 8 и выше это окно будет выглядеть современно и аккуратно
            ppd.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument pd = new PrintDocument();

                // Проверяем, какая вкладка выбрана (используем Name вкладки)
                if (tabRKO.SelectedTab.Name == "tabPrintForm")
                {
                    pd.DefaultPageSettings.Landscape = false;
                    pd.PrintPage += new PrintPageEventHandler(PrintOrderPage);
                }
                else if (tabRKO.SelectedTab.Name == "tabData")
                {
                    // ПРИНУДИТЕЛЬНО устанавливаем альбомную ориентацию
                    pd.DefaultPageSettings.Landscape = true;
                    // Некоторые принтеры требуют установки Landscape и в PrinterSettings
                    pd.PrinterSettings.DefaultPageSettings.Landscape = true;

                    pd.PrintPage += new PrintPageEventHandler(PrintRegistryPage);
                }

                PrintDialog pDialog = new PrintDialog();
                pDialog.Document = pd;
                pDialog.UseEXDialog = true;

                if (pDialog.ShowDialog() == DialogResult.OK)
                {
                    pd.Print();
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
            // Принудительно устанавливаем альбомную ориентацию
            e.PageSettings.Landscape = true;
            Graphics g = e.Graphics;
            // Настройки шрифтов
            Font fHeader = new Font("Arial", 8, FontStyle.Bold);
            Font fCell = new Font("Arial", 8);
            Pen pen = new Pen(Color.Black, 1);
            int x = 40;
            int y = 40;
            // Ширины колонок (индекс 4 — "Основание")
            int[] colWidths = { 130, 160, 50, 120, 480 };
            int headerHeight = 60;
            // 1. Отрисовка шапки таблицы
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
            // 2. Отрисовка строк данных
            while (currentRowIndex < dgvData.Rows.Count)
            {
                DataGridViewRow row = dgvData.Rows[currentRowIndex];
                if (row.IsNewRow) { currentRowIndex++; continue; }
                // Расчет динамической высоты по колонке "Основание" (индекс 4)
                string basisText = (row.Cells[4].Value ?? "").ToString();
                // MeasureString учитывает переносы при заданной ширине колонки
                SizeF size = g.MeasureString(basisText, fCell, colWidths[4]);
                int rowHeight = Math.Max(25, (int)Math.Ceiling(size.Height) + 10); // +10 для полей
                // Проверка: влезет ли строка на текущую страницу
                if (y + rowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true; // Будет вызван этот же метод для следующей страницы
                    return;
                }
                x = 40;
                for (int i = 0; i < dgvData.Columns.Count; i++)
                {
                    Rectangle rect = new Rectangle(x, y, colWidths[i], rowHeight);
                    g.DrawRectangle(pen, rect);
                    string cellValue = (row.Cells[i].Value ?? "").ToString();
                    using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Near })
                    {
                        // Выравнивание: номера колонок (строка 0) по центру, остальное по левому краю
                        sf.Alignment = (currentRowIndex == 0) ? StringAlignment.Center : StringAlignment.Near;
                        // Включаем перенос слов для длинного текста
                        sf.FormatFlags = StringFormatFlags.LineLimit;
                        g.DrawString(cellValue, fCell, Brushes.Black, rect, sf);
                    }
                    x += colWidths[i];
                }
                y += rowHeight;
                currentRowIndex++;
            }
            // Сбрасываем индекс после завершения всей печати
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

                // Обновляем базу
                bool isUpdated = _service.UpdateCashOrder(this._orderId, personId, txtEditBasis.Text, txtEditAppendix.Text);

                if (isUpdated)
                {
                    MessageBox.Show("Данные РКО успешно изменены!");

                    // 1. Обновляем титул (шапку)
                    LoadOrderDataForPrint(_orderId);

                    // 2. ДОБАВИТЬ ЭТУ СТРОКУ: Обновляем таблицу под бланком
                    LoadRkoRegistryTable();

                    ppControl.InvalidatePreview();
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
