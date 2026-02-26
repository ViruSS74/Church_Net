using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class IncomeForm : Form
    {
        // Строка подключения
        private string connectionString;
        private bool isDirty = false;
        private decimal currentTotalSum = 0;

        private ListOfDocsService _service;

        public IncomeForm()
        {
            InitializeComponent();
            _service = new ListOfDocsService(Program.DbPath);
            dtpDocDate.ValueChanged += (s, e) =>
            {
                txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Д");
            };

            // Динамически формируем путь к базе в папке Data
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            dbPath = Path.Combine(dbPath, "church.db"); // Убедитесь, что имя файла совпадает (church.db)
            connectionString = string.Format("Data Source={0};Version=3;", dbPath);

            // Обновление суммы при изменении данных в ячейках
            dgvItems.CellValueChanged += (s, e) => { isDirty = true; UpdateTotal(); };
            dgvItems.RowsRemoved += (s, e) => { isDirty = true; UpdateTotal(); };

            dtpDocDate.ValueChanged += (s, e) =>
            {
                txtDocNumber.Text = GenerateDocNumber(dtpDocDate.Value);
            };

            // Подписка на удаление строки клавишей Delete
            dgvItems.KeyDown += dgvItems_KeyDown;
        }

        private void IncomeForm_Load(object sender, EventArgs args)
        {
            this.Text = "Новый приходный документ";

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
            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Д");
            LoadCategories();
            UpdateTotal();

            // 4. Кнопки
            SetupButtons();
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
            Button[] buttons = { btnSaveDoc, btnNewDoc, btnClose };

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
            string prefix = string.Format("Д-{0}-", datePart);
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    // ВАЖНО: Сортируем по doc_number, чтобы корректно найти последний номер за день
                    string sql = "SELECT doc_number FROM income_docs WHERE doc_number LIKE @pref || '%' ORDER BY doc_number DESC LIMIT 1";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@pref", prefix);
                        var result = cmd.ExecuteScalar();
                        int nextId = 1;
                        if (result != null)
                        {
                            string lastNum = result.ToString();
                            string[] parts = lastNum.Split('-');
                            // Если номер Д-20022026-001, то индекс нужной части [2]
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
                    new SQLiteDataAdapter("SELECT id, name, parent_id FROM income_categories", conn).Fill(dt);
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
            // 1. Предварительная проверка UI
            if (dgvItems.Rows.Count == 0 || (dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow))
            {
                MessageBox.Show("Таблица пуста! Добавьте хотя бы одну позицию.");
                return;
            }

            // 2. Работа с базой (Одно открытие - все действия)
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Проверка уникальности номера прямо здесь, используя текущее соединение
                if (_service.IsNumberExists(txtDocNumber.Text, "income_docs", conn))
                {
                    MessageBox.Show("Документ №" + txtDocNumber.Text + " уже существует!");
                    return;
                }

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Сохраняем заголовок
                        string sqlDoc = "INSERT INTO income_docs (doc_number, date, total) VALUES (@n, @d, @t); SELECT last_insert_rowid();";
                        var cmdDoc = new SQLiteCommand(sqlDoc, conn, trans);
                        cmdDoc.Parameters.AddWithValue("@n", txtDocNumber.Text);
                        cmdDoc.Parameters.AddWithValue("@d", dtpDocDate.Value.ToString("yyyy-MM-dd"));
                        cmdDoc.Parameters.AddWithValue("@t", (double)currentTotalSum);
                        long docId = Convert.ToInt64(cmdDoc.ExecuteScalar());

                        // Сохраняем строки
                        foreach (DataGridViewRow row in dgvItems.Rows)
                        {
                            if (row.IsNewRow) continue;

                            var cmdItem = new SQLiteCommand("INSERT INTO income_items (doc_id, category, amount, description) VALUES (@id, @c, @a, @desc)", conn, trans);
                            cmdItem.Parameters.AddWithValue("@id", docId);
                            cmdItem.Parameters.AddWithValue("@c", row.Cells["colCategory"].Value);

                            decimal amt = 0;
                            decimal.TryParse(row.Cells["colAmount"].Value?.ToString(), out amt);
                            cmdItem.Parameters.AddWithValue("@a", (double)amt);
                            cmdItem.Parameters.AddWithValue("@desc", row.Cells["colDescription"].Value?.ToString() ?? "");

                            cmdItem.ExecuteNonQuery();
                        }

                        // --- НОВЫЙ БЛОК: Создание ПКО автоматически ---
                        string pkoNumber = _service.GetNextCashOrderNumber("ПКО", connectionString);
                        string sqlPKO = @"INSERT INTO cash_orders (order_type, order_number, doc_ref_id, date, amount, base) 
                  VALUES ('ПКО', @pNum, @refId, @date, @amt, @base)";
                        using (var cmdPko = new SQLiteCommand(sqlPKO, conn, trans))
                        {
                            cmdPko.Parameters.AddWithValue("@pNum", pkoNumber);
                            cmdPko.Parameters.AddWithValue("@refId", docId); // ID только что созданного дохода
                            cmdPko.Parameters.AddWithValue("@date", dtpDocDate.Value.ToString("yyyy-MM-dd"));
                            cmdPko.Parameters.AddWithValue("@amt", (double)currentTotalSum);
                            cmdPko.Parameters.AddWithValue("@base", "Приход по документу №" + txtDocNumber.Text);

                            cmdPko.ExecuteNonQuery();
                        }
                        // ----------------------------------------------

                        trans.Commit();
                        isDirty = false;
                        MessageBox.Show("Документ №" + txtDocNumber.Text + " успешно сохранен!");

                        // Очищаем форму ПОСЛЕ успешного коммита
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Ошибка сохранения: " + ex.Message);
                    }
                }
            }
        }

        private void ClearForm()
        {
            dgvItems.Rows.Clear();
            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Д");
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

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}