using System.Windows.Forms;

namespace BinSend
{
    public partial class frmHelp : Form
    {
        public frmHelp()
        {
            InitializeComponent();
        }

        public void SetHelp(string HelpText)
        {
            if (string.IsNullOrEmpty(HelpText))
            {
                Close();
            }
            else
            {
                tbHelp.Text = HelpText;
            }
        }
    }
}
