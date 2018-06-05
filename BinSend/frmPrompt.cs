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
    public partial class frmPrompt : Form
    {
        public string Value { get; private set; }

        public frmPrompt(string Prompt, string Title = null, string Default = null)
        {
            InitializeComponent();
            if (Default != null)
            {
                tbInput.Text = Default;
            }
            Value = Default;
            if (Title == null)
            {
                Title = Application.ProductName;
            }
            Text = Title;
            lblText.Text = Prompt;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Value = tbInput.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Value = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
