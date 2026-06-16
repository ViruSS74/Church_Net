using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class TypesOfDocsEditForm : Form
    {
        private int? _id = null;

        public string DocName { get { return txtName.Text; } }

        // Конструктор №1: Для добавления
        public TypesOfDocsEditForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(TypesOfDocsEditForm_Load);
        }

        // Конструктор №2: Для редактирования
        public TypesOfDocsEditForm(int id, string currentName)
        {
            InitializeComponent();
            _id = id;
            this.Load += new EventHandler(TypesOfDocsEditForm_Load);

            // Заполняем поле данными
            if (txtName != null) txtName.Text = currentName;
        }

        private void TypesOfDocsEditForm_Load(object sender, EventArgs e)
        {
            this.Text = _id == null ? "Добавление" : "Редактирование";
            ImageHelper.ApplyToButtons(this, 24);
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Введите название!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}