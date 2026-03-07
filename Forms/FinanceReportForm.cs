using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class FinanceReportForm : Form
    {
        public FinanceReportForm()
        {
            InitializeComponent();

            ImageHelper.ApplyToButtons(this, 24);

            ImageHelper.ApplyToDataGridViews(this);
        }

        // Обработка кнопок
        //private void btnView_Click(object sender, EventArgs e)
        //{
        //    // Создаем документ для печати
        //    PrintDocument pd = new PrintDocument();
        //    // Подписываем его на наш метод рисования
        //    pd.PrintPage += new PrintPageEventHandler(PrintFinreportPage);

        //    // Создаем окно предпросмотра
        //    PrintPreviewDialog ppd = new PrintPreviewDialog();
        //    ppd.Document = pd;

        //    // На Windows 8 и выше это окно будет выглядеть современно и аккуратно
        //    ppd.ShowDialog();
        //}

        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        PrintDocument pd = new PrintDocument();

        //        // Проверяем, какая вкладка выбрана (используем Name вкладки)
        //        if (tabFinreport.SelectedTab.Name == "tabPrintForm")
        //        {
        //            pd.DefaultPageSettings.Landscape = false;
        //            pd.PrintPage += new PrintPageEventHandler(PrintFinreportPage);
        //        }
        //        else if (tabFinereport.SelectedTab.Name == "tabData")
        //        {
        //            // ПРИНУДИТЕЛЬНО устанавливаем альбомную ориентацию
        //            pd.DefaultPageSettings.Landscape = true;
        //            // Некоторые принтеры требуют установки Landscape и в PrinterSettings
        //            pd.PrinterSettings.DefaultPageSettings.Landscape = true;

        //            pd.PrintPage += new PrintPageEventHandler(PrintFinereportPage);
        //        }

        //        PrintDialog pDialog = new PrintDialog();
        //        pDialog.Document = pd;
        //        pDialog.UseEXDialog = true;

        //        if (pDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            pd.Print();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка при печати: " + ex.Message);
        //    }
        //}

        private int currentRowIndex = 0;
        //private void PrintFinreportPage(object sender, PrintPageEventArgs e)
        //{
        //    // Настройки шрифтов
        //    Font fHeader = new Font("Arial", 12, FontStyle.Bold);
        //    Font fCell = new Font("Arial", 11);
        //    Pen pen = new Pen(Color.Black, 1);
        //    int x = 40;
        //    int y = 40;
        //    // 1. Отрисовка шапки таблицы
        //    for (int i = 0; i < dgvData.Columns.Count; i++)
        //    {
        //        Rectangle rect = new Rectangle(x, y, colWidths[i], headerHeight);
        //        g.DrawRectangle(pen, rect);
        //        using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        //        {
        //            g.DrawString(dgvData.Columns[i].HeaderText, fHeader, Brushes.Black, rect, sf);
        //        }
        //        x += colWidths[i];
        //    }
        //    y += headerHeight;
        //    // 2. Отрисовка строк данных
        //    while (currentRowIndex < dgvData.Rows.Count)
        //    {
        //        DataGridViewRow row = dgvData.Rows[currentRowIndex];
        //        if (row.IsNewRow) { currentRowIndex++; continue; }
        //        // MeasureString учитывает переносы при заданной ширине колонки
        //        SizeF size = g.MeasureString(basisText, fCell, colWidths[4]);
        //        int rowHeight = Math.Max(25, (int)Math.Ceiling(size.Height) + 10); // +10 для полей
        //        // Проверка: влезет ли строка на текущую страницу
        //        if (y + rowHeight > e.MarginBounds.Bottom)
        //        {
        //            e.HasMorePages = true; // Будет вызван этот же метод для следующей страницы
        //            return;
        //        }
        //        x = 40;
        //        for (int i = 0; i < dgvData.Columns.Count; i++)
        //        {
        //            Rectangle rect = new Rectangle(x, y, colWidths[i], rowHeight);
        //            g.DrawRectangle(pen, rect);
        //            string cellValue = (row.Cells[i].Value ?? "").ToString();
        //            using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Near })
        //            {
        //                // Выравнивание: номера колонок (строка 0) по центру, остальное по левому краю
        //                sf.Alignment = (currentRowIndex == 0) ? StringAlignment.Center : StringAlignment.Near;
        //                // Включаем перенос слов для длинного текста
        //                sf.FormatFlags = StringFormatFlags.LineLimit;
        //                g.DrawString(cellValue, fCell, Brushes.Black, rect, sf);
        //            }
        //            x += colWidths[i];
        //        }
        //        y += rowHeight;
        //        currentRowIndex++;
        //    }
        //    // Сбрасываем индекс после завершения всей печати
        //    currentRowIndex = 0;
        //    e.HasMorePages = false;
        //}

        //private void btnSaveEdit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int? personId = null;
        //        if (cmbRecipient.SelectedValue != null && cmbRecipient.SelectedValue != DBNull.Value)
        //        {
        //            int val = Convert.ToInt32(cmbRecipient.SelectedValue);
        //            if (val != -1) personId = val;
        //        }

        //        // Обновляем базу
        //        bool isUpdated = _service.UpdateCashOrder(this._orderId, personId, txtEditBasis.Text, txtEditAppendix.Text);

        //        if (isUpdated)
        //        {
        //            MessageBox.Show("Данные РКО успешно изменены!");

        //            // 1. Обновляем переменные класса из БД (чтобы паспорт и ФИО подтянулись в _personPassportData)
        //            LoadOrderDataForPrint(_orderId);

        //            // 2. Перерисовываем структуру таблицы (теперь она чистая из БД)
        //            LoadRkoRegistryTable();

        //            // 3. ВАЖНО: Принудительно вписываем выбранные в ComboBox данные в строку 2 (индекс 2)
        //            // Это не даст таблице показать "первого по алфавиту" или пустоту
        //            if (dgvData.DataSource != null)
        //            {
        //                DataTable dt = (DataTable)dgvData.DataSource;
        //                if (dt.Rows.Count > 2)
        //                {
        //                    dt.Rows[2]["1"] = cmbRecipient.Text;        // ФИО
        //                    dt.Rows[2]["1а"] = _personPassportData;    // Паспорт из обновленного метода LoadOrderData
        //                    dt.Rows[2]["3"] = txtEditAppendix.Text;    // Приложение из текстового поля
        //                }
        //            }

        //            ppControl.InvalidatePreview(); // Обновляем предпросмотр печати
        //        }
        //        else
        //        {
        //            MessageBox.Show("Не удалось найти запись для обновления.");
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show("Ошибка при сохранении: " + ex.Message); }
        //}

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}
