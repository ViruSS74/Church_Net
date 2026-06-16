using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using ChurchBudget;

namespace ChurchBudget.Forms
{
    public partial class OpeningBalanceForm : Form
    {
        private string _connectionString;
        private int _orgId;
        private FinancialReportService _service;

        public OpeningBalanceForm(int orgId, string connectionString)
        {
            InitializeComponent();
            _orgId = orgId;
            _connectionString = connectionString;
            _service = new FinancialReportService(connectionString);
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void OpeningBalanceForm_Load(object sender, EventArgs e)
        {
            LoadBalancesList();
            dtpBalanceDate.Value = DateTime.Now;
            txtAmount.Text = "0.00";
            ImageHelper.ApplyToButtons(this, 24);
        }

        private void LoadBalancesList()
        {
            var balances = _service.GetOpeningBalancesList(_orgId);
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("Дата", typeof(string));
            dt.Columns.Add("Сумма", typeof(decimal));
            dt.Columns.Add("Комментарий", typeof(string));

            foreach (var item in balances)
            {
                dt.Rows.Add(
                    item["id"],
                    Convert.ToDateTime(item["date"]).ToString("dd.MM.yyyy"),
                    Convert.ToDecimal(item["amount"]),
                    item["comment"]?.ToString() ?? ""
                );
            }

            dgvBalances.DataSource = dt;
            dgvBalances.Columns["id"].Visible = false;
            dgvBalances.Columns["Дата"].Width = 100;
            dgvBalances.Columns["Сумма"].Width = 120;
            dgvBalances.Columns["Сумма"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBalances.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBalances.MultiSelect = false;
            dgvBalances.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal amount;
            if (!decimal.TryParse(txtAmount.Text, out amount) || amount < 0)
            {
                MessageBox.Show("Введите корректную сумму остатка.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return;
            }

            try
            {
                _service.SaveOpeningBalance(_orgId, dtpBalanceDate.Value, amount, txtComment.Text.Trim());
                MessageBox.Show("Остаток сохранён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadBalancesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void dgvBalances_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvBalances.Rows[e.RowIndex];
                dtpBalanceDate.Value = Convert.ToDateTime(row.Cells["Дата"].Value);
                txtAmount.Text = Convert.ToDecimal(row.Cells["Сумма"].Value).ToString("F2");
                txtComment.Text = row.Cells["Комментарий"].Value?.ToString() ?? "";
            }
        }
    }
}