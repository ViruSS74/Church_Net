using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class AboutBoxForm : Form
    {
        public AboutBoxForm()
        {
            InitializeComponent();

            ImageHelper.ApplyToButtons(this, 24);
        }

        private void button1_Click(object sender, System.EventArgs e) { this.Close(); }
    }
}
