namespace ChurchBudget.Forms
{
    public partial class IncomeListForm : BaseDocListForm
    {
        public IncomeListForm()
        {
            InitializeComponent();
            _tableName = "income_docs";
            _itemsTable = "income_items";
            _docPrefix = "Д";
            this.Text = "Реестр доходов";
        }
    }
}
