using System;
using System.Drawing; // Для работы с оформлением
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class ViewsOfIDDocsEditForm : Form
    {
        private int? _id = null;

        // Свойство для получения текста из TextBox
        public string DocName { get { return txtName.Text; } }

        // Конструктор №1: Для добавления
        public ViewsOfIDDocsEditForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(ViewsOfIDDocsEditForm_Load);
        }

        // Конструктор №2: Для редактирования
        public ViewsOfIDDocsEditForm(int id, string currentName)
        {
            InitializeComponent();
            _id = id;
            this.Load += new EventHandler(ViewsOfIDDocsEditForm_Load);

            // Заполняем поле данными
            if (txtName != null) txtName.Text = currentName;
        }

        private void ViewsOfIDDocsEditForm_Load(object sender, EventArgs e)
        {
            // 1. Динамический заголовок
            this.Text = _id == null ? "Добавление" : "Редактирование";

            // 2. Оформление кнопок (ресайз и позиционирование)
            SetupButtons();

            // 3. Установка фокуса на текстовое поле
            txtName.Focus();
        }

        private void SetupButtons()
        {
            // Предположим, кнопки называются btnSave и btnCancel (или как в дизайнере)
            Button[] buttons = { btnSave, btnCancel };

            foreach (var btn in buttons)
            {
                if (btn != null && btn.Image != null)
                {
                    // Уменьшаем иконку 64x64 до 24x24 через ваш ImageHelper
                    btn.Image = ImageHelper.Resize(btn.Image, 24);

                    // Текст СЛЕВА от иконки (илиImageBeforeText для иконки слева)
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleRight;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;

                    // Внутренние отступы для красоты
                    btn.Padding = new Padding(10, 0, 10, 0);
                }
            }
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