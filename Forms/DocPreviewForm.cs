using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class DocPreviewForm : Form
    {
        private int _docId;
        private string _type;
        private ListOfDocsService _service; // Добавляем поле сервиса

        // private string _connectionString = "Data Source=Data/church.db;Version=3;";

        // В конструктор теперь ПЕРЕДАЕМ сервис
        public DocPreviewForm(int docId, string type, ListOfDocsService service)
        {
            InitializeComponent();
            _docId = docId;
            _type = type;
            _service = service; // Принимаем готовый сервис
        }

        private void DocPreviewForm_Load(object sender, EventArgs e)
        {
            if (_type == "Доходы" || _type == "Income")
                lblActionType.Text = "Принято:";
            else
                lblActionType.Text = "Потрачено:";

            LoadOrganizationData();

            // 1. Сначала получаем основные данные документа (нужно для даты)
            DataTable docInfo = _service.GetDocumentItems(_type, _docId);

            if (docInfo != null && docInfo.Rows.Count > 0)
            {
                // Выставляем дату в шапку (lblDocDate)
                DataRow firstRow = docInfo.Rows[0];
                if (docInfo.Columns.Contains("date") && firstRow["date"] != DBNull.Value)
                {
                    lblDocDate.Text = Convert.ToDateTime(firstRow["date"]).ToString("dd.MM.yyyy");
                }
            }

            // 2. Получаем ПОЛНУЮ таблицу для Рапортички (все статьи из справочника + суммы)
            DataTable items = _service.GetFullRaportichka(_type, _docId);

            if (items != null && items.Rows.Count > 0)
            {
                dgvPrint.DataSource = items;

                // --- НАСТРОЙКИ ВНЕШНЕГО ВИДА (БЛАНК) ---
                dgvPrint.Columns["display_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPrint.Columns["amount"].Width = 100;
                dgvPrint.Columns["amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPrint.Columns["amount"].DefaultCellStyle.Format = "N2";

                // Скрываем всё лишнее (сетку, заголовки, рамки)
                dgvPrint.ColumnHeadersVisible = false;
                dgvPrint.RowHeadersVisible = false;
                dgvPrint.AllowUserToAddRows = false;
                dgvPrint.BackgroundColor = Color.White;
                dgvPrint.BorderStyle = BorderStyle.None;
                dgvPrint.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dgvPrint.ReadOnly = true;
                dgvPrint.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Шрифт
                dgvPrint.DefaultCellStyle.Font = new Font("Arial", 10);

                // Расчет и вывод итоговой суммы
                decimal totalSum = 0;
                foreach (DataRow row in items.Rows)
                {
                    if (row["amount"] != DBNull.Value)
                        totalSum += Convert.ToDecimal(row["amount"]);
                }

                // Форматируем строку: слово "Итого:", число с двумя знаками после запятой и валюта
                lblTotal.Text = string.Format("Итого: {0:N2} руб.", totalSum);
            }
            else
            {
                MessageBox.Show("Не удалось сформировать список статей.");
            }

            LoadPersonalToCombo();
        }

        private void LoadOrganizationData()
        {
            DataRow org = _service.GetOrganizationData();
            if (org != null)
            {
                lblOrgName.Text = org["name"].ToString();
                lblOrgDetails.Text = string.Format("{0}, {1}", org["location"], org["deanery"]);
            }
        }
        //private void LoadDocumentData()
        //{
        //    // 1. Определяем имена таблиц (учитываем оба варианта написания)
        //    bool isIncome = (_type == "Доход" || _type == "Доходы" || _type == "Income");

        //    string tableDocs = isIncome ? "income_docs" : "expense_docs";
        //    string tableItems = isIncome ? "income_items" : "expense_items";
        //    string tableCats = isIncome ? "income_categories" : "expensee_categories"; // Ваша таблица с двумя 'e'

        //    using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
        //    {
        //        conn.Open();

        //        // 2. Загрузка даты документа
        //        string sqlDoc = string.Format("SELECT date FROM {0} WHERE id = @id", tableDocs);
        //        using (SQLiteCommand cmd = new SQLiteCommand(sqlDoc, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@id", _docId);
        //            object dateRaw = cmd.ExecuteScalar();
        //            if (dateRaw != null)
        //            {
        //                DateTime dt = DateTime.Parse(dateRaw.ToString());
        //                lblDocDate.Text = dt.ToString("dd MMMM yyyy 'г.'", new System.Globalization.CultureInfo("ru-RU"));
        //            }
        //        }

        //        // 3. Заголовок (Принято / Расходовано)
        //        lblActionType.Text = isIncome ? "Принято:" : "Расходовано:";
        //        cmbDocType.Text = isIncome ? "РАПОРТИЧКА" : "РАСХОДЫ";

        //        // 4. Загрузка позиций С НАЗВАНИЯМИ КАТЕГОРИЙ (JOIN)
        //        // Мы берем c.name и называем его 'category', чтобы DataGridView его подхватил
        //        string sqlItems = string.Format(@"
        //    SELECT c.name as category, i.amount 
        //    FROM {0} i
        //    JOIN {1} c ON i.category = c.id
        //    WHERE i.doc_id = @id", tableItems, tableCats);

        //        SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlItems, conn);
        //        adapter.SelectCommand.Parameters.AddWithValue("@id", _docId);
        //        DataTable dtItems = new DataTable();
        //        adapter.Fill(dtItems);

        //        // Привязываем данные
        //        dgvPrint.DataSource = dtItems;

        //        // 5. Расчет Итого
        //        decimal totalSum = 0;
        //        foreach (DataRow row in dtItems.Rows)
        //        {
        //            if (row["amount"] != DBNull.Value)
        //                totalSum += Convert.ToDecimal(row["amount"]);
        //        }
        //        lblTotal.Text = "Итого: " + totalSum.ToString("N2") + " руб.";

        //        // 6. Оформление таблицы (выполнять только если есть колонки)
        //        if (dgvPrint.Columns.Count > 0)
        //        {
        //            dgvPrint.ColumnHeadersVisible = false;
        //            dgvPrint.RowHeadersVisible = false;
        //            dgvPrint.BorderStyle = BorderStyle.None;
        //            dgvPrint.CellBorderStyle = DataGridViewCellBorderStyle.None;

        //            if (dgvPrint.Columns.Contains("category"))
        //                dgvPrint.Columns["category"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        //            if (dgvPrint.Columns.Contains("amount"))
        //            {
        //                dgvPrint.Columns["amount"].Width = 100;
        //                dgvPrint.Columns["amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //                dgvPrint.Columns["amount"].DefaultCellStyle.Format = "N2";
        //            }
        //        }
        //    }
        //}

        private void LoadPersonalToCombo()
        {
            using (SQLiteConnection conn = new SQLiteConnection(_service.ConnectionString))
            {
                try
                {
                    conn.Open();
                    // Выбираем данные точно по именам колонок со скриншота
                    string sql = "SELECT last_name, first_name, middle_name, role FROM personal";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        cmbPersonal.Items.Clear();
                        int treasurerIndex = -1;
                        int currentIndex = 0;

                        while (reader.Read())
                        {
                            string ln = reader["last_name"].ToString();
                            string fn = reader["first_name"].ToString();
                            string mn = reader["middle_name"].ToString();
                            string role = reader["role"].ToString();

                            // Безопасное получение первой буквы для инициалов
                            string f = fn.Length > 0 ? fn.Substring(0, 1) : "";
                            string m = mn.Length > 0 ? mn.Substring(0, 1) : "";

                            // Формируем: Драгун Е. В.
                            string shortName = string.Format("{0} {1}.{2}.", ln, f, m);
                            cmbPersonal.Items.Add(shortName);

                            // Если в колонке role написано "Казначей" — запоминаем индекс
                            if (role.Trim() == "Казначей")
                            {
                                treasurerIndex = currentIndex;
                            }

                            currentIndex++;
                        }

                        // Устанавливаем казначея по умолчанию
                        if (treasurerIndex != -1)
                            cmbPersonal.SelectedIndex = treasurerIndex;
                        else if (cmbPersonal.Items.Count > 0)
                            cmbPersonal.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке сотрудников: " + ex.Message);
                }
            }
        }

        private string FormatShortName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return "";
            string[] p = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (p.Length >= 3)
                return string.Format("{0} {1}.{2}.", p[0], p[1].Substring(0, 1), p[2].Substring(0, 1));
            return fullName;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // 1. Прячем кнопки, чтобы не попали в кадр
            panelButtons.Visible = false; // замените на имя вашей панели
            dgvPrint.ClearSelection();

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPageHandler);

            PrintDialog pDialog = new PrintDialog();
            pDialog.Document = pd;

            // ПАРАМЕТР 1: Заставляет Windows 11 показать современное окно выбора принтера
            pDialog.UseEXDialog = true;

            if (pDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }

            panelButtons.Visible = true;
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            // 1. Запоминаем текущий стиль рамки
            FormBorderStyle oldStyle = this.FormBorderStyle;

            // 2. ВРЕМЕННО УБИРАЕМ ЗАГОЛОВОК ОКНА (делаем форму "голым листом")
            this.FormBorderStyle = FormBorderStyle.None;

            // Перерисовываем форму, чтобы заголовок исчез из памяти
            this.Refresh();

            // 3. Делаем снимок ЧИСТОЙ формы
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));

            // 4. ВОЗВРАЩАЕМ ЗАГОЛОВОК ОКНА НАЗАД (чтобы пользователь мог закрыть форму)
            this.FormBorderStyle = oldStyle;

            // 5. Выводим чистый снимок на печать с небольшим отступом
            e.Graphics.DrawImage(bmp, 50, 50);

            // Освобождаем память
            bmp.Dispose();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            dgvPrint.ClearSelection();
            panelButtons.Visible = false;

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPageHandler);

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.WindowState = FormWindowState.Maximized;
            ppd.ShowDialog();

            panelButtons.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}