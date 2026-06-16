using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class ConstantDirForm : Form
    {
        private string _connectionString;

        public ConstantDirForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
        }

        private void ConstantDirForm_Load(object sender, EventArgs e)
        {
            LoadConstants();
            ImageHelper.ApplyToDataGridViews(this);
            ImageHelper.ApplyToButtons(this, 24);
        }

        private void LoadConstants()
        {
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT id, key AS 'Ключ', value AS 'Значение' FROM constants ORDER BY key";
                    using (var adapter = new SQLiteDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvConstants.DataSource = dt;
                    }
                }

                

                // Настройка таблицы
                dgvConstants.Columns["id"].Visible = false;
                dgvConstants.Columns["Ключ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvConstants.Columns["Значение"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvConstants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvConstants.MultiSelect = false;
                dgvConstants.ReadOnly = true;
                dgvConstants.AllowUserToAddRows = false;
                dgvConstants.AllowUserToDeleteRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки констант:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var editForm = new ConstantEditForm(_connectionString, 0))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                    LoadConstants();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvConstants.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для редактирования.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvConstants.SelectedRows[0].Cells["id"].Value);
            using (var editForm = new ConstantEditForm(_connectionString, id))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                    LoadConstants();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvConstants.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string key = dgvConstants.SelectedRows[0].Cells["Ключ"].Value.ToString();
            if (MessageBox.Show(string.Format("Удалить константу \"{0}\"?\nЭто действие нельзя отменить.", key),
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvConstants.SelectedRows[0].Cells["id"].Value);
                    using (var conn = new SQLiteConnection(_connectionString))
                    {
                        conn.Open();
                        string sql = "DELETE FROM constants WHERE id = @id";
                        using (var cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadConstants();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}