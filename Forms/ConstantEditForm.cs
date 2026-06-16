using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class ConstantEditForm : Form
    {
        private string _connectionString;
        private int _constantId; // 0 = новая запись, >0 = редактирование

        public ConstantEditForm(string connectionString, int id)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _constantId = id;

            // 👇 ДОБАВЬТЕ ЭТУ СТРОКУ:
            this.Load += ConstantEditForm_Load;
        }

        private void ConstantEditForm_Load(object sender, EventArgs e)
        {
            if (_constantId > 0)
            {
                this.Text = "Редактирование константы";
                LoadData();  // ← Обязательно вызовите этот метод!
            }
            else
            {
                this.Text = "Добавление константы";
            }
        }

        private void LoadData()
        {
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT key, value FROM constants WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _constantId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtKey.Text = reader["key"].ToString();
                                txtValue.Text = reader["value"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string key = txtKey.Text.Trim();
            string value = txtValue.Text.Trim();

            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("Введите ключ константы.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKey.Focus();
                return;
            }

            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    if (_constantId > 0)
                    {
                        string sql = "UPDATE constants SET key = @key, value = @value WHERE id = @id";
                        using (var cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@key", key);
                            cmd.Parameters.AddWithValue("@value", value);
                            cmd.Parameters.AddWithValue("@id", _constantId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Проверка на дубликат ключа
                        string checkSql = "SELECT COUNT(*) FROM constants WHERE key = @key";
                        using (var cmd = new SQLiteCommand(checkSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@key", key);
                            long count = Convert.ToInt64(cmd.ExecuteScalar());
                            if (count > 0)
                            {
                                MessageBox.Show(string.Format("Константа с ключом \"{0}\" уже существует.", key), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        string insertSql = "INSERT INTO constants (key, value) VALUES (@key, @value)";
                        using (var cmd = new SQLiteCommand(insertSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@key", key);
                            cmd.Parameters.AddWithValue("@value", value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}