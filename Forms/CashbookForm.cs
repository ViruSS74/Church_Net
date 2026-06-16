using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class CashbookForm : Form
    {
        public CashbookForm()
        {
            InitializeComponent();
            ImageHelper.ApplyToButtons(this, 24);
            ImageHelper.ApplyToDataGridViews(this);
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}
