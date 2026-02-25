using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class IDDocsEditForm : Form
    {
        private int? _id = null;
        private IDDocsService _service = new IDDocsService();

        // Свойства для получения данных из формы вовне
        public int SelectedEmployeeId { get { return Convert.ToInt32(cmbEmployee.SelectedValue); } }
        public int SelectedTypeId { get { return Convert.ToInt32(cmbDocType.SelectedValue); } }
        public string Series { get { return txtSeries.Text.Trim(); } }
        public string Number { get { return txtNumber.Text.Trim(); } }
        public string IssuedBy { get { return txtIssuedBy.Text.Trim(); } }
        public DateTime IssueDate { get { return dtpIssueDate.Value; } }

        // Конструктор для ДОБАВЛЕНИЯ
        public IDDocsEditForm()
        {
            InitializeComponent();
            InitForm();
        }

        // Конструктор для РЕДАКТИРОВАНИЯ
        public IDDocsEditForm(int id, int empId, int typeId, string series, string number, string issuedBy, DateTime date)
        {
            InitializeComponent();
            _id = id;
            InitForm();

            // Заполняем поля данными
            cmbEmployee.SelectedValue = empId;
            cmbDocType.SelectedValue = typeId;
            txtSeries.Text = series;
            txtNumber.Text = number;
            txtIssuedBy.Text = issuedBy;
            dtpIssueDate.Value = date;
        }

        private void InitForm()
        {
            this.Load += new EventHandler(IDDocsEditForm_Load);

            // Загружаем данные в выпадающие списки
            FillCombos();
        }

        private void FillCombos()
        {
            // 1. Сотрудники
            cmbEmployee.DataSource = _service.GetEmployeesForCombo();
            cmbEmployee.DisplayMember = "fio";
            cmbEmployee.ValueMember = "id";
            cmbEmployee.SelectedIndex = -1; // По умолчанию пусто

            // 2. Типы документов
            cmbDocType.DataSource = _service.GetDocTypesForCombo();
            cmbDocType.DisplayMember = "name";
            cmbDocType.ValueMember = "id";
            cmbDocType.SelectedIndex = -1;
        }

        private void IDDocsEditForm_Load(object sender, EventArgs e)
        {
            this.Text = _id == null ? "Добавление документа" : "Редактирование документа";
            SetupButtons();
        }

        private void SetupButtons()
        {
            Button[] buttons = { btnSave, btnCancel };
            foreach (var btn in buttons)
            {
                if (btn != null && btn.Image != null)
                {
                    btn.Image = ImageHelper.Resize(btn.Image, 24);
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleRight;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(10, 0, 10, 0);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Валидация
            if (cmbEmployee.SelectedValue == null)
            {
                MessageBox.Show("Выберите сотрудника!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbDocType.SelectedValue == null)
            {
                MessageBox.Show("Выберите тип документа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtNumber.Text.Trim()))
            {
                MessageBox.Show("Введите номер документа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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