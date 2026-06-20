using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class CashbookForm : Form
    {
        private CashbookService _service;
        private int _currentYear;
        private int _currentMonth;
        private int _currentPage;

        public CashbookForm()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                ImageHelper.ApplyToButtons(this, 24);

                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "church.db");
                string connectionString = $"Data Source={dbPath};Version=3;";
                _service = new CashbookService(connectionString);

                SetupControls();
            }
        }

        private void SetupControls()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 3; year <= currentYear + 1; year++)
                cmbYear.Items.Add(year);
            cmbYear.SelectedItem = currentYear;

            string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                               "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            cmbMonth.Items.AddRange(months);
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;

            btnView.Click += BtnView_Click;
            btnPrint.Click += BtnPrint_Click;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            _currentYear = Convert.ToInt32(cmbYear.SelectedItem);
            _currentMonth = cmbMonth.SelectedIndex + 1;
            _currentPage = 0;
            UpdatePageDisplay();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            NavigatePage(-1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            NavigatePage(1);
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            if (_currentPage == 0)
            {
                btnGenerate_Click(sender, e);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (webBrowser.Document != null)
            {
                try
                {
                    // Отключаем колонтитулы через реестр
                    DisableBrowserHeadersFooters();

                    // Печатаем только содержимое WebBrowser
                    webBrowser.ShowPrintDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка печати: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Восстанавливаем колонтитулы (опционально)
                    // RestoreBrowserHeadersFooters();
                }
            }
            else
            {
                MessageBox.Show("Сначала сформируйте кассовую книгу", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DisableBrowserHeadersFooters()
        {
            try
            {
                // Отключаем колонтитулы для IE/Edge WebView
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Internet Explorer\PageSetup", true);

                if (key != null)
                {
                    key.SetValue("header", "");
                    key.SetValue("footer", "");
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Не удалось отключить колонтитулы: {ex.Message}");
            }
        }

        private void RestoreBrowserHeadersFooters()
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Internet Explorer\PageSetup", true);

                if (key != null)
                {
                    key.SetValue("header", "&w&bСтраница &p из &P");
                    key.SetValue("footer", "&u&b&d");
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Не удалось восстановить колонтитулы: {ex.Message}");
            }
        }

        private void NavigatePage(int direction)
        {
            int daysInMonth = DateTime.DaysInMonth(_currentYear, _currentMonth);
            int totalPages = daysInMonth + 2;

            _currentPage += direction;
            if (_currentPage < 0) _currentPage = 0;
            if (_currentPage >= totalPages) _currentPage = totalPages - 1;

            UpdatePageDisplay();
        }

        private void UpdatePageDisplay()
        {
            string html = "";
            int daysInMonth = DateTime.DaysInMonth(_currentYear, _currentMonth);
            int totalPages = daysInMonth + 2;

            if (_currentPage == 0)
            {
                html = GenerateTitlePage(_currentYear);
                // Скрываем маркеры на титульном листе
                pnlMarkers.Visible = false;
            }
            else if (_currentPage <= daysInMonth)
            {
                html = GenerateDayPage(_currentYear, _currentMonth, _currentPage, daysInMonth);
                // Показываем маркеры и обновляем их
                pnlMarkers.Visible = true;

                // Обновляем значение в розовом квадрате
                int pagesBefore = CalculatePagesBefore(_currentYear, _currentMonth);
                lblPagesBefore.Text = pagesBefore.ToString();

                // Автоматически устанавливаем чекбоксы
                bool isLastInMonth = (_currentPage == daysInMonth);
                bool isLastInYear = (_currentMonth == 12 && _currentPage == daysInMonth);

                chkLastInMonth.Checked = isLastInMonth;
                chkLastInYear.Checked = isLastInYear;
            }
            else
            {
                html = GenerateLastPage(_currentYear, _currentMonth, daysInMonth);
                // Скрываем маркеры на последнем листе
                pnlMarkers.Visible = false;
            }

            webBrowser.DocumentText = html;
            lblPage.Text = $"Страница {_currentPage + 1} из {totalPages}";
        }

        private string GenerateTitlePage(int year)
        {
            var orgInfo = _service.GetOrganizationInfo();
            string orgName = orgInfo?["name"]?.ToString() ?? "________________";
            string location = orgInfo?["location"]?.ToString() ?? "";
            string fullOrgName = $"{orgName} {location}".Trim();

            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: 'Times New Roman', Times, serif; font-size: 12pt; margin: 0; padding: 0; }}
        .title-box {{ 
            border: 2px solid black; 
            width: 600px; 
            margin: 50px auto; 
            padding: 40px 20px;
            text-align: center;
        }}
        .org-name {{ 
            font-size: 11pt;
            margin-bottom: 10px;
            font-weight: bold;
        }}
        .org-line {{ 
            border-bottom: 1px solid black; 
            margin-bottom: 5px;
            padding-bottom: 5px;
        }}
        .org-subtitle {{ 
            font-size: 10pt;
            margin-bottom: 30px;
            font-style: italic;
        }}
        .main-title {{ 
            font-weight: bold; 
            font-size: 14pt; 
            margin: 20px 0;
        }}
        .year {{ 
            margin-top: 20px;
            font-size: 12pt;
        }}
        @media print {{
        @page {{
            margin: 10mm;
            /* Отключаем колонтитулы */
            margin-top: 0;
            margin-bottom: 0;
        }}
    
        /* Скрываем все лишнее */
        body {{
            -webkit-print-color-adjust: exact;
            print-color-adjust: exact;
        }}
    }}
        @media print {{ body {{ margin: 0; }} }}
    </style>
</head>
<body>
    <div class='title-box'>
        <div class='org-name'>{fullOrgName}</div>
        <div class='org-line'></div>
        <div class='org-subtitle'>(полное наименование юридического лица)</div>
        <div class='main-title'>КАССОВАЯ КНИГА</div>
        <div class='year'>на {year} год</div>
    </div>
</body>
</html>";
        }

        private string GenerateDayPage(int year, int month, int day, int totalDays)
        {
            var operations = _service.GetOperationsForDay(year, month, day);
            decimal openingBalance = day == 1
                ? _service.GetOpeningBalanceForYear(year)
                : _service.CalculateClosingBalance(year, month, day - 1);

            decimal totalIncome = 0;
            decimal totalExpense = 0;

            StringBuilder rowsHtml = new StringBuilder();
            int rowNum = 1;

            // Операции за день (БЕЗ первой строки с остатком!)
            foreach (DataRow row in operations.Rows)
            {
                string orderType = row["order_type"].ToString();
                decimal amount = Convert.ToDecimal(row["amount"]);

                if (orderType == "ПКО")
                    totalIncome += amount;
                else
                    totalExpense += amount;

                string counterparty = row["counterparty"].ToString();
                string orderNumber = row["order_number"].ToString();

                rowsHtml.AppendLine($@"<tr>
            <td>{rowNum++}</td>
            <td>{orderNumber}</td>
            <td>{counterparty}</td>
            <td></td>
            <td style='text-align: right;'>{(orderType == "ПКО" ? amount.ToString("N2").Replace(',', '.') : "")}</td>
            <td style='text-align: right;'>{(orderType == "РКО" ? amount.ToString("N2").Replace(',', '.') : "")}</td>
        </tr>");
            }

            // Пустые строки (всего 15 строк)
            int emptyRows = 15 - operations.Rows.Count;
            for (int i = 0; i < emptyRows; i++)
            {
                rowsHtml.AppendLine(@"<tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td style='text-align: center;'>-</td>
            <td style='text-align: center;'>-</td>
        </tr>");
            }

            decimal closingBalance = openingBalance + totalIncome - totalExpense;

            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ 
            font-family: 'Times New Roman', Times, serif; 
            font-size: 9pt; 
            margin: 0; 
            padding: 8mm;
            width: 280mm;
        }}
        .header-top {{ 
            text-align: right;
            margin-bottom: 5px;
            font-size: 9pt;
        }}
        .header-title {{
            text-align: center;
            font-weight: bold;
            margin: 10px 0;
            font-size: 10pt;
        }}
        .date-line {{ 
            display: flex; 
            justify-content: space-between;
            margin-bottom: 5px; 
            font-size: 9pt;
        }}
        table {{ 
            border-collapse: collapse; 
            width: 100%; 
            font-size: 8pt;
            margin-top: 5px;
        }}
        th, td {{ 
            border: 1px solid black; 
            padding: 3px 4px; 
            vertical-align: middle;
        }}
        th {{ 
            text-align: center; 
            font-weight: bold;
            background-color: #f0f0f0;
        }}
        .totals-row td {{
            background-color: #e0ffff;
        }}
        .right {{ 
            text-align: right; 
        }}
        .center {{
            text-align: center;
        }}
        .bottom-info {{
            margin-top: 10px;
            font-size: 9pt;
            line-height: 1.8;
        }}
        .signature-line {{
            display: inline-block;
            border-bottom: 1px solid black;
            width: 120px;
            margin: 0 5px;
        }}
        .signature-line-long {{
            display: inline-block;
            border-bottom: 1px solid black;
            width: 250px;
            margin: 0 5px;
        }}
        .signature-label {{
            font-size: 8pt;
            font-style: italic;
        }}
        .check-line {{
            background-color: #e0ffff;
            margin: 5px 0;
            padding: 3px 5px;
        }}
        @media print {{
            body {{ margin: 6mm; width: 280mm; }}
        }}
    </style>
</head>
<body>
    <div class='header-top'>
        <div>Вкладной лист кассовой книги</div>
        <div>Лист <u>&nbsp;&nbsp;&nbsp;&nbsp;{day}&nbsp;&nbsp;&nbsp;&nbsp;</u></div>
    </div>
    
    <div class='date-line'>
        <span>Касса за {day} {GetMonthName(month)} {year} г.</span>
        <span>Лист {day} из {totalDays}</span>
    </div>
    
    <table>
        <thead>
            <tr>
                <th rowspan='2' style='width: 35px;'>№<br>п/п</th>
                <th rowspan='2' style='width: 100px;'>Номер<br>документа</th>
                <th rowspan='2'>От кого получено или кому выдано</th>
                <th rowspan='2' style='width: 90px;'>Номер корреспонди-<br>рующего счета,<br>субсчета</th>
                <th rowspan='2' style='width: 90px;'>Приход,<br>рублей</th>
                <th rowspan='2' style='width: 90px;'>Расход,<br>рублей</th>
            </tr>
        </thead>
        <tbody>
            {rowsHtml}
            <tr class='totals-row'>
                <td colspan='4' style='text-align: right;'>Итого по листу:</td>
                <td style='text-align: right;'>-</td>
                <td style='text-align: right;'>-</td>
            </tr>
            <tr class='totals-row'>
                <td colspan='4' style='text-align: right;'>Итого за день:</td>
                <td style='text-align: right;'>{totalIncome:N2}</td>
                <td style='text-align: right;'>{totalExpense:N2}</td>
            </tr>
            <tr class='totals-row'>
                <td colspan='4' style='text-align: right;'>Остаток на начало дня:</td>
                <td colspan='2' style='text-align: right;'>{openingBalance:N2}</td>
            </tr>
            <tr class='totals-row'>
                <td colspan='4' style='text-align: right;'>Остаток на конец дня:</td>
                <td colspan='2' style='text-align: right;'>{closingBalance:N2}</td>
            </tr>
        </tbody>
    </table>

    <div class='bottom-info'>
        <div style='margin-top: 10px;'>
            <span>Кассир</span>
            <span class='signature-line'></span>
            <span style='float: right;' class='signature-label'>(инициалы, фамилия)</span>
        </div>
        <div style='text-align: center; margin: 5px 0;'>
            <span class='signature-label'>(подпись)</span>
        </div>
        
        <div style='margin-top: 10px; clear: both;'>
            Записи в кассовой книге проверил и документы в количестве
        </div>
        
        <div class='check-line'>
            <span style='float: right;' class='signature-label'>(прописью)</span>
            <span style='float: right; margin-right: 50px;'>-</span>
        </div>
        
        <div style='margin-top: 10px; clear: both;'>
            <span>приходных и</span>
            <span class='signature-line-long'></span>
            <span>расходных получил.</span>
        </div>
        
        <div style='text-align: center; margin: 5px 0;'>
            <span class='signature-label'>(прописью)</span>
        </div>
        
        <div style='margin-top: 10px; clear: both;'>
            <span>Бухгалтер</span>
            <span class='signature-line'></span>
            <span style='float: right;' class='signature-label'>(инициалы, фамилия)</span>
        </div>
        <div style='text-align: center; margin: 5px 0;'>
            <span class='signature-label'>(подпись)</span>
        </div>
    </div>
</body>
</html>";
        }

        private string GetMonthName(int month)
        {
            string[] months = { "", "января", "февраля", "марта", "апреля", "мая", "июня",
                       "июля", "августа", "сентября", "октября", "ноября", "декабря" };
            return months[month];
        }

        private int GetSequentialNumber(int year, int month, int day, int localNumber)
        {
            // Считаем количество операций с начала года до текущего дня
            int sequentialStart = 1;

            // Суммируем все операции за предыдущие месяцы
            for (int m = 1; m < month; m++)
            {
                int daysInPrevMonth = DateTime.DaysInMonth(year, m);
                for (int d = 1; d <= daysInPrevMonth; d++)
                {
                    var ops = _service.GetOperationsForDay(year, m, d);
                    sequentialStart += ops.Rows.Count;
                }
            }

            // Суммируем операции за текущий месяц до текущего дня
            for (int d = 1; d < day; d++)
            {
                var ops = _service.GetOperationsForDay(year, month, d);
                sequentialStart += ops.Rows.Count;
            }

            // Возвращаем: начало + локальный номер
            return sequentialStart + localNumber - 1;
        }

        private string GenerateLastPage(int year, int month, int totalDays)
        {
            var (cashier, accountant) = _service.GetSignatures();
            decimal yearEndBalance = _service.CalculateClosingBalance(year, 12, 31);
            int totalPages = totalDays + 2;

            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ 
            font-family: 'Times New Roman', Times, serif; 
            font-size: 10pt; 
            margin: 0; 
            padding: 20mm;
            min-height: 257mm;
        }}
        .signature-box {{
            border: 1px solid black;
            padding: 25px 30px;
            width: 400px;
            margin-left: auto;
            margin-right: 0;
            margin-top: 60mm;
            float: right;
        }}
        .signature-box p {{
            margin: 12px 0;
            line-height: 1.6;
            text-align: center;
        }}
        .signature-block {{
            margin: 20px 0;
            text-align: left;
        }}
        .signature-title {{
            margin-bottom: 5px;
        }}
        .signature-title-line {{
            display: inline-block;
            border-bottom: 1px solid black;
            width: 180px;
            margin-left: 10px;
        }}
        .signature-sub {{
            margin-top: 3px;
            font-size: 8pt;
            font-style: italic;
            text-align: right;
            padding-right: 105px; /* ← Сдвигает текст влево от правого края */
        }}
        .stamp {{
            font-weight: bold;
            text-align: center;
            margin: 25px 0;
            font-size: 11pt;
        }}
        .date-line {{
            margin-top: 25px;
            text-align: left;
        }}
        .date-line span {{
            display: inline-block;
            border-bottom: 1px solid black;
            width: 45px;
            margin: 0 3px;
        }}
        .clearfix::after {{
            content: "";
            display: table;
            clear: both;
        }}
        
        @media print {{
            @page {{ margin: 0; }}
            body {{ margin: 0; padding: 15mm; min-height: 267mm; }}
        }}
    </style>
</head>
<body>
    <div class='clearfix'>
        <div class='signature-box'>
            <p>В настоящей книге пронумеровано,<br>прошнуровано и скреплено печатью<br>
            _______________ листов</p>
            
            <div class='signature-block'>
                <div class='signature-title'>Руководитель <span class='signature-title-line'></span></div>
                <div class='signature-sub'>(подпись) (инициалы, фамилия)</div>
            </div>
            
            <p class='stamp'>М.П.</p>
            
            <div class='signature-block'>
                <div class='signature-title'>Главный бухгалтер <span class='signature-title-line'></span></div>
                <div class='signature-sub'>(подпись) (инициалы, фамилия)</div>
            </div>
            
            <div class='date-line'>
                <span></span> <span></span> <span></span> г.
            </div>
        </div>
    </div>
</body>
</html>";
        }

        private int CalculatePagesBefore(int year, int month)
        {
            int pagesBefore = 0;
            for (int m = 1; m < month; m++)
            {
                pagesBefore += DateTime.DaysInMonth(year, m);
            }
            return pagesBefore;
        }

        private string NumberToWords(int number)
        {
            string[] ones = { "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
            string[] teens = { "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать",
                              "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
            string[] tens = { "", "", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят",
                             "семьдесят", "восемьдесят", "девяносто" };

            if (number == 0) return "ноль";
            if (number < 10) return ones[number];
            if (number < 20) return teens[number - 10];
            if (number < 100) return tens[number / 10] + (number % 10 > 0 ? " " + ones[number % 10] : "");

            return number.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}