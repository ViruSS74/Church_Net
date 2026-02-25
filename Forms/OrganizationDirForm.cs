using System;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class OrganizationDirForm : Form
    {
        // Сервис для работы с БД
        private ChurchService _service = new ChurchService();

        public OrganizationDirForm()
        {
            InitializeComponent();
            // Подписываемся на событие загрузки формы
            this.Load += new EventHandler(OrganizationDirForm_Load);
        }

        private void OrganizationDirForm_Load(object sender, EventArgs e)
        {
            // 1. Загружаем данные в таблицу
            LoadData();

            // 2. Настраиваем иконки кнопок через ImageHelper
            if (btnAdd.Image != null) btnAdd.Image = ImageHelper.Resize(btnAdd.Image, 24);
            if (btnEdit.Image != null) btnEdit.Image = ImageHelper.Resize(btnEdit.Image, 24);
            if (btnDelete.Image != null) btnDelete.Image = ImageHelper.Resize(btnDelete.Image, 24);
            if (btnClose.Image != null) btnClose.Image = ImageHelper.Resize(btnClose.Image, 24);

            // 3. Ставим иконку СЛЕВА от текста
            btnAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnEdit.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDelete.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnClose.TextImageRelation = TextImageRelation.ImageBeforeText;
        }

        private void LoadData()
        {
            try
            {
                dgvChurches.DataSource = null;
                dgvChurches.DataSource = _service.GetAllChurches();

                // Настройки внешнего вида таблицы
                dgvChurches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvChurches.RowHeadersVisible = false;
                dgvChurches.AllowUserToAddRows = false;
                dgvChurches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Настройка заголовков (регистр Id/Name должен совпадать с классом Church)
                if (dgvChurches.Columns["Id"] != null) dgvChurches.Columns["Id"].Visible = false;
                if (dgvChurches.Columns["Name"] != null) dgvChurches.Columns["Name"].HeaderText = "Название";
                if (dgvChurches.Columns["Location"] != null) dgvChurches.Columns["Location"].HeaderText = "Адрес";
                if (dgvChurches.Columns["Deanery"] != null) dgvChurches.Columns["Deanery"].HeaderText = "Благочиние";
                if (dgvChurches.Columns["Diocese"] != null) dgvChurches.Columns["Diocese"].HeaderText = "Епархия";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }

            // 1. Увеличиваем шрифт основного текста в таблице
            dgvChurches.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);

            // 2. Увеличиваем шрифт заголовков колонок
            dgvChurches.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            // 3. Увеличиваем высоту строк, чтобы текст не прилипал к краям
            dgvChurches.RowTemplate.Height = 30;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var f = new OrganizationEditForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                // Используем свойства из формы редактирования
                _service.SaveChurch(null, f.ChurchName, f.Location, f.Deanery, f.Diocese);
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvChurches.CurrentRow != null)
            {
                var row = dgvChurches.CurrentRow;
                int id = Convert.ToInt32(row.Cells["Id"].Value);

                var f = new OrganizationEditForm(id,
                    row.Cells["Name"].Value.ToString(),
                    row.Cells["Location"].Value.ToString(),
                    row.Cells["Deanery"].Value.ToString(),
                    row.Cells["Diocese"].Value.ToString());

                if (f.ShowDialog() == DialogResult.OK)
                {
                    _service.SaveChurch(id, f.ChurchName, f.Location, f.Deanery, f.Diocese);
                    LoadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvChurches.CurrentRow != null)
            {
                var row = dgvChurches.CurrentRow;
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                string name = row.Cells["Name"].Value?.ToString() ?? "организацию";

                if (MessageBox.Show($"Удалить '{name}'?", "Удаление",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _service.DeleteChurch(id);
                    LoadData();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}