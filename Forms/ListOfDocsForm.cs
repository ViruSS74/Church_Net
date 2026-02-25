using ChurchBudget.Forms;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget
{
    public partial class ListOfDocsForm : Form
    {
        private DataTable dtDocuments;
        private DataView dvDocuments;
        private ListOfDocsService _docsService;

        public ListOfDocsForm()
        {
            InitializeComponent();

            // Настройка путей
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string dbPath = Path.Combine(dataFolder, "church.db");
            _docsService = new ListOfDocsService(dbPath);

            SetupGrid();

            // Инициализация ComboBox
            cmbTypeOfDocs.Items.AddRange(new string[] { "Все", "Доходы", "Расходы", "ПКО", "РКО" });
            cmbTypeOfDocs.SelectedIndex = 0;

            // Подписки на фильтры
            dtpStart.ValueChanged += (s, e) => RefreshData();
            dtpEnd.ValueChanged += (s, e) => RefreshData();
            cmbTypeOfDocs.SelectedIndexChanged += (s, e) => RefreshData();

            // ПОДПИСКИ НА СОБЫТИЯ
            dgvDocs.DataBindingComplete += dgvDocs_DataBindingComplete;
            dgvDocs.CellFormatting += dgvDocs_CellFormatting;

            txtSearch.TextChanged += txtSearch_TextChanged;

            btnClose.Click += btnClose_Click;

            RefreshData();
        }

        private void SetupGrid()
        {
            dgvDocs.AutoGenerateColumns = false;
            dgvDocs.AllowUserToAddRows = false;
            dgvDocs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocs.ReadOnly = true;
            dgvDocs.RowHeadersVisible = false;
            dgvDocs.Columns.Clear();

            // 1. № п/п (Без DataPropertyName, заполняется вручную)
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "№ п/п",
                Name = "ColIndex",
                Width = 50,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 2. Дата
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Дата",
                DataPropertyName = "date",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" }
            });

            // 3. Наименование
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Наименование документа",
                DataPropertyName = "doc_number",
                Name = "doc_number",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // 4. Тип (Столбец для плюсиков)
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Доход / Расход / ПКО / РКО",
                DataPropertyName = "doc_type",
                Name = "colType",
                Width = 180
            });
        }
        private void RefreshData()
        {
            try
            {
                // Передаем параметры фильтров в сервис
                dtDocuments = _docsService.GetAllDocuments(
                    dtpStart.Value,
                    dtpEnd.Value,
                    cmbTypeOfDocs.SelectedItem.ToString());

                dvDocuments = new DataView(dtDocuments);
                dgvDocs.DataSource = dvDocuments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        // Автоматический пересчет номеров при любом изменении данных в сетке
        private void dgvDocs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) { UpdateRowNumbers(); }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvDocs.Rows.Count; i++)
            {
                dgvDocs.Rows[i].Cells["ColIndex"].Value = (i + 1).ToString();
            }
        }

        private void dgvDocs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDocs.Columns[e.ColumnIndex].Name == "colType" && e.Value != null)
            {
                string val = e.Value.ToString();
                // Добавляем проверку на "Доходы" и "Доход" (в зависимости от того, как пишете в БД)
                bool isPositive = val == "Income" || val == "ПКО" || val == "Доход" || val == "Доходы";
                e.Value = isPositive ? "          +" : "          -";
                e.CellStyle.Font = new Font(dgvDocs.Font, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (isPositive)
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.SelectionForeColor = Color.LightGreen;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.SelectionForeColor = Color.Pink;
                }
                e.FormattingApplied = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dvDocuments == null) return;

            string filter = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(filter))
                dvDocuments.RowFilter = "";
            else
                dvDocuments.RowFilter = string.Format("doc_number LIKE '%{0}%'", filter);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (dgvDocs.CurrentRow == null) return;

            // 1. Получаем доступ к данным строки (включая скрытые колонки)
            DataRowView rowView = (DataRowView)dgvDocs.CurrentRow.DataBoundItem;

            // 2. Берем ID и Тип из запроса
            int docId = Convert.ToInt32(rowView["id"]);
            string docType = rowView["doc_type"].ToString();

            // 3. Выбираем форму для открытия
            switch (docType)
            {
                case "Income":
                case "Expense":
                    // Стандартный просмотр для обычных доходов/расходов
                    DocPreviewForm preview = new DocPreviewForm(docId, docType, this._docsService);
                    preview.ShowDialog();
                    break;

                case "ПКО":
                    OrderInForm orderIn = new OrderInForm(docId, this._docsService);
                    orderIn.ShowDialog();
                    break;
                case "РКО":
                    OrderOutForm orderOut = new OrderOutForm(docId, this._docsService);
                    orderOut.ShowDialog();
                    break;

                default:
                    MessageBox.Show("Тип документа '" + docType + "' пока не поддерживается.");
                    break;
            }
        }
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}