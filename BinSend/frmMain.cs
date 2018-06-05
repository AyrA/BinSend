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
            var selected = cbFromAddr.SelectedItem == null ? null : cbFromAddr.SelectedItem.ToString();

            tbToAddr.AutoCompleteCustomSource.Clear();
            cbFromAddr.Items.Clear();

            var Addr = RPC.listAddressBookEntries().FromJson<BitmessageAddrInfoContainer>();
            if (Addr.addresses != null)
            {
                foreach (var A in Addr.addresses)
                {
                    tbToAddr.AutoCompleteCustomSource.Add($"{A.label.B64().UTF()} <{A.address}>");
                }
            }
            Addr = RPC.listAddresses().FromJson<BitmessageAddrInfoContainer>();
            if (Addr.addresses != null)
            {
                foreach (var A in Addr.addresses)
                {
                    cbFromAddr.Items.Add($"{A.label} <{A.address}>");
                }
            }
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
