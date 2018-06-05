using System;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmMain : Form
    {
        private BitmessageRPC RPC;
        private Config C;

        public frmMain(Config Configuration)
        {
            InitializeComponent();
            C = Configuration;
            RPC = Tools.GetRPC(C.ApiSettings);
            LoadAddresses();
        }

        private void LoadAddresses()
        {
            throw new NotImplementedException();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = OFD.FileName;
            }
        }
    }
}
