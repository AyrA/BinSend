using System;
using System.Linq;
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
            var selected = cbFromAddr.SelectedItem == null ? "" : Tools.GetBmAddr(cbFromAddr.SelectedItem.ToString());

            tbToAddr.AutoCompleteCustomSource.Clear();
            cbFromAddr.Items.Clear();

            var Contacts = RPC.listAddressBookEntries().FromJson<BitmessageAddrInfoContainer>();
            if (Contacts.addresses != null)
            {
                foreach (var A in Contacts.addresses)
                {
                    tbToAddr.AutoCompleteCustomSource.Add($"{A.address} [{A.label.B64().UTF()}]");
                }
            }
            var Self = RPC.listAddresses().FromJson<BitmessageAddrInfoContainer>();
            if (Self.addresses != null)
            {
                foreach (var A in Self.addresses)
                {
                    //Add own addresses to recipient list too
                    if (!Contacts.addresses.Any(m => m.address == A.address))
                    {
                        tbToAddr.AutoCompleteCustomSource.Add($"{A.address} [{A.label}]");
                    }
                    cbFromAddr.Items.Add($"{A.label} <{A.address}>");
                    //Reselect our address
                    if (A.address == selected)
                    {
                        cbFromAddr.SelectedIndex = cbFromAddr.Items.Count - 1;
                    }
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
