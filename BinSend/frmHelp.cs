using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
