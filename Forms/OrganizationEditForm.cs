using System;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class OrganizationEditForm : Form
    {
        private int? _id = null;

        // Конструктор для добавления новой записи
        public OrganizationEditForm()
        {
            InitializeComponent();

            this.Text = "Добавить организацию";
        }

        // Конструктор для редактирования существующей записи
        public OrganizationEditForm(int id, string name, string loc, string dean, string dioc) : this()
        {
            _id = id;
            this.Text = "Редактировать организацию";

            // Заполняем поля данными из таблицы
            txtName.Text = name;
            txtLocation.Text = loc;
            txtDeanery.Text = dean;
            txtDiocese.Text = dioc;
        }

        // Свойства ДОЛЖНЫ быть внутри класса!
        public string ChurchName => txtName.Text.Trim();
        public new string Location => txtLocation.Text.Trim();
        public string Deanery => txtDeanery.Text.Trim();
        public string Diocese => txtDiocese.Text.Trim();

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Простейшая проверка на пустое название
            if (string.IsNullOrEmpty(ChurchName))
            {
                MessageBox.Show("Введите название организации!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void OrganizationEditForm_Load(object sender, EventArgs e)
        {
            // Масштабируем иконки (если ImageHelper уже создан)
            if (btnSave.Image != null) btnSave.Image = ImageHelper.Resize(btnSave.Image, 24);
            if (btnCancel.Image != null) btnCancel.Image = ImageHelper.Resize(btnCancel.Image, 24);

            btnSave.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCancel.TextImageRelation = TextImageRelation.ImageBeforeText;
        }
    }
}