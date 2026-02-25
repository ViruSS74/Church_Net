using System;
using System.Drawing; // Добавлено для работы с Image
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class EmployeeEditForm : Form
    {
        private int? _employeeId = null;

        public EmployeeEditForm()
        {
            InitializeComponent();
            // Подписываемся на событие загрузки вручную, если оно не привязано в дизайнере
            this.Load += EmployeeEditForm_Load;
            this.Text = "Добавление сотрудника";
        }

        public EmployeeEditForm(int id, string last, string first, string middle, string role) : this()
        {
            _employeeId = id;
            this.Text = "Редактирование сотрудника";

            txtLastName.Text = last;
            txtFirstName.Text = first;
            txtMiddleName.Text = middle;
            txtRole.Text = role;
        }

        public string LastName => txtLastName.Text.Trim();
        public string FirstName => txtFirstName.Text.Trim();
        public string MiddleName => txtMiddleName.Text.Trim();
        public string Role => txtRole.Text.Trim();

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Старый добрый способ для .NET 3.5
            if (string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(FirstName))
            {
                MessageBox.Show("Заполните Фамилию и Имя!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void EmployeeEditForm_Load(object sender, EventArgs e)
        {
            // Уменьшаем иконки (проверьте, что класс ImageHelper создан в проекте)
            if (btnSave.Image != null)
                btnSave.Image = ImageHelper.Resize(btnSave.Image, 24);

            if (btnCancel.Image != null)
                btnCancel.Image = ImageHelper.Resize(btnCancel.Image, 24);

            btnSave.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCancel.TextImageRelation = TextImageRelation.ImageBeforeText;

            // Немного отступа для иконки от края
            btnSave.TextAlign = ContentAlignment.MiddleRight;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancel.TextAlign = ContentAlignment.MiddleRight;
            btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
        }
    }
}