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

            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string dbPath = Path.Combine(dataFolder, "church.db");
            _docsService = new ListOfDocsService(dbPath);

            SetupGrid();

            cmbTypeOfDocs.Items.AddRange(new string[] { "Все", "Доходы", "Расходы", "ПКО", "РКО" });
            cmbTypeOfDocs.SelectedIndex = 0;

            dtpStart.ValueChanged += (s, e) => RefreshData();
            dtpEnd.ValueChanged += (s, e) => RefreshData();
            cmbTypeOfDocs.SelectedIndexChanged += (s, e) => RefreshData();

            dgvDocs.DataBindingComplete += dgvDocs_DataBindingComplete;
            dgvDocs.CellFormatting += dgvDocs_CellFormatting;

            ImageHelper.ApplyToButtons(this, 24);

            btnClose.Click += btnClose_Click;
            // 👇 ДОБАВЛЕНО: привязка кнопки удаления
            btnDelete.Click += btnDelete_Click;

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

            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "№ п/п", Name = "ColIndex", Width = 50, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Дата", DataPropertyName = "date", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" } });
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Наименование документа", DataPropertyName = "doc_number", Name = "doc_number", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Тип", DataPropertyName = "doc_type", Name = "colType", Width = 100 });

            // Скрытый столбец для ID, нужен для удаления
            dgvDocs.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "id", Visible = false });
        }

        private void RefreshData()
        {
            try
            {
                dtDocuments = _docsService.GetAllDocuments(
                    dtpStart.Value,
                    dtpEnd.Value,
                    cmbTypeOfDocs.SelectedItem.ToString());

                dvDocuments = new DataView(dtDocuments);
                dgvDocs.DataSource = dvDocuments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDocs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateRowNumbers();
        }

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
                bool isPositive = val == "Income" || val == "ПКО" || val == "Доход" || val == "Доходы";

                e.Value = isPositive ? "+" : "-";
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

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (dgvDocs.CurrentRow == null) return;

            DataRowView rowView = (DataRowView)dgvDocs.CurrentRow.DataBoundItem;
            int docId = Convert.ToInt32(rowView["id"]);
            string docType = rowView["doc_type"].ToString();

            switch (docType)
            {
                case "Income":
                    DocPreviewForm previewIn = new DocPreviewForm(docId, "Income", this._docsService);
                    previewIn.ShowDialog();
                    break;
                case "Expense":
                    DocPreviewForm previewOut = new DocPreviewForm(docId, "Expense", this._docsService);
                    previewOut.ShowDialog();
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
            RefreshData(); // Обновляем список после закрытия формы (вдруг данные изменились)
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDocs.CurrentRow == null)
            {
                MessageBox.Show("Выберите документ для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView rowView = (DataRowView)dgvDocs.CurrentRow.DataBoundItem;
            int docId = Convert.ToInt32(rowView["id"]);
            string docType = rowView["doc_type"].ToString();
            string docNumber = rowView["doc_number"].ToString();

            DialogResult res = MessageBox.Show(
                string.Format("Вы уверены, что хотите безвозвратно удалить документ \"{0}\"?\nЭто действие нельзя отменить.", docNumber),
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                try
                {
                    _docsService.DeleteDocument(docId, docType);
                    MessageBox.Show("Документ успешно удалён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}