using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class EmployeeDirForm : Form
    {
        private readonly EmployeeService _service;

        public EmployeeDirForm()
        {
            InitializeComponent();
            _service = new EmployeeService();

            // 1. НАСТРОЙКА ТАБЛИЦЫ
            dgvEmployees.DefaultCellStyle.Font = new Font("Segoe UI", 11);
            dgvEmployees.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvEmployees.RowTemplate.Height = 30;
            dgvEmployees.ColumnHeadersVisible = true;
            dgvEmployees.ColumnHeadersHeight = 35;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 2. НАСТРОЙКА КНОПОК
            try
            {
                btnAdd.Image = new Bitmap(Properties.Resources.add, new Size(24, 24));
                btnEdit.Image = new Bitmap(Properties.Resources.edit, new Size(24, 24));
                btnDelete.Image = new Bitmap(Properties.Resources.delete, new Size(24, 24));
                btnClose.Image = new Bitmap(Properties.Resources.exit, new Size(24, 24));

                foreach (var btn in new[] { btnAdd, btnEdit, btnDelete, btnClose })
                {
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                }
            }
            catch { }

            // 3. НАСТРОЙКА ПОИСКА
            txtSearch.Text = "Поиск по фамилии...";
            txtSearch.ForeColor = Color.Gray;

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "Поиск по фамилии...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtSearch.Text) || txtSearch.Text.Trim().Length == 0)
                {
                    txtSearch.Text = "Поиск по фамилии...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            LoadData();
        }

        private void LoadData()
        {
            dgvEmployees.DataSource = _service.GetAllEmployees();

            // Настройка имен колонок (из SQL приходят оригинальные имена)
            if (dgvEmployees.Columns["last_name"] != null) dgvEmployees.Columns["last_name"].HeaderText = "Фамилия";
            if (dgvEmployees.Columns["first_name"] != null) dgvEmployees.Columns["first_name"].HeaderText = "Имя";
            if (dgvEmployees.Columns["middle_name"] != null) dgvEmployees.Columns["middle_name"].HeaderText = "Отчество";
            if (dgvEmployees.Columns["role"] != null) dgvEmployees.Columns["role"].HeaderText = "Должность";

            if (dgvEmployees.Columns["id"] != null) dgvEmployees.Columns["id"].Visible = false;
        }
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            var f = new EmployeeEditForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                _service.SaveEmployee(null, f.LastName, f.FirstName, f.MiddleName, f.Role);
                LoadData();
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null)
            {
                var row = dgvEmployees.CurrentRow;
                int id = Convert.ToInt32(row.Cells["id"].Value);

                var f = new EmployeeEditForm(id,
                    row.Cells["Фамилия"].Value.ToString(),
                    row.Cells["Имя"].Value.ToString(),
                    row.Cells["Отчество"].Value.ToString(),
                    row.Cells["Должность"].Value.ToString());

                if (f.ShowDialog() == DialogResult.OK)
                {
                    _service.SaveEmployee(id, f.LastName, f.FirstName, f.MiddleName, f.Role);
                    LoadData();
                }
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null)
            {
                var row = dgvEmployees.CurrentRow;
                int id = Convert.ToInt32(row.Cells["id"].Value);
                string name = row.Cells["Фамилия"].Value?.ToString() ?? "";

                // Спрашиваем подтверждение
                var result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {name}?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _service.DeleteEmployee(id); // Вызываем метод удаления в вашем сервисе
                    LoadData(); // Обновляем таблицу
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}