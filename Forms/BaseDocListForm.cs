using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ChurchBudget
{
    public partial class BaseDocListForm : Form
    {
        // Поля, которые определят логику конкретной формы-наследника
        protected string _tableName;     // income_docs или expense_docs
        protected string _itemsTable;    // income_items или expenses_items
        protected string _docPrefix;     // Д или Р
        protected string _connectionString = "Data Source=church_budget.db;Version=3;";

        public BaseDocListForm()
        {
            InitializeComponent();
        }

        // Загрузка списка документов с учетом хронологии (от ранних к поздним)
        protected virtual void RefreshGrid()
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                string sql = string.Format(@"
                    SELECT id, date AS 'Дата', doc_number AS 'Номер', total AS 'Сумма' 
                    FROM {0} 
                    WHERE date BETWEEN @start AND @end 
                    ORDER BY date ASC", _tableName);

                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                // Предполагается, что на форме есть dtpStart и dtpEnd
                cmd.Parameters.AddWithValue("@start", dtpStart.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@end", dtpEnd.Value.ToString("yyyy-MM-dd"));

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvDocs.DataSource = dt;

                if (dgvDocs.Columns["id"] != null) dgvDocs.Columns["id"].Visible = false;
            }
        }

        // Логика быстрого поиска по номеру
        protected void SearchByNumber(string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber)) return;

            foreach (DataGridViewRow row in dgvDocs.Rows)
            {
                if (row.Cells["Номер"].Value.ToString().Contains(docNumber))
                {
                    row.Selected = true;
                    dgvDocs.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }
            MessageBox.Show("Документ с таким номером не найден в текущем списке.");
        }
    }
}