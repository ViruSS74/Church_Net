using System;
using System.Collections.Generic;
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
        private ListOfDocsService _service;

        public ExpensesDocForm()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            dbPath = Path.Combine(dbPath, "church.db");
            connectionString = string.Format("Data Source={0};Version=3;", dbPath);

            _service = new ListOfDocsService(connectionString);

            string receiptsDir = Path.Combine(Application.StartupPath, "Receipts");
            if (!Directory.Exists(receiptsDir))
            {
                Directory.CreateDirectory(receiptsDir);
            }

            dtpDocDate.ValueChanged += (s, e) =>
            {
                txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");
            };

            dgvItems.CellValueChanged += (s, e) => { isDirty = true; UpdateTotal(); };
            dgvItems.RowsRemoved += (s, e) => { isDirty = true; UpdateTotal(); };
            dgvItems.KeyDown += dgvItems_KeyDown;
            dgvItems.SelectionChanged += dgvItems_SelectionChanged;
            pbReceipt.Click += pbReceipt_Click;
            pbReceipt.DoubleClick += pbReceipt_DoubleClick;

            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");
        }

        private void ExpensesDocForm_Load(object sender, EventArgs args)
        {
            this.Text = "Новый расходный документ";

            tvCategories.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            tvCategories.ItemHeight = 30;

            dgvItems.DefaultCellStyle.Font = new Font("Segoe UI", 11F);
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvItems.RowTemplate.Height = 28;
            dgvItems.RowHeadersVisible = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvItems.Columns.Clear();

            DataGridViewTextBoxColumn colCategory = new DataGridViewTextBoxColumn();
            colCategory.Name = "colCategory";
            colCategory.HeaderText = "Категория";
            colCategory.Width = 300;
            dgvItems.Columns.Add(colCategory);

            DataGridViewTextBoxColumn colAmount = new DataGridViewTextBoxColumn();
            colAmount.Name = "colAmount";
            colAmount.HeaderText = "Сумма";
            colAmount.Width = 80;
            dgvItems.Columns.Add(colAmount);

            DataGridViewTextBoxColumn colCheckNumber = new DataGridViewTextBoxColumn();
            colCheckNumber.Name = "colCheckNumber";
            colCheckNumber.HeaderText = "№ Чека";
            colCheckNumber.Width = 80;
            dgvItems.Columns.Add(colCheckNumber);

            DataGridViewTextBoxColumn colBasis = new DataGridViewTextBoxColumn();
            colBasis.Name = "colBasis";
            colBasis.HeaderText = "Основание";
            colBasis.Width = 150;
            dgvItems.Columns.Add(colBasis);

            DataGridViewTextBoxColumn colReceiptPath = new DataGridViewTextBoxColumn();
            colReceiptPath.Name = "colReceiptPath";
            colReceiptPath.HeaderText = "Путь";
            colReceiptPath.Visible = false;
            dgvItems.Columns.Add(colReceiptPath);

            dgvItems.Columns["colAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItems.Columns["colCheckNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvItems.EditingControlShowing += (s, ev) =>
            {
                if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["colAmount"].Index)
                {
                    TextBox tb = ev.Control as TextBox;
                    if (tb != null)
                    {
                        tb.SelectAll();
                        tb.MouseWheel -= Amount_MouseWheel;
                        tb.MouseWheel += Amount_MouseWheel;
                    }
                }
            };

            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");
            LoadCategories();
            UpdateTotal();

            ImageHelper.ApplyToButtons(this, 24);
        }

        private void dgvItems_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow != null)
            {
                string path = dgvItems.CurrentRow.Cells["colReceiptPath"].Value as string;
                ShowReceipt(path);
            }
        }

        private void ShowReceipt(string path)
        {
            try
            {
                if (pbReceipt.Image != null)
                {
                    pbReceipt.Image.Dispose();
                    pbReceipt.Image = null;
                }

                if (string.IsNullOrEmpty(path)) return;

                string receiptsDir = Path.Combine(Application.StartupPath, "Receipts");
                string fullPath = Path.IsPathRooted(path) ? path : Path.Combine(receiptsDir, path);

                if (File.Exists(fullPath))
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        using (Image tempImg = Image.FromStream(fs))
                        {
                            pbReceipt.Image = new Bitmap(tempImg);
                        }
                    }
                    pbReceipt.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch
            {
                pbReceipt.Image = null;
            }
        }

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
                    string fileName = Path.GetFileName(ofd.FileName);
                    string receiptsDir = Path.Combine(Application.StartupPath, "Receipts");
                    string destPath = Path.Combine(receiptsDir, fileName);

                    File.Copy(ofd.FileName, destPath, true);

                    dgvItems.CurrentRow.Cells["colReceiptPath"].Value = fileName;
                    ShowReceipt(fileName);
                    isDirty = true;
                }
            }
        }

        private void pbReceipt_DoubleClick(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow != null)
            {
                string path = dgvItems.CurrentRow.Cells["colReceiptPath"].Value as string;
                if (!string.IsNullOrEmpty(path))
                {
                    string fullPath = Path.Combine(Path.Combine(Application.StartupPath, "Receipts"), path);
                    if (File.Exists(fullPath)) Process.Start(fullPath);
                }
            }
        }

        private void Amount_MouseWheel(object sender, MouseEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (decimal.TryParse(tb.Text, out decimal val))
            {
                decimal rubles = Math.Truncate(val);
                decimal kopeks = val - rubles;

                if (e.Delta > 0) rubles += 1;
                else rubles -= 1;

                if (rubles < 0) rubles = 0;

                tb.Text = (rubles + kopeks).ToString("N2");
                tb.SelectAll();
            }
        }

        private string GenerateDocNumber(DateTime selectedDate)
        {
            string datePart = selectedDate.ToString("ddMMyyyy");
            string prefix = string.Format("Р-{0}-", datePart);
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
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

        private void tvCategories_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                try
                {
                    int index = dgvItems.Rows.Add();
                    dgvItems.Rows[index].Cells["colCategory"].Value = e.Node.Text;
                    dgvItems.Rows[index].Cells["colAmount"].Value = 0.00m;
                    dgvItems.Rows[index].Cells["colCheckNumber"].Value = "";
                    dgvItems.Rows[index].Cells["colBasis"].Value = "";

                    isDirty = true;

                    dgvItems.CurrentCell = dgvItems.Rows[index].Cells["colAmount"];
                    dgvItems.BeginEdit(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count == 0 || (dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow))
            {
                MessageBox.Show("Таблица пуста!"); return;
            }

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
                        string sqlDoc = "INSERT INTO expense_docs (doc_number, date, total) VALUES (@n, @d, @t);";
                        var cmdDoc = new SQLiteCommand(sqlDoc, conn, trans);
                        cmdDoc.Parameters.AddWithValue("@n", txtDocNumber.Text);
                        cmdDoc.Parameters.AddWithValue("@d", dtpDocDate.Value.ToString("yyyy-MM-dd"));
                        cmdDoc.Parameters.AddWithValue("@t", (double)currentTotalSum);
                        cmdDoc.ExecuteNonQuery();

                        long docId = conn.LastInsertRowId;

                        foreach (DataGridViewRow row in dgvItems.Rows)
                        {
                            if (row.IsNewRow) continue;

                            var cmdItem = new SQLiteCommand(@"
                                INSERT INTO expense_items 
                                (doc_id, category, amount, description, check_number, basis) 
                                VALUES (@id, @c, @a, @desc, @check, @basis)", conn, trans);

                            cmdItem.Parameters.AddWithValue("@id", docId);
                            cmdItem.Parameters.AddWithValue("@c", row.Cells["colCategory"].Value);
                            decimal amt = 0;
                            decimal.TryParse(row.Cells["colAmount"].Value?.ToString(), out amt);
                            cmdItem.Parameters.AddWithValue("@a", (double)amt);
                            cmdItem.Parameters.AddWithValue("@desc", DBNull.Value);
                            cmdItem.Parameters.AddWithValue("@check", row.Cells["colCheckNumber"].Value?.ToString() ?? "");
                            cmdItem.Parameters.AddWithValue("@basis", row.Cells["colBasis"].Value?.ToString() ?? "");

                            cmdItem.ExecuteNonQuery();
                        }

                        // 3. Создание РКО
                        string rkoNumber = _service.GetNextCashOrderNumber("РКО", connectionString);

                        // ✅ СОБИРАЕМ ОСНОВАНИЯ И НОМЕРА ЧЕКОВ ИЗ ВСЕХ СТРОК
                        List<string> basisList = new List<string>();
                        List<string> checkList = new List<string>();

                        foreach (DataGridViewRow row in dgvItems.Rows)
                        {
                            if (row.IsNewRow) continue;

                            // Собираем основания
                            string basis = row.Cells["colBasis"].Value?.ToString();
                            if (!string.IsNullOrEmpty(basis) && !basisList.Contains(basis))
                            {
                                basisList.Add(basis);
                            }

                            // Собираем номера чеков
                            string checkNum = row.Cells["colCheckNumber"].Value?.ToString();
                            if (!string.IsNullOrEmpty(checkNum) && !checkList.Contains(checkNum))
                            {
                                checkList.Add(checkNum);
                            }
                        }

                        // Объединяем в строки через "; "
                        string allBasis = string.Join("; ", basisList);
                        string allChecks = string.Join("; ", checkList);

                        // Если ничего не введено — используем дефолтное значение
                        if (string.IsNullOrEmpty(allBasis))
                        {
                            allBasis = "Расход по документу №" + txtDocNumber.Text;
                        }

                        string sqlRKO = @"INSERT INTO cash_orders 
        (order_type, order_number, doc_ref_id, date, amount, base, appendix, person_id, person_name_manual) 
        VALUES ('РКО', @rNum, @refId, @date, @amt, @base, @app, @pId, @pManual)";

                        using (var cmdRko = new SQLiteCommand(sqlRKO, conn, trans))
                        {
                            cmdRko.Parameters.AddWithValue("@rNum", rkoNumber);
                            cmdRko.Parameters.AddWithValue("@refId", docId);
                            cmdRko.Parameters.AddWithValue("@date", dtpDocDate.Value.ToString("yyyy-MM-dd"));
                            cmdRko.Parameters.AddWithValue("@amt", (double)currentTotalSum);

                            // ✅ ОСНОВАНИЕ из поля "Основание" (colBasis)
                            cmdRko.Parameters.AddWithValue("@base", allBasis);

                            // ✅ ПРИЛОЖЕНИЕ — номера чеков (colCheckNumber)
                            cmdRko.Parameters.AddWithValue("@app", string.IsNullOrEmpty(allChecks) ? DBNull.Value : (object)allChecks);

                            cmdRko.Parameters.AddWithValue("@pId", DBNull.Value);
                            cmdRko.Parameters.AddWithValue("@pManual", DBNull.Value);

                            cmdRko.ExecuteNonQuery();
                        }

                        trans.Commit();
                        isDirty = false;
                        MessageBox.Show($"Документ №{txtDocNumber.Text} и РКО №{rkoNumber} сохранены!");

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
            txtDocNumber.Text = _service.GetNextDocNumber(dtpDocDate.Value, "Р");
            isDirty = false;
            UpdateTotal();
            pbReceipt.Image = null;
        }

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