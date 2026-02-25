namespace ChurchBudget.Forms
{
    public partial class ExpenseListForm : BaseDocListForm
    {
        public ExpenseListForm()
        {
            InitializeComponent();
            this._tableName = "expense_docs";
            this._docPrefix = "H";
            this.Text = "Реестр расходов";
        }
    }
}
