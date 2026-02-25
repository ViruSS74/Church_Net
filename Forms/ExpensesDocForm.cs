using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class ExpensesDocForm : Form
    {
        private string connectionString;
        private bool isDirty = false;
        private decimal currentTotalSum = 0;
        private ListOfDocsService _service; // Сервис для работы с БД

        public ExpensesDocForm()
        {
            InitializeComponent();

            // 1. Настройка пути к базе (делаем ОДИН раз)
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            dbPath = Path.Combine(dbPath, "church.db");
            connectionString = string.Format("Data Source={0};Version=3;", dbPath);

            // 2. Инициализация сервиса
            _service = new ListOfDocsService(connectionString);

            // 3. Подготовка папки для чеков
            string receiptsDir = Path.Combine(Application.StartupPath, "Receipts");
            if (!Directory.Exists(receiptsDir))
            {
                Directory.CreateDirectory(receiptsDir);
            }

            // 4. Логика генерации номера при изменении даты
            dtpDocDate.ValueChanged += (s, e) =>
            {
                // Используем метод из сервиса (префикс "Р" для Расхода/РКО)
                txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");
            };

            // 5. Подписки на события таблицы и элементов
            dgvItems.CellValueChanged += (s, e) => { isDirty = true; UpdateTotal(); };
            dgvItems.RowsRemoved += (s, e) => { isDirty = true; UpdateTotal(); };

            dgvItems.KeyDown += dgvItems_KeyDown;
            dgvItems.SelectionChanged += dgvItems_SelectionChanged;

            pbReceipt.Click += pbReceipt_Click;
            pbReceipt.DoubleClick += pbReceipt_DoubleClick;

            // Первичная генерация номера при открытии формы
            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");

            // Обновление суммы при изменении данных в ячейках
            dgvItems.CellValueChanged += (s, e) => { isDirty = true; UpdateTotal(); };
            dgvItems.RowsRemoved += (s, e) => { isDirty = true; UpdateTotal(); };

            dtpDocDate.ValueChanged += (s, e) =>
            {
                txtDocNumber.Text = GenerateDocNumber(dtpDocDate.Value);
            };

            // Подписка на удаление строки клавишей Delete
            dgvItems.KeyDown += dgvItems_KeyDown;

            // Подписка на смену строки (чтобы менялось фото чека справа)
            dgvItems.SelectionChanged += dgvItems_SelectionChanged;

            // Подписка на клик по PictureBox (выбор чека)
            pbReceipt.Click += pbReceipt_Click;

            // Подписка на двойной клик по фото (открыть для просмотра/печати)
            pbReceipt.DoubleClick += pbReceipt_DoubleClick;
        }

        private void ExpensesDocForm_Load(object sender, EventArgs args)
        {
            this.Text = "Новый расходный документ";

            // 1. Настройки дерева
            tvCategories.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            tvCategories.ItemHeight = 30;

            // 2. Настройки таблицы
            dgvItems.DefaultCellStyle.Font = new Font("Segoe UI", 11F);
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvItems.RowTemplate.Height = 28;
            dgvItems.RowHeadersVisible = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvItems.Columns["colCategory"].Width = 300;
            dgvItems.Columns["colAmount"].Width = 80;
            dgvItems.Columns["colDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvItems.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Скрываем техническую колонку с путем к чеку
            if (dgvItems.Columns.Contains("colReceiptPath"))
            {
                dgvItems.Columns["colReceiptPath"].Visible = false;
            }

            // Подписка на колесико мыши
            dgvItems.EditingControlShowing += (s, ev) =>
            {
                if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["colAmount"].Index)
                {
                    TextBox tb = ev.Control as TextBox;
                    if (tb != null)
                    {
                        tb.SelectAll();
                        tb.MouseWheel -= Amount_MouseWheel; // Удаляем старую подписку
                        tb.MouseWheel += Amount_MouseWheel; // Добавляем новую
                    }
                }
            };

            // 3. Загрузка данных
            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");
            LoadCategories();
            UpdateTotal();

            // 4. Кнопки
            SetupButtons();
        }

        // При смене строки в таблице — показываем чек этой строки
        private void dgvItems_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow != null)
            {
                // Извлекаем путь (безопасно приводим к строке)
                string path = dgvItems.CurrentRow.Cells["colReceiptPath"].Value as string;
                ShowReceipt(path);
            }
        }

        // Вспомогательный метод для корректного отображения картинки (ОДИН экземпляр)
        private void ShowReceipt(string path)
        {
            try
            {
                // 1. Освобождаем память от старого изображения, если оно было
                if (pbReceipt.Image != null)
                {
                    pbReceipt.Image.Dispose();
                    pbReceipt.Image = null;
                }

                // 2. Если пути нет, выходим
                if (string.IsNullOrEmpty(path)) return;

                // 3. Собираем путь (матрешка Path.Combine специально для .NET 3.5)
                string receiptsDir = Path.Combine(Application.StartupPath, "Receipts");
                string fullPath = Path.IsPathRooted(path)
                    ? path
                    : Path.Combine(receiptsDir, path);

                // 4. Загружаем картинку, если файл существует
                if (File.Exists(fullPath))
                {
                    // Открываем файл через поток, чтобы не блокировать его в системе
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        using (Image tempImg = Image.FromStream(fs))
                        {
                            // Создаем независимую копию в памяти (Bitmap)
                            pbReceipt.Image = new Bitmap(tempImg);
                        }
                    }
                    pbReceipt.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch
            {
                // Если файл поврежден или это не картинка — просто очищаем превью
                pbReceipt.Image = null;
            }
        }

        // Выбор чека при клике на PictureBox
        private void pbReceipt_Click(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow == null || dgvItems.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Выберите строку в таблице, к которой хотите прикрепить чек.");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dgvItems.CurrentRow.Cells["colReceiptPath"].Value = ofd.FileName;
                    ShowReceipt(ofd.FileName);
                    isDirty = true;
                }
            }
        }

        // Открытие чека в системном просмотрщике (для печати/просмотра)
        private void pbReceipt_DoubleClick(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow != null)
            {
                string path = dgvItems.CurrentRow.Cells["colReceiptPath"].Value as string;
                if (!string.IsNullOrEmpty(path))
                {
                    string fullPath = Path.IsPathRooted(path) ? path : Path.Combine(Path.Combine(Application.StartupPath, "Receipts"), path);
                    if (File.Exists(fullPath)) Process.Start(fullPath);
                }
            }
        }

        private void Amount_MouseWheel(object sender, MouseEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (decimal.TryParse(tb.Text, out decimal val))
            {
                // Логика: меняем только целое число (рубли)
                decimal rubles = Math.Truncate(val);
                decimal kopeks = val - rubles;

                if (e.Delta > 0) rubles += 1; // Вверх +1 рубль
                else rubles -= 1;             // Вниз -1 рубль

                if (rubles < 0) rubles = 0;

                tb.Text = (rubles + kopeks).ToString("N2");
                tb.SelectAll(); // Сохраняем выделение для удобства
            }
        }

        private void SetupButtons()
        {
            // Укажите здесь имена ваших кнопок (Save, New, Close и т.д.)
            Button[] buttons = { btnSaveDoc, btnNewDoc, btnAttachReceipt, btnClose };

            foreach (var btn in buttons)
            {
                if (btn != null)
                {
                    // Уменьшаем иконку 64x64 до 24x24
                    if (btn.Image != null) btn.Image = ImageHelper.Resize(btn.Image, 24);

                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(10, 0, 0, 0); // Небольшой отступ слева
                }
            }
        }

        // 1. Генерация уникального номера документа (Д-ддммгггг-00№)
        private string GenerateDocNumber(DateTime selectedDate)
        {
            string datePart = selectedDate.ToString("ddMMyyyy");
            string prefix = string.Format("Р-{0}-", datePart);
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    // ВАЖНО: Сортируем по doc_number, чтобы корректно найти последний номер за день
                    string sql = "SELECT doc_number FROM expense_docs WHERE doc_number LIKE @pref || '%' ORDER BY doc_number DESC LIMIT 1";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@pref", prefix);
                        var result = cmd.ExecuteScalar();
                        int nextId = 1;
                        if (result != null)
                        {
                            string lastNum = result.ToString();
                            string[] parts = lastNum.Split('-');
                            // Если номер H-20022026-001, то индекс нужной части [2]
                            if (parts.Length == 3 && int.TryParse(parts[2], out nextId))
                            {
                                nextId++;
                            }
                        }
                        return string.Format("{0}{1:D3}", prefix, nextId);
                    }
                }
            }
            catch { return prefix + "001"; }
        }

        // 2. Загрузка дерева категорий (рекурсия)
        private void LoadCategories()
        {
            tvCategories.Nodes.Clear();
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    new SQLiteDataAdapter("SELECT id, name, parent_id FROM expense_categories", conn).Fill(dt);
                }
                AddNodes(null, 0, dt);
                tvCategories.ExpandAll();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка БД: " + ex.Message); }
        }

        private void AddNodes(TreeNode parentNode, int parentId, DataTable dt)
        {
            string filter = parentId == 0 ? "parent_id IS NULL OR parent_id = 0" : "parent_id = " + parentId;
            DataRow[] rows = dt.Select(filter);

            foreach (DataRow row in rows)
            {
                TreeNode node = new TreeNode(row["name"].ToString());
                node.Tag = row["id"];
                if (parentNode == null) tvCategories.Nodes.Add(node);
                else parentNode.Nodes.Add(node);
                AddNodes(node, Convert.ToInt32(row["id"]), dt);
            }
        }

        // 3. Добавление категории в таблицу по двойному клику
        private void tvCategories_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Добавляем, только если это "лист" (конечная категория без подкатегорий)
            if (e.Node.Nodes.Count == 0)
            {
                try
                {
                    // 1. Сначала добавляем новую пустую строку и получаем её индекс
                    int index = dgvItems.Rows.Add();

                    // 2. Теперь берем ЭТУ конкретную строку по индексу и заполняем её ячейки (Cells)
                    dgvItems.Rows[index].Cells["colCategory"].Value = e.Node.Text;
                    dgvItems.Rows[index].Cells["colAmount"].Value = 0.00m;

                    isDirty = true;

                    // 3. Устанавливаем фокус на ввод суммы в созданной строке
                    dgvItems.CurrentCell = dgvItems.Rows[index].Cells["colAmount"];
                    dgvItems.BeginEdit(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message +
                        "\n\nПроверьте в дизайнере (Edit Columns), что (Name) колонок " +
                        "точно 'colCategory' и 'colAmount'!");
                }
            }
        }

        // 4. Расчет итоговой суммы
        private void UpdateTotal()
        {
            currentTotalSum = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;
                object val = row.Cells["colAmount"].Value;
                if (val != null && val != DBNull.Value)
                {
                    decimal amount;
                    if (decimal.TryParse(val.ToString(), out amount))
                        currentTotalSum += amount;
                }
            }
            lblTotal.Text = string.Format("Итого: {0:N2} руб.", currentTotalSum);
        }

        // 5. Сохранение документа (Транзакция)
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Базовые проверки
            if (dgvItems.Rows.Count == 0 || (dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow))
            {
                MessageBox.Show("Таблица пуста!"); return;
            }

            // 2. ПРОВЕРКА НА ДУБЛИКАТ
            if (_service.IsNumberExists(txtDocNumber.Text, "expense_docs"))
            {
                MessageBox.Show("Документ с таким номером уже существует!", "Внимание");
                return;
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Сохраняем заголовок (expense_docs)
                        string sqlDoc = "INSERT INTO expense_docs (doc_number, date, total) VALUES (@n, @d, @t);";
                        var cmdDoc = new SQLiteCommand(sqlDoc, conn, trans);
                        cmdDoc.Parameters.AddWithValue("@n", txtDocNumber.Text);
                        cmdDoc.Parameters.AddWithValue("@d", dtpDocDate.Value.ToString("yyyy-MM-dd"));
                        cmdDoc.Parameters.AddWithValue("@t", (double)currentTotalSum);
                        cmdDoc.ExecuteNonQuery();

                        long docId = conn.LastInsertRowId; // Получаем ID созданного расхода

                        // 2. Сохраняем строки (expense_items)
                        foreach (DataGridViewRow row in dgvItems.Rows)
                        {
                            if (row.IsNewRow) continue;
                            var cmdItem = new SQLiteCommand("INSERT INTO expense_items (doc_id, category, amount, description) VALUES (@id, @c, @a, @desc)", conn, trans);
                            cmdItem.Parameters.AddWithValue("@id", docId);
                            cmdItem.Parameters.AddWithValue("@c", row.Cells["colCategory"].Value);

                            decimal amt = 0;
                            decimal.TryParse(row.Cells["colAmount"].Value?.ToString(), out amt);
                            cmdItem.Parameters.AddWithValue("@a", (double)amt);
                            cmdItem.Parameters.AddWithValue("@desc", row.Cells["colDescription"].Value?.ToString() ?? "");
                            cmdItem.ExecuteNonQuery();
                        }

                        // --- НОВЫЙ БЛОК: Создание РКО автоматически ---
                        // 1. Получаем следующий номер для РКО через сервис
                        string rkoNumber = _service.GetNextCashOrderNumber("РКО", connectionString);

                        // 2. Запрос на вставку (добавляем поля для будущей привязки человека)
                        string sqlRKO = @"INSERT INTO cash_orders 
                (order_type, order_number, doc_ref_id, date, amount, base, person_id, person_name_manual) 
                VALUES ('РКО', @rNum, @refId, @date, @amt, @base, @pId, @pManual)";

                        using (var cmdRko = new SQLiteCommand(sqlRKO, conn, trans))
                        {
                            cmdRko.Parameters.AddWithValue("@rNum", rkoNumber);
                            cmdRko.Parameters.AddWithValue("@refId", docId); // ID только что созданного расхода
                            cmdRko.Parameters.AddWithValue("@date", dtpDocDate.Value.ToString("yyyy-MM-dd"));
                            cmdRko.Parameters.AddWithValue("@amt", (double)currentTotalSum);

                            // Формируем основание (текст txtBasis можно добавить, если он есть на форме)
                            cmdRko.Parameters.AddWithValue("@base", "Расход по документу №" + txtDocNumber.Text);

                            // Устанавливаем пустые значения для получателя (выберем их в "Редактировании")
                            cmdRko.Parameters.AddWithValue("@pId", DBNull.Value);
                            cmdRko.Parameters.AddWithValue("@pManual", DBNull.Value);

                            cmdRko.ExecuteNonQuery();
                        }
                        // ----------------------------------------------

                        trans.Commit();
                        isDirty = false;
                        MessageBox.Show("Документ №" + txtDocNumber.Text + " и РКО №" + rkoNumber + " сохранены!");

                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Ошибка при сохранении: " + ex.Message);
                    }
                }
            }
        }

        private void ClearForm()
        {
            dgvItems.Rows.Clear();
            // Генерируем номер для ТОЙ ЖЕ даты, которая выбрана в календаре сейчас
            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");
            isDirty = false;
            UpdateTotal();
        }

        // Удаление строки клавишей Delete
        private void dgvItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvItems.CurrentRow != null && !dgvItems.CurrentRow.IsNewRow)
            {
                dgvItems.Rows.Remove(dgvItems.CurrentRow);
                isDirty = true;
                UpdateTotal();
            }
        }

        private void btnNewDoc_Click(object sender, EventArgs e)
        {
            if (isDirty)
            {
                var res = MessageBox.Show("Сохранить изменения?", "Вопрос", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes) btnSave_Click(null, null);
                else if (res == DialogResult.Cancel) return;
            }
            ClearForm();
        }

        private void btnAttachReceipt_Click(object sender, EventArgs e)
        {
            // 1. Проверка на наличие строк (безопаснее проверять Count)
            if (dgvItems.CurrentRow == null || dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("Сначала добавьте или выберите строку в таблице!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp|Все файлы|*.*";
                ofd.Title = "Выберите фото чека";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Для .NET 3.5 собираем путь "матрешкой"
                        string receiptsDir = Path.Combine(Application.StartupPath, "Receipts");
                        if (!Directory.Exists(receiptsDir)) Directory.CreateDirectory(receiptsDir);

                        string ext = Path.GetExtension(ofd.FileName);
                        string newFileName = "receipt_" + DateTime.Now.Ticks.ToString() + ext;
                        string destPath = Path.Combine(receiptsDir, newFileName);

                        File.Copy(ofd.FileName, destPath, true);

                        // ЗАПИСЬ: Принудительно завершаем редактирование ячейки, чтобы данные "прилипли"
                        dgvItems.EndEdit();
                        dgvItems.CurrentRow.Cells["colReceiptPath"].Value = newFileName;

                        // ОБНОВЛЕНИЕ ПРЕВЬЮ
                        ShowReceipt(newFileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при сохранении чека: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}