using ChurchBudget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class FinanceReportForm : Form
    {
        private FinancialReportService _reportService;
        private int _currentOrgId;
        private string _currentHtml = string.Empty;
        private DateTime _reportDate;  // ✅ Новое поле для даты отчёта

        public FinanceReportForm(int orgId, string connectionString)
        {
            InitializeComponent();
            _currentOrgId = orgId;
            _reportService = new FinancialReportService(connectionString);

            if (cmbMonth.Items.Count == 0)
            {
                string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                               "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
                cmbMonth.Items.AddRange(months);
                cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
            }
            numYear.Value = DateTime.Now.Year;
        }

        public FinanceReportForm()
        {
            InitializeComponent();
        }

        private void FinanceReportForm_Load(object sender, EventArgs e)
        {
            ImageHelper.ApplyToButtons(this, 24);
            ImageHelper.ApplyToDataGridViews(this);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate, endDate;

                if (rbMonth.Checked)
                {
                    int month = cmbMonth.SelectedIndex + 1;
                    int year = (int)numYear.Value;
                    startDate = new DateTime(year, month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    int year = (int)numYear.Value;
                    startDate = new DateTime(year, 1, 1);
                    endDate = new DateTime(year, 12, 31);
                }

                _reportDate = startDate;  // ✅ Сохраняем дату отчёта

                decimal openingBalance = _reportService.GetOpeningBalanceForPeriod(_currentOrgId, startDate);
                var incomeData = _reportService.GetIncomeByReportCode(startDate, endDate);
                var expenseData = _reportService.GetExpenseByReportCode(startDate, endDate);
                var orgInfo = _reportService.GetOrganizationInfo(_currentOrgId);
                var signatures = _reportService.GetSignatures(_currentOrgId);

                decimal totalIncome = 0;
                foreach (var amount in incomeData.Values) totalIncome += amount;

                decimal totalExpense = 0;
                foreach (var amount in expenseData.Values) totalExpense += amount;

                decimal closingBalance = openingBalance + totalIncome - totalExpense;

                // ✅ ИСПРАВЛЕНО: убрано "за" из periodText
                string periodText = rbMonth.Checked
                    ? string.Format("{0:MMMM yyyy}", startDate)
                    : string.Format("{0} год", startDate.Year);

                string html = GenerateReportHtml(
                    orgInfo, periodText,
                    openingBalance, incomeData, totalIncome,
                    expenseData, totalExpense, closingBalance,
                    signatures);

                webBrowserReport.DocumentText = html;
                _currentHtml = html;  // ✅ Сохраняем HTML для экспорта в PDF
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ошибка формирования отчёта:\n{0}", ex.Message),
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (webBrowserReport.Document != null)
                webBrowserReport.Print();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentHtml))
            {
                MessageBox.Show("Сначала сформируйте отчёт, нажав кнопку «Просмотреть».",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Формируем имя файла на основе периода отчёта
            string fileName;
            if (rbMonth.Checked)
            {
                // Месячный отчёт: FinReport_2026-06.pdf
                fileName = string.Format("FinReport_{0:yyyy-MM}.pdf", _reportDate);
            }
            else
            {
                // Годовой отчёт: FinReport_2026.pdf
                fileName = string.Format("FinReport_{0:yyyy}.pdf", _reportDate);
            }

            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "PDF-документы|*.pdf|HTML-файлы|*.html|Все файлы|*.*",
                FileName = fileName,
                FilterIndex = 1 // По умолчанию выбран PDF
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                string ext = Path.GetExtension(dlg.FileName).ToLower();

                if (ext == ".pdf")
                {
                    SaveAsPdf(dlg.FileName);
                }
                else if (ext == ".html")
                {
                    File.WriteAllText(dlg.FileName, _currentHtml, Encoding.UTF8);
                }
                else
                {
                    // Если расширение не указано — сохраняем как PDF
                    if (!dlg.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        dlg.FileName += ".pdf";
                    SaveAsPdf(dlg.FileName);
                }

                MessageBox.Show("Отчёт успешно сохранён!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAsPdf(string filePath)
        {
            string exeName = Environment.Is64BitOperatingSystem ? "wkhtmltopdf_x64.exe" : "wkhtmltopdf-x86.exe";

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string wkhtmltopdfPath = Path.Combine(baseDir, exeName);

            if (!File.Exists(wkhtmltopdfPath))
            {
                throw new FileNotFoundException(
                    "Не найден файл " + exeName + "\n\n" +
                    "Ожидаемый путь: " + wkhtmltopdfPath);
            }

            string tempHtmlPath = Path.Combine(Path.GetTempPath(), "temp_report.html");
            File.WriteAllText(tempHtmlPath, _currentHtml, Encoding.UTF8);

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = wkhtmltopdfPath,
                    Arguments = string.Format(
                        "--page-size A4 " +
                        "--orientation Portrait " +  // ✅ Добавлено: принудительная книжная ориентация
                        "--margin-top 10mm " +
                        "--margin-bottom 10mm " +
                        "--margin-left 10mm " +
                        "--margin-right 10mm " +
                        "--encoding utf-8 " +
                        "--enable-local-file-access " +
                        "\"{0}\" \"{1}\"",
                        tempHtmlPath, filePath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    if (!process.WaitForExit(30000))
                    {
                        process.Kill();
                        throw new TimeoutException("Превышено время ожидания конвертации в PDF");
                    }

                    if (process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception("Ошибка wkhtmltopdf: " + error);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempHtmlPath))
                {
                    try { File.Delete(tempHtmlPath); } catch { }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }

        private string GenerateReportHtml(
        Dictionary<string, string> orgInfo, string periodText,
        decimal openingBalance, Dictionary<string, decimal> incomeData, decimal totalIncome,
        Dictionary<string, decimal> expenseData, decimal totalExpense, decimal closingBalance,
        Dictionary<string, string> signatures)
            {
                StringBuilder html = new StringBuilder();

                string patriarchate = _reportService.GetConstant("Patriarchate");
                string reportTitle = _reportService.GetConstant("ReportTitle");

                if (string.IsNullOrEmpty(patriarchate))
                    patriarchate = "Белорусской Православной Церкви Московского Патриархата";
                if (string.IsNullOrEmpty(reportTitle))
                    reportTitle = "Финансовый отчет о движении денежных средств";

                html.AppendLine(@"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                         @page { 
                            size: A4 portrait; 
                            margin: 10mm; 
                        }
                        body { 
                            font-family: 'Times New Roman', Times, serif; 
                            font-size: 11pt; 
                            margin: 10mm;
                            line-height: 1.3;
                            width: 190mm;
                            max-width: 190mm;
                        }
                        .header { 
                            text-align: center; 
                            font-weight: bold; 
                            margin-bottom: 5px;
                        }
                        .subheader { 
                            text-align: center; 
                            font-weight: bold; 
                            margin: 10px 0;
                            font-size: 16pt;
                        }
                        table { 
                            border-collapse: collapse; 
                            width: 190mm;
                            margin: 15px 0;
                        }
                        td, th { 
                            border: 1px solid #000; 
                            padding: 4px 6px; 
                            vertical-align: top;
                        }
                        .expense-group { 
                            font-weight: bold; 
                        }
                        .expense-sub { 
                            font-weight: normal; 
                            padding-left: 20px;
                        }
                        .empty-row td { 
                            border: none !important; 
                            height: 8px; 
                            padding: 0 !important;
                        }
                        .num { text-align: right; width: 120px; }
                        .note { width: 150px; }
                        .indent { padding-left: 20px; }
                        .bold { font-weight: bold; }
                    
                        /* ✅ НОВЫЕ СТИЛИ ДЛЯ ТАБЛИЦЫ ПОДПИСЕЙ */
                        .signature-table {
                            width: 190mm;
                            border: none;
                            border-collapse: collapse;
                            margin-top: 20px; 
                        }
                        .signature-table td {
                            border: none;
                            padding: 5px 0;
                            vertical-align: bottom; /* Текст и линия выравниваются по нижнему краю */
                        }
                        .sig-position { width: 45%; text-align: left; }
                        .sig-gap { width: 3%; }
                        .sig-line { 
                            width: 34%; 
                            border-bottom: 1px solid #000; /* Ровная черта вместо символов ___ */
                        }
                        .sig-name { 
                            width: 18%; /* Подогнано под ширину столбца Сумма (120px) */
                            text-align: center; 
                        }

                        /* ✅ Добавьте эти стили для принудительной портретной ориентации */
                        @media print {
                            @page { 
                                size: A4 portrait !important; 
                                margin: 10mm !important;
                            }
                            body { 
                                margin: 10mm; 
                                width: 190mm;
                                max-width: 190mm;
                            }
                        }
                    </style>
                </head>
                <body>");

                // === Шапка ===
                string diocese = orgInfo.ContainsKey("Diocese") ? orgInfo["Diocese"] : "Борисовская епархия";
                string fullHeader = string.Format("{0} {1}", diocese, patriarchate);
                html.AppendLine(string.Format("<div class='header'>{0}</div>", fullHeader));

                html.AppendLine(string.Format("<div class='subheader'>{0}</div>", reportTitle));

                html.AppendLine(string.Format("<div style='text-align: center; margin: 20px 0 5px 0;'>за <span style='border-bottom: 1px solid #000; display: inline-block; min-width: 250px;'>{0}</span></div>", periodText));

                string orgFullInfo = string.Format("{0}, {1}, {2}",
                    orgInfo.ContainsKey("Name") ? orgInfo["Name"] : "",
                    orgInfo.ContainsKey("Location") ? orgInfo["Location"] : "",
                    orgInfo.ContainsKey("Blagochinie") ? orgInfo["Blagochinie"] : "");

                html.AppendLine(string.Format("<div style='text-align: center; margin: 5px 0;'>В <span style='border-bottom: 1px solid #000; display: inline-block; min-width: 500px;'>{0}</span></div>", orgFullInfo));
                html.AppendLine("<div style='text-align: center; font-size: 9pt; margin-bottom: 25px;'>(наименование прихода, место расположения, благочиние)</div>");

                // === Таблица ===
                html.AppendLine("<table>");
                html.AppendLine("<tr><th style='width:30px'>№</th><th>Содержание</th><th class='num'>Сумма (руб.)</th><th class='note'>Примечание</th></tr>");

                // Строка 1
                html.AppendLine(string.Format("<tr><td>1</td><td class='bold'>Остаток средств на начало периода</td><td class='num bold'>{0:F2}</td><td></td></tr>", openingBalance));

                html.AppendLine(string.Format("<tr><td>2</td><td class='bold'>Доходы (поступления)</td><td class='num bold'>{0:F2}</td><td></td></tr>", totalIncome));
                html.AppendLine("<tr><td></td><td class='indent'>в т.ч. (статьи доходов):</td><td class='num'></td><td></td></tr>");

                AddIncomeRow(html, incomeData, "INC_BOX", "Скарбонка");
                AddIncomeRow(html, incomeData, "INC_BAPTISM", "Крещение");
                AddIncomeRow(html, incomeData, "INC_FUNER", "Отпевание");
                AddIncomeRow(html, incomeData, "INC_WED", "Венчание");
                AddIncomeRow(html, incomeData, "INC_CANDLES", "Свечи");
                AddIncomeRow(html, incomeData, "INC_UTEN", "Церковная утварь");
                AddIncomeRow(html, incomeData, "INC_BOOKS", "Литература");
                AddIncomeRow(html, incomeData, "INC_GOLD", "Изделия из драгоценных металлов религиозного назначения");
                AddIncomeRow(html, incomeData, "INC_NOTES", "Записки");
                AddIncomeRow(html, incomeData, "INC_CHARHELP", "Благотворительная помощь (без предоставления отчета)");
                AddIncomeRow(html, incomeData, "INC_SPONCHELP", "Спонсорская помощь (с предоставлением отчета)");
                AddIncomeRow(html, incomeData, "INC_TARGDONAT", "Целевое пожертвование на нужды храма");
                AddIncomeRow(html, incomeData, "INC_OTHER", "Прочие доходы", "указать источник");

                html.AppendLine(string.Format("<tr><td></td><td class='bold'>Итого поступило за период (по стр. 2)</td><td class='num bold'>{0:F2}</td><td></td></tr>", totalIncome));

                // === Строка 3: Расходы ===
                html.AppendLine(string.Format("<tr><td>3</td><td class='bold'>Расходы</td><td class='num bold'>{0:F2}</td><td></td></tr>", totalExpense));

                // 1. Коммунальные платежи
                decimal commTotal = GetExpenseSum(expenseData, "EXP_ELECTR", "EXP_HEAT", "EXP_WATER", "EXP_LINK", "EXP_OTHERS");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>1. Коммунальные платежи</td><td class='num bold'>{0:F2}</td><td></td></tr>", commTotal));
                AddExpenseRow(html, expenseData, "EXP_ELECTR", "Электроэнергия");
                AddExpenseRow(html, expenseData, "EXP_HEAT", "Отопление");
                AddExpenseRow(html, expenseData, "EXP_WATER", "Вода и канализация");
                AddExpenseRow(html, expenseData, "EXP_LINK", "Связь (телефон, интернет)");
                AddExpenseRow(html, expenseData, "EXP_OTHERS", "Другие расходы");

                decimal transpTotal = GetExpenseSum(expenseData, "EXP_FUEL", "EXP_SPARE", "EXP_INSURE", "EXP_RTOLL");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>2. Транспортные расходы</td><td class='num bold'>{0:F2}</td><td></td></tr>", transpTotal));
                AddExpenseRow(html, expenseData, "EXP_FUEL", "Топливо");
                AddExpenseRow(html, expenseData, "EXP_SPARE", "Запчасти");
                AddExpenseRow(html, expenseData, "EXP_INSURE", "Оплата страхового полиса");
                AddExpenseRow(html, expenseData, "EXP_RTOLL", "Дорожный сбор");

                decimal houseTotal = GetExpenseSum(expenseData, "EXP_HOUSE");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>3. Хозяйственные расходы</td><td class='num bold'>{0:F2}</td><td></td></tr>", houseTotal));

                // 4. Ремонтно-строительные работы
                decimal buildTotal = GetExpenseSum(expenseData, "EXP_BUILD", "EXP_BUILDPAY");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>4. Ремонтно-строительные работы</td><td class='num bold'>{0:F2}</td><td></td></tr>", buildTotal));
                AddExpenseRow(html, expenseData, "EXP_BUILD", "Стройматериалы");
                AddExpenseRow(html, expenseData, "EXP_BUILDPAY", "Оплата за строительные работы");

                decimal shopTotal = GetExpenseSum(expenseData, "EXP_CANDLES", "EXP_CHUTEN", "EXP_BOOKS", "EXP_GOLD", "EXP_OTHERPROD");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>5. Церковная лавка</td><td class='num bold'>{0:F2}</td><td></td></tr>", shopTotal));
                AddExpenseRow(html, expenseData, "EXP_CANDLES", "Свечи");
                AddExpenseRow(html, expenseData, "EXP_CHUTEN", "Церковная утварь");
                AddExpenseRow(html, expenseData, "EXP_BOOKS", "Литература");
                AddExpenseRow(html, expenseData, "EXP_GOLD", "Изделия из драгоценных металлов религиозного назначения");
                AddExpenseRow(html, expenseData, "EXP_OTHERPROD", "Прочие изделия");

                // 6. Расходы на оплату труда
                decimal payTotal = GetExpenseSum(expenseData, "EXP_PAYM", "EXP_TAX", "EXP_FINSUP");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>6. Расходы на оплату труда</td><td class='num bold'>{0:F2}</td><td></td></tr>", payTotal));
                AddExpenseRow(html, expenseData, "EXP_PAYM", "Зарплата");
                AddExpenseRow(html, expenseData, "EXP_TAX", "Налоги");
                AddExpenseRow(html, expenseData, "EXP_FINSUP", "Материальная поддержка");

                decimal charTotal = GetExpenseSum(expenseData, "EXP_CHAR");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>7. Благотворительная помощь</td><td class='num bold'>{0:F2}</td><td></td></tr>", charTotal));

                // 8. Взносы
                decimal contrTotal = GetExpenseSum(expenseData, "EXP_DIOCONTR", "EXP_ADDCONTR", "EXP_OTHERCONTR");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>8. Взносы</td><td class='num bold'>{0:F2}</td><td></td></tr>", contrTotal));
                AddExpenseRow(html, expenseData, "EXP_DIOCONTR", "Епархиальные взносы");
                AddExpenseRow(html, expenseData, "EXP_ADDCONTR", "Дополнительный взнос");
                AddExpenseRow(html, expenseData, "EXP_OTHERCONTR", "Прочие взносы");

                decimal holdTotal = GetExpenseSum(expenseData, "EXP_HOLD");
                html.AppendLine(string.Format("<tr><td></td><td class='expense-group'>9. Расходы на организацию и проведение праздничных приходских и прочих мероприятий (духовно-просветительские, встречи, семинары, беседы)</td><td class='num bold'>{0:F2}</td><td></td></tr>", holdTotal));

                AddExpenseRowNoIndent(html, expenseData, "EXP_LIB", "10. Расходы на содержание библиотеки");
                AddExpenseRowNoIndent(html, expenseData, "EXP_SOC", "11. Расходы на социальную работу");
                AddExpenseRowNoIndent(html, expenseData, "EXP_SUNSCHL", "12. Расходы на содержание Воскресной школы");
                AddExpenseRowNoIndent(html, expenseData, "EXP_FOOD", "13. Расходы на питание");
                AddExpenseRowNoIndent(html, expenseData, "EXP_UTEN", "14. Приобретение утвари, оборудования");
                AddExpenseRowNoIndent(html, expenseData, "EXP_BISTRIP", "15. Командировки, паломничество");
                AddExpenseRowNoIndent(html, expenseData, "EXP_TECHNOLOG", "16. Проектная документация");
                AddExpenseRowNoIndent(html, expenseData, "EXP_OTHER", "17. Прочие расходы");

                html.AppendLine(string.Format("<tr><td></td><td class='bold'>Итого расходы за период (по стр. 3)</td><td class='num bold'>{0:F2}</td><td></td></tr>", totalExpense));
                html.AppendLine("<tr class='empty-row'><td colspan='4'>&nbsp;</td></tr>");

                html.AppendLine(string.Format("<tr><td></td><td class='bold'>Остаток средств на конец периода</td><td class='num bold'>{0:F2}</td><td></td></tr>", closingBalance));
                html.AppendLine("<tr class='empty-row'><td colspan='4'>&nbsp;</td></tr>");

                html.AppendLine("</table>");

                // === ✅ ИСПРАВЛЕНО: Подписи через таблицу без рамок ===
                // === Подписи ===
                string rector = signatures.ContainsKey("Настоятель храма") ? signatures["Настоятель храма"] : "";
                string chairman = signatures.ContainsKey("Председатель приходского совета") ? signatures["Председатель приходского совета"] : "";
                string treasurer = signatures.ContainsKey("Казначей") ? signatures["Казначей"] : "";
                string auditor = signatures.ContainsKey("Член ревизионной комиссии") ? signatures["Член ревизионной комиссии"] : "";

                html.AppendLine("<table class='signature-table'>");

                // Настоятель
                html.AppendLine("<tr>");
                html.AppendLine("<td class='sig-position'>Настоятель храма</td>");
                html.AppendLine("<td class='sig-gap'></td>");
                html.AppendLine("<td class='sig-line' style='border-bottom: 1px solid #000;'>&nbsp;</td>");
                html.AppendLine("<td class='sig-name'>/ " + rector + " /</td>");
                html.AppendLine("</tr>");

                // Председатель
                html.AppendLine("<tr>");
                html.AppendLine("<td class='sig-position'>Председатель приходского совета</td>");
                html.AppendLine("<td class='sig-gap'></td>");
                html.AppendLine("<td class='sig-line' style='border-bottom: 1px solid #000;'>&nbsp;</td>");
                html.AppendLine("<td class='sig-name'>/ " + chairman + " /</td>");
                html.AppendLine("</tr>");

                // Казначей
                html.AppendLine("<tr>");
                html.AppendLine("<td class='sig-position'>Казначей</td>");
                html.AppendLine("<td class='sig-gap'></td>");
                html.AppendLine("<td class='sig-line' style='border-bottom: 1px solid #000;'>&nbsp;</td>");
                html.AppendLine("<td class='sig-name'>/ " + treasurer + " /</td>");
                html.AppendLine("</tr>");

                // Ревизионная комиссия
                html.AppendLine("<tr>");
                html.AppendLine("<td class='sig-position'>Член ревизионной комиссии</td>");
                html.AppendLine("<td class='sig-gap'></td>");
                html.AppendLine("<td class='sig-line' style='border-bottom: 1px solid #000;'>&nbsp;</td>");
                html.AppendLine("<td class='sig-name'>/ " + auditor + " /</td>");
                html.AppendLine("</tr>");

                html.AppendLine("</table>");
                html.AppendLine("<p style='margin-top:15px; font-weight: bold;'>М.П.</p>");

                html.AppendLine("</body></html>");

                return html.ToString();
            }

        // Вспомогательные методы
        private void AddIncomeRow(StringBuilder html, Dictionary<string, decimal> data, string code, string label, string note = "")
        {
            decimal amount = data.ContainsKey(code) ? data[code] : 0;
            html.AppendLine(string.Format("<tr><td></td><td class='indent'>{0}</td><td class='num'>{1:F2}</td><td>{2}</td></tr>",
                label, amount, note));
        }

        private void AddExpenseRow(StringBuilder html, Dictionary<string, decimal> data, string code, string label)
        {
            decimal amount = data.ContainsKey(code) ? data[code] : 0;
            html.AppendLine(string.Format("<tr><td></td><td class='expense-sub'>{0}</td><td class='num'>{1:F2}</td><td></td></tr>",
                label, amount));
        }

        // ✅ НОВЫЙ метод: без отступа и без жирного (для пунктов 10-17)
        private void AddExpenseRowNoIndent(StringBuilder html, Dictionary<string, decimal> data, string code, string label)
        {
            decimal amount = data.ContainsKey(code) ? data[code] : 0;
            html.AppendLine(string.Format("<tr><td></td><td class='bold'>{0}</td><td class='num bold'>{1:F2}</td><td></td></tr>",
                label, amount));
        }

        // ✅ НОВЫЙ метод: сумма по нескольким кодам
        private decimal GetExpenseSum(Dictionary<string, decimal> data, params string[] codes)
        {
            decimal sum = 0;
            foreach (string code in codes)
            {
                if (data.ContainsKey(code))
                    sum += data[code];
            }
            return sum;
        }
    }
}