using System;
using System.Drawing; // Добавлено для шрифтов и выравнивания
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class TypesOfDocsDirForm : Form
    {
        private TypesOfDocsDirService _service = new TypesOfDocsDirService();

        public TypesOfDocsDirForm()
        {
            InitializeComponent();
            // Подписываем форму на событие загрузки
            this.Load += new EventHandler(TypesOfDocsDirForm_Load);
        }

        private void TypesOfDocsDirForm_Load(object sender, EventArgs e)
        {
            // 1. Загружаем данные
            LoadData();

            // 2. Настраиваем иконки кнопок (ресайз из 64х64 в 24х24)
            SetupButtons();
        }

        private void SetupButtons()
        {
            // Уменьшаем иконки и ставим их слева от текста
            Button[] buttons = { btnAdd, btnEdit, btnDelete, btnClose };

            foreach (var btn in buttons)
            {
                if (btn.Image != null)
                {
                    // Используем ваш ImageHelper для ресайза
                    btn.Image = ImageHelper.Resize(btn.Image, 24);
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleRight;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(5, 0, 5, 0); // Небольшой отступ внутри
                }
            }
        }

        private void LoadData()
        {
            try
            {
                dgvData.DataSource = _service.GetAllViews();

                // Настройка таблицы
                dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvData.RowHeadersVisible = false;
                dgvData.AllowUserToAddRows = false;

                // Стили таблицы (шрифты как в других формах)
                dgvData.DefaultCellStyle.Font = new Font("Segoe UI", 11F);
                dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                dgvData.RowTemplate.Height = 32;

                if (dgvData.Columns["Id"] != null) dgvData.Columns["Id"].Visible = false;
                if (dgvData.Columns["Name"] != null) dgvData.Columns["Name"].HeaderText = "Вид документа";
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var f = new TypesOfDocsEditForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                _service.SaveView(null, f.DocName);
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvData.CurrentRow.Cells["Id"].Value);
                string name = dgvData.CurrentRow.Cells["Name"].Value.ToString();

                var f = new TypesOfDocsEditForm(id, name);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _service.SaveView(id, f.DocName);
                    LoadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvData.CurrentRow.Cells["Id"].Value);
                if (MessageBox.Show("Удалить выбранную запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _service.DeleteView(id);
                    LoadData();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}