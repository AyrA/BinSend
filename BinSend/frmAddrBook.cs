using System;
using System.Linq;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmAddrBook : Form
    {
        private ApiConfig A;

        public string SelectedAddress { get; private set; }

        public frmAddrBook(ApiConfig C)
        {
            SelectedAddress = null;
            A = C;
            InitializeComponent();
            FillList();
        }

        private void FillList()
        {
            var AddrList = Tools.GetRPC(A).listAddressBookEntries().FromJson<BitmessageAddrInfoContainer>();
            lvAddresses.Items.Clear();
            if (AddrList.addresses != null)
            {
                foreach (var Addr in AddrList.addresses.OrderBy(m => m.label))
                {
                    var I = lvAddresses.Items.Add(Addr.label.B64().UTF());
                    I.SubItems.Add(Addr.address);
                }
            }
            lvAddresses.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbLabel.Text))
            {
                if (!string.IsNullOrWhiteSpace(tbAddress.Text))
                {
                    var Addr = Tools.GetBmAddr(tbAddress.Text);
                    if (!string.IsNullOrWhiteSpace(Addr))
                    {
                        var RPC = Tools.GetRPC(A);
                        RPC.deleteAddressBookEntry(Addr);
                        RPC.addAddressBookEntry(Addr, tbLabel.Text.UTF().B64());
                        FillList();
                    }
                    else
                    {
                        MessageBox.Show("This is not a bitmessage address", "Address Generator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Please specify an address", "Address Generator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Please specify a label", "Address Generator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lvAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAddresses.SelectedItems.Count == 1)
            {
                tbLabel.Text = lvAddresses.SelectedItems[0].Text;
                tbAddress.Text = lvAddresses.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void lvAddresses_DoubleClick(object sender, EventArgs e)
        {
            if (lvAddresses.SelectedItems.Count == 1)
            {
                SelectedAddress = lvAddresses.SelectedItems[0].SubItems[1].Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
