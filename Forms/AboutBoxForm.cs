using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class AboutBoxForm : Form
    {
        public AboutBoxForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e) { this.Close(); }
    }
}
