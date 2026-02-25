using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class IDDocsDirForm : Form
    {
        private IDDocsService _service = new IDDocsService();

        public IDDocsDirForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(IDDocsDirForm_Load);
        }

        private void IDDocsDirForm_Load(object sender, EventArgs e)
        {
            LoadData();
            SetupButtons();
        }

        private void SetupButtons()
        {
            Button[] buttons = { btnAdd, btnEdit, btnDelete, btnClose };
            foreach (var btn in buttons)
            {
                if (btn != null && btn.Image != null)
                {
                    btn.Image = ImageHelper.Resize(btn.Image, 24);
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleRight;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(5, 0, 5, 0);
                }
            }
        }

        private void LoadData()
        {
            try
            {
                dgvData.DataSource = _service.GetAllDocs();

                // Скрываем служебные ID, чтобы пользователь их не видел
                if (dgvData.Columns["Id"] != null) dgvData.Columns["Id"].Visible = false;
                if (dgvData.Columns["employee_id"] != null) dgvData.Columns["employee_id"].Visible = false;
                if (dgvData.Columns["type_id_doc"] != null) dgvData.Columns["type_id_doc"].Visible = false;

                // Настраиваем заголовок и внешний вид
                if (dgvData.Columns["EmployeeName"] != null) dgvData.Columns["EmployeeName"].HeaderText = "Сотрудник";
                if (dgvData.Columns["DocTypeName"] != null) dgvData.Columns["DocTypeName"].HeaderText = "Тип документа";

                dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvData.AllowUserToAddRows = false;
                dgvData.RowHeadersVisible = false;
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки: " + ex.Message); }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var f = new IDDocsEditForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _service.SaveDoc(null, f.SelectedEmployeeId, f.SelectedTypeId, f.Series, f.Number, f.IssuedBy, f.IssueDate);
                    LoadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                // Извлекаем данные (с проверкой на null для Series)
                int id = Convert.ToInt32(dgvData.CurrentRow.Cells["Id"].Value);
                int empId = Convert.ToInt32(dgvData.CurrentRow.Cells["employee_id"].Value);
                int typeId = Convert.ToInt32(dgvData.CurrentRow.Cells["type_id_doc"].Value);

                string ser = dgvData.CurrentRow.Cells["Series"].Value != DBNull.Value
                             ? dgvData.CurrentRow.Cells["Series"].Value.ToString() : "";

                string num = dgvData.CurrentRow.Cells["Number"].Value.ToString();
                string issued = dgvData.CurrentRow.Cells["IssuedBy"].Value.ToString();
                DateTime date = Convert.ToDateTime(dgvData.CurrentRow.Cells["IssueDate"].Value);

                using (var f = new IDDocsEditForm(id, empId, typeId, ser, num, issued, date))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        _service.SaveDoc(id, f.SelectedEmployeeId, f.SelectedTypeId, f.Series, f.Number, f.IssuedBy, f.IssueDate);
                        LoadData();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvData.CurrentRow.Cells["Id"].Value);
                string empName = dgvData.CurrentRow.Cells["EmployeeName"].Value.ToString();

                if (MessageBox.Show("Удалить документ для " + empName + "?", "Удаление",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _service.DeleteDoc(id);
                    LoadData();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}