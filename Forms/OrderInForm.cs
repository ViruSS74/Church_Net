using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class OrderInForm : Form
    {
        // 1. ОБЪЯВЛЯЕМ ПЕРЕМЕННЫЕ ЗДЕСЬ (внутри класса, но вне методов)
        private int _orderId;
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

        private string _orderNumber = "";
        private string _orderDate = "";
        private double _orderAmount = 0;
        private string _orderBase = "";
        private string _rectorName = "";
        private string _treasurerName = "";

        public OrderInForm(int orderId, ListOfDocsService service)
        {
            InitializeComponent();

            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            _orderId = orderId;
            _service = service;

            // 2. ВЫЗЫВАЕМ ЗАГРУЗКУ ДАННЫХ
            LoadOrderData();    // Загрузка для печати
            LoadPkoRegistry();  // ЗАПОЛНЕНИЕ ТАБЛИЦЫ НА ВКЛАДКЕ ДАННЫЕ

            // Привязываем событие отрисовки
            this.printPKOTitle.PrintPage += new PrintPageEventHandler(PrintOrderPage);

            // ВАЖНО: Заставляем контрол перечитать документ и нарисовать его заново
            ppControl.InvalidatePreview();
        }

        private void LoadOrderData()
        {
            try
            {
                string rectorFull = "";
                string rectorLast = "";
                string rectorFM = "";

                // 1. Сначала загружаем персонал (чтобы знать Настоятеля заранее)
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
                            rectorFull = string.Format("{0} {1} {2}", lName, fName, mName).Trim();
                            rectorLast = lName;
                            rectorFM = string.Format("{0} {1}", fName, mName).Trim();
                        }
                        if (role.Contains("казначей")) _treasurerName = sn;
                    }
                }

                // 2. Данные организации
                DataRow orgRow = _service.GetOrganizationData();
                if (orgRow != null)
                    _orgName = (orgRow["name"] ?? "").ToString() + " " + (orgRow["location"] ?? "").ToString();

                // 3. Данные ордера
                DataRow orderRow = _service.GetCashOrderData(_orderId);
                if (orderRow != null)
                {
                    _orderNumber = (orderRow["order_number"] ?? "").ToString();
                    _orderAmount = orderRow["amount"] != DBNull.Value ? Convert.ToDouble(orderRow["amount"]) : 0;
                    _orderBase = (orderRow["base"] ?? "").ToString();

                    // Логика "Принято от": если в БД пусто, берем данные Настоятеля
                    string dbPerson = (orderRow["person"] ?? "").ToString();

                    if (string.IsNullOrEmpty(dbPerson))
                    {
                        _personNameFull = rectorFull;
                        _personLastName = rectorLast;
                        _personFirstMiddle = rectorFM;
                    }
                    else
                    {
                        _personNameFull = dbPerson;
                        _personLastName = dbPerson; // Если ввели вручную, пишем в первую строку
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

                // Обновляем превью
                if (ppControl != null) ppControl.InvalidatePreview();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка загрузки ПКО: " + ex.Message);
            }
        }

        private void LoadPkoRegistry()
        {
            try
            {
                // 1. Создаем структуру таблицы вручную
                DataTable dt = new DataTable();
                dt.Columns.Add("1"); dt.Columns.Add("1а");
                dt.Columns.Add("2"); dt.Columns.Add("2а");
                dt.Columns.Add("3");

                // 2. Строка с цифрами (будет первой)
                dt.Rows.Add("1", "1а", "2", "2а", "3");

                // 3. Получаем данные сотрудников из вашего сервиса (используя роли из БД)
                // Строка с Казначеем — здесь пишем валюту и основание (один раз!)
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

                // Заполняем: Казначей (с валютой и услугами)
                dt.Rows.Add(treasurerLast, treasurerFull, "BYR", "Белорусский рубль", _orderBase);

                // Заполняем: Настоятель (валюта и основание пустые, чтобы не дублировать)
                dt.Rows.Add(rectorLast, rectorFull, "", "", "");

                // 4. Добавляем пустые строки для эффекта "полного листа" (например, 15 строк)
                for (int i = 0; i < 15; i++)
                {
                    dt.Rows.Add("", "", "", "", "");
                }

                dgvData.DataSource = dt;

                // --- НАСТРОЙКА ВИЗУАЛА dgvData ---
                dgvData.RowHeadersVisible = false;
                dgvData.AllowUserToAddRows = false;
                dgvData.GridColor = Color.Black; // Четкая сетка для печати
                dgvData.BorderStyle = BorderStyle.FixedSingle;

                // Жирный шрифт для шапки и первой строки
                dgvData.ColumnHeadersDefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);
                dgvData.ColumnHeadersHeight = 85;
                dgvData.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // Заголовки (Длинные названия)
                dgvData.Columns["1"].HeaderText = "Фамилия физ. лица (наименование организации)";
                dgvData.Columns["1"].Width = 140;
                dgvData.Columns["1а"].HeaderText = "Собственное имя и отчество (если таковое имеется)";
                dgvData.Columns["1а"].Width = 180;
                dgvData.Columns["2"].HeaderText = "Код валюты";
                dgvData.Columns["2"].Width = 60;
                dgvData.Columns["2а"].HeaderText = "Наименование валюты";
                dgvData.Columns["2а"].Width = 140;
                dgvData.Columns["3"].HeaderText = "Частоприменяемые формулировки основания";
                dgvData.Columns["3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Выравнивание текста ПО ВЕРХНЕМУ КРАЮ
                dgvData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvData.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Чтобы основание переносилось
                dgvData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // Выделение первой строки (1, 1а...) жирным
                dgvData.Rows[0].DefaultCellStyle.Font = new Font(dgvData.Font, FontStyle.Bold);
                dgvData.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
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
            Font fBold = new Font("Arial", 9, FontStyle.Bold);
            Font fReg = new Font("Arial", 8, FontStyle.Regular);
            Font fSmall = new Font("Arial", 6);
            Pen pThin = new Pen(Color.Black, 0.5f);
            Pen pDash = new Pen(Color.Black, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };

            int x = 40; int y = 40; int w = 460;
            int gap = 20;
            int rx = x + w + gap;
            int rw = 210; // Увеличил для лучшей вместимости квитанции

            // 0. ОБЪЯВЛЯЕМ ОБЩУЮ ПЕРЕМЕННУЮ В НАЧАЛЕ МЕТОДА PrintPage
            string rawSum = CurrencyToWords(_orderAmount);
            string sumWords = char.ToUpper(rawSum[0]) + rawSum.Substring(1);

            // --- ЛЕВАЯ ЧАСТЬ: ПКО ---
            g.DrawString("ПРИХОДНЫЙ КАССОВЫЙ ОРДЕР № " + _orderNumber, fBold, Brushes.Black, x, y);
            y += 25;
            g.DrawString(_dateDay, fReg, Brushes.Black, x + 5, y);
            g.DrawLine(pThin, x, y + 12, x + 25, y + 12);
            g.DrawString(_dateMonth, fReg, Brushes.Black, x + 40, y);
            g.DrawLine(pThin, x + 30, y + 12, x + 120, y + 12);
            g.DrawString(_dateYear + " г.", fReg, Brushes.Black, x + 125, y);

            y += 30;
            g.DrawRectangle(pThin, x, y, w - 20, 45);
            g.DrawLine(pThin, x + 250, y, x + 250, y + 45);
            g.DrawLine(pThin, x, y + 20, x + w - 20, y + 20);
            g.DrawString("Корреспондирующий счет, субсчет", fSmall, Brushes.Black, x + 40, y + 5);
            g.DrawString("Сумма, руб. коп.", fSmall, Brushes.Black, x + 290, y + 5);
            g.DrawString(_orderAmount.ToString("N2"), fBold, Brushes.Black, x + 300, y + 25);

            // Принято от (Ордер)
            y += 60;
            g.DrawString("Принято от:", fReg, Brushes.Black, x, y);
            g.DrawString(_personNameFull, fReg, Brushes.Black, x + 80, y - 2);
            g.DrawLine(pThin, x + 75, y + 12, x + w - 20, y + 12);
            g.DrawString("(фамилия, собственное имя и отчество)", fSmall, Brushes.Black, x + 150, y + 14);

            y += 35;
            g.DrawString("Основание:", fReg, Brushes.Black, x, y);
            g.DrawString(_orderBase, fReg, Brushes.Black, x + 75, y);
            g.DrawLine(pThin, x + 70, y + 12, x + w - 20, y + 12);

            y += 25;
            g.DrawString("Ставка НДС _______ %  Сумма НДС ___________ руб. ___ коп.", fSmall, Brushes.Black, x, y);

            y += 30;
            // 1. Подготовка данных для двухстрочного вывода
            string wordsOnly = sumWords.Replace(" руб. 00 коп.", "").Trim();
            string rublesDigits = ((int)_orderAmount).ToString();
            string kopeksDigits = ((int)((_orderAmount % 1) * 100)).ToString("00");

            // 2. ВЕРХНЯЯ СТРОКА (Пропись)
            g.DrawString("Сумма с НДС", fReg, Brushes.Black, x, y);
            g.DrawString(wordsOnly, fReg, Brushes.Black, x + 100, y);
            g.DrawLine(pThin, x + 85, y + 12, x + w - 20, y + 12);
            g.DrawString("(прописью)", fSmall, Brushes.Black, x + 150, y + 14);

            y += 30;
            // 3. НИЖНЯЯ СТРОКА (Цифры и "руб.")
            g.DrawString(rublesDigits, fReg, Brushes.Black, x + 40, y);
            g.DrawLine(pThin, x + 10, y + 12, x + 100, y + 12);
            g.DrawString("руб.", fReg, Brushes.Black, x + 110, y);

            g.DrawString(kopeksDigits, fReg, Brushes.Black, x + 180, y);
            g.DrawLine(pThin, x + 150, y + 12, x + 230, y + 12);
            g.DrawString("коп.", fReg, Brushes.Black, x + 240, y);
            g.DrawString("(цифрами)", fSmall, Brushes.Black, x + 175, y + 14);

            y += 35;
            g.DrawString("Приложение:", fReg, Brushes.Black, x, y);
            g.DrawLine(pThin, x + 75, y + 12, x + w - 20, y + 12);

            y += 40;
            g.DrawString("Настоятель храма _________________  " + _rectorName, fReg, Brushes.Black, x, y);
            y += 30;
            g.DrawString("Получил казначей прихода _________________  " + _treasurerName, fReg, Brushes.Black, x, y);

            // --- ЛИНИЯ ОТРЕЗА ---
            g.DrawLine(pDash, x + w + (gap / 2), 20, x + w + (gap / 2), y + 30);

            // --- ПРАВАЯ ЧАСТЬ: КВИТАНЦИЯ ---
            int ry = 25;
            g.DrawString(_orgName, fSmall, Brushes.Black, rx, ry);
            g.DrawLine(pThin, rx, ry + 12, rx + rw, ry + 12);

            ry += 20;
            g.DrawString("КВИТАНЦИЯ", fBold, Brushes.Black, rx, ry);
            g.DrawString("к приходному кассовому", fSmall, Brushes.Black, rx, ry + 15);
            g.DrawString("ордеру", fSmall, Brushes.Black, rx, ry + 27);
            g.DrawString("№", fReg, Brushes.Black, rx + 65, ry + 22);
            g.DrawString(_orderNumber, fBold, Brushes.Black, rx + 85, ry + 22);
            g.DrawLine(pThin, rx + 80, ry + 35, rx + 160, ry + 35);

            ry += 60;
            g.DrawString(_dateDay, fReg, Brushes.Black, rx + 5, ry);
            g.DrawLine(pThin, rx, ry + 12, rx + 30, ry + 12);
            g.DrawString(_dateMonth, fReg, Brushes.Black, rx + 55, ry);
            g.DrawLine(pThin, rx + 35, ry + 12, rx + 140, ry + 12);
            g.DrawString(_dateYear.Substring(2), fReg, Brushes.Black, rx + 145, ry);
            g.DrawLine(pThin, rx + 142, ry + 12, rx + 165, ry + 12);
            g.DrawString("г.", fReg, Brushes.Black, rx + 170, ry);

            ry += 40;
            // Принято от (Квитанция) — Две строки, обычный шрифт
            g.DrawString("Принято от:", fReg, Brushes.Black, rx, ry);

            // Первая строка (Фамилия) - используем rx и ry
            g.DrawString(_personLastName, fReg, Brushes.Black, rx + 80, ry - 2);
            g.DrawLine(pThin, rx + 75, ry + 12, rx + rw, ry + 12);

            // Вторая строка (Имя Отчество) — смещаем ry еще на 18-20 пикселей вниз
            if (!string.IsNullOrEmpty(_personFirstMiddle))
            {
                ry += 20; // Увеличиваем ry для второй строки
                g.DrawString(_personFirstMiddle, fReg, Brushes.Black, rx + 10, ry - 2); // С небольшим отступом слева
                g.DrawLine(pThin, rx, ry + 12, rx + rw, ry + 12);
            }

            ry += 35;
            g.DrawString("Основание", fSmall, Brushes.Black, rx, ry);
            g.DrawLine(pThin, rx + 65, ry + 12, rx + rw, ry + 12);
            g.DrawString(_orderBase, fSmall, Brushes.Black, new RectangleF(rx + 65, ry, rw - 65, 35));

            ry += 35;
            g.DrawString("Ставка НДС", fSmall, Brushes.Black, rx, ry);
            g.DrawLine(pThin, rx + 65, ry + 12, rx + 100, ry + 12);
            g.DrawString("-", fReg, Brushes.Black, rx + 75, ry);
            g.DrawString("%", fSmall, Brushes.Black, rx + 105, ry);

            ry += 20;
            g.DrawString("Сумма НДС", fSmall, Brushes.Black, rx, ry);
            g.DrawLine(pThin, rx + 65, ry + 12, rx + 140, ry + 12);
            g.DrawString("-", fReg, Brushes.Black, rx + 100, ry);
            g.DrawString("руб.", fSmall, Brushes.Black, rx + 145, ry);
            g.DrawLine(pThin, rx + 175, ry + 12, rx + 205, ry + 12);
            g.DrawString("-", fReg, Brushes.Black, rx + 182, ry);
            g.DrawString("коп.", fSmall, Brushes.Black, rx + 210, ry);

            ry += 35;
            g.DrawString("Сумма с НДС", fSmall, Brushes.Black, rx, ry);
            g.DrawLine(pThin, rx + 75, ry + 12, rx + rw, ry + 12);

            string sWordsShort = sumWords;
            if (!string.IsNullOrEmpty(sumWords) && sumWords.Contains(" руб. 00 коп."))
            {
                sWordsShort = sumWords.Replace(" руб. 00 коп.", "");
            }
            g.DrawString(sWordsShort, fSmall, Brushes.Black, rx + 80, ry);
            g.DrawString("(прописью)", fSmall, Brushes.Black, rx + 120, ry + 13);

            ry += 20;
            g.DrawLine(pThin, rx, ry + 15, rx + 140, ry + 15);
            g.DrawString(((int)_orderAmount).ToString(), fReg, Brushes.Black, rx + 80, ry + 2);
            g.DrawString("руб.", fSmall, Brushes.Black, rx + 145, ry + 8);
            g.DrawLine(pThin, rx + 175, ry + 15, rx + 205, ry + 15);
            g.DrawString(((_orderAmount % 1) * 100).ToString("00"), fReg, Brushes.Black, rx + 180, ry + 2);
            g.DrawString("коп.", fSmall, Brushes.Black, rx + 210, ry + 8);

            ry += 40;
            g.DrawString("Настоятель храма", fSmall, Brushes.Black, rx, ry);
            g.DrawLine(pThin, rx + 95, ry + 12, rx + 140, ry + 12);
            g.DrawString(_rectorName, fReg, Brushes.Black, rx + 145, ry);

            ry += 30;
            g.DrawString("Получил кассир", fSmall, Brushes.Black, rx, ry);
            g.DrawLine(pThin, rx + 95, ry + 12, rx + 140, ry + 12);
            g.DrawString(_treasurerName, fReg, Brushes.Black, rx + 145, ry);
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

        private void SetupGrid()
        {
            dgvData.AutoGenerateColumns = false; // Отключаем автосоздание, настроим сами
            dgvData.Columns.Clear();

            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "order_number", HeaderText = "№ ПКО", Width = 70 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "date", HeaderText = "Дата", Width = 90 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "person", HeaderText = "От кого", Width = 180 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "amount", HeaderText = "Сумма", Width = 100 });
            dgvData.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "base", HeaderText = "Основание", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            // Форматируем сумму, чтобы было "100.00"
            dgvData.Columns[3].DefaultCellStyle.Format = "N2";
            dgvData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvData.ReadOnly = true; // Запрещаем случайное редактирование в сетке
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Выделение всей строки
        }

        private void RefreshGridData()
        {
            // Здесь _service виден, так как он объявлен в начале класса формы
            DataTable dt = _service.GetAllCashOrders();

            // Здесь dgvData виден, так как это контрол на форме
            if (dt != null)
            {
                dgvData.DataSource = dt;
            }
        }

        private void SetupPkoDataGrid(int pkoId)
        {
            DataTable dt = _service.GetPkoRegistryRow(pkoId);
            dgvData.DataSource = dt;

            // 1. Устанавливаем многострочные заголовки
            dgvData.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvData.ColumnHeadersHeight = 60; // Увеличиваем высоту шапки

            // 2. Переименовываем колонки в длинные названия из вашего ТЗ
            dgvData.Columns["1"].HeaderText = "Фамилия физического лица...";
            dgvData.Columns["1а"].HeaderText = "Собственное имя и отчество...";
            dgvData.Columns["2"].HeaderText = "Код валюты";
            dgvData.Columns["2а"].HeaderText = "Наименование валюты";
            dgvData.Columns["3"].HeaderText = "Частоприменяемые формулировки основания";

            // 3. Включаем перенос текста в ячейках (для столбца 3)
            dgvData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // 4. Настраиваем ширину
            dgvData.Columns["3"].Width = 250;
        }

        private void LoadPkoTableData(int pkoId)
        {
            try
            {
                DataTable dt = _service.GetPkoReportData(pkoId);
                dgvData.DataSource = dt;

                // Настройка заголовков как на картинке
                dgvData.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvData.ColumnHeadersHeight = 80;

                dgvData.Columns["1"].HeaderText = "Фамилия физического лица, от которого приняты...";
                dgvData.Columns["1а"].HeaderText = "Собственное имя и отчество (если таковое имеется)";
                dgvData.Columns["2"].HeaderText = "Код валюты";
                dgvData.Columns["2а"].HeaderText = "Наименование валюты";
                dgvData.Columns["3"].HeaderText = "Частоприменяемые формулировки основания";

                // Чтобы в столбце 3 работал перенос строк (char(10)):
                dgvData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // Визуальная настройка ширины
                dgvData.Columns["3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при заполнении таблицы: " + ex.Message);
            }
        }

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
                if (tabPKO.SelectedTab.Name == "tabPrintForm")
                {
                    pd.DefaultPageSettings.Landscape = false;
                    pd.PrintPage += new PrintPageEventHandler(PrintOrderPage);
                }
                else if (tabPKO.SelectedTab.Name == "tabData")
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

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}
