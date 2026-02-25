using System;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class IncomeCatEditForm : Form
    {
        private int? _id = null;

        // 1. Базовый конструктор (вызывается всегда первым)
        public IncomeCatEditForm()
        {
            InitializeComponent();

            // Вызываем настройку иконок сразу здесь
            SetupButtons();
        }

        private void SetupButtons()
        {
            // Масштабируем и устанавливаем иконку слева от текста
            if (btnSave.Image != null)
            {
                btnSave.Image = ImageHelper.Resize(btnSave.Image, 24);
                btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                btnSave.TextImageRelation = TextImageRelation.ImageBeforeText;
            }

            if (btnCancel.Image != null)
            {
                btnCancel.Image = ImageHelper.Resize(btnCancel.Image, 24);
                btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                btnCancel.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
        }

        // 2. Конструктор для ДОБАВЛЕНИЯ (передаем только имя родителя)
        public IncomeCatEditForm(string parentName) : this()
        {
            this.Text = "Добавить категорию";
            // Устанавливаем стиль, позволяющий свободный ввод текста
            txtParent.DropDownStyle = ComboBoxStyle.DropDown;
            txtParent.Text = parentName;
        }

        // 3. Конструктор для РЕДАКТИРОВАНИЯ (передаем всё)
        public IncomeCatEditForm(int id, string name, string parentName) : this()
        {
            _id = id;
            this.Text = "Редактировать категорию";

            txtName.Text = name;

            // Повторяем ту же логику для отображения родителя
            txtParent.Items.Clear();
            txtParent.Items.Add(parentName);
            txtParent.SelectedIndex = 0;
        }

        // Свойства для получения данных в главной форме
        public string CategoryName => txtName.Text.Trim();

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CategoryName))
            {
                MessageBox.Show("Введите наименование категории!", "Предупреждение",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void IncomeCatEditForm_Load(object sender, EventArgs e)
        {
            // Настройка графики
            // Масштабируем иконки (если ImageHelper уже создан)
            if (btnSave.Image != null) btnSave.Image = ImageHelper.Resize(btnSave.Image, 24);
            if (btnCancel.Image != null) btnCancel.Image = ImageHelper.Resize(btnCancel.Image, 24);

            btnSave.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCancel.TextImageRelation = TextImageRelation.ImageBeforeText;
        }
    }
}