using System;
using System.Linq;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmAddr : Form
    {
        ApiConfig A;
        public frmAddr(ApiConfig Conf)
        {
            A = Conf;
            InitializeComponent();
            FillAddr();
        }

        private void EnableAll(bool E)
        {
            foreach (var C in Controls.OfType<Button>())
            {
                C.Enabled = E;
            }
        }

        private void FillAddr()
        {
            lbAddr.Items.Clear();
            var AddrList = Tools.GetRPC(A).listAddresses().FromJson<BitmessageAddrInfoContainer>();
            if (AddrList.addresses != null)
            {
                foreach (var Addr in AddrList.addresses)
                {
                    lbAddr.Items.Add($"{Addr.address}\t{Addr.label}");
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lbAddr.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Delete the selected addresses?", "Delete Addresses", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    EnableAll(false);
                    var AllItems = lbAddr.SelectedItems.OfType<string>().ToArray();
                    var AllStatuses = new string[AllItems.Length];
                    var RPC = Tools.GetRPC(A);
                    for (var i = 0; i < AllItems.Length; i++)
                    {
                        var Addr = Tools.GetBmAddr(AllItems[i]);
                        AllStatuses[i] = string.Format("{0}: {1}", Addr, RPC.deleteAddress(Addr));
                        lbAddr.Items.Remove(AllItems[i]);
                        Application.DoEvents();
                    }
                    MessageBox.Show(string.Join(Environment.NewLine, AllStatuses), "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    EnableAll(true);
                    FillAddr();
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Enabled = false;
            string Addr = null;
            var RPC = Tools.GetRPC(A);

            if (cbRandom.Checked)
            {
                if (!string.IsNullOrWhiteSpace(tbLabel.Text))
                {
                    Addr = RPC.createRandomAddress(tbLabel.Text.UTF().B64(), cbShortAddr.Checked);
                }
                else
                {
                    MessageBox.Show("Please specify a label", "Address Generator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (!string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                if (cbDML.Checked)
                {
                    Addr = RPC.createChan(tbPassword.Text.UTF().B64());
                }
                else
                {
                    Addr = RPC.createDeterministicAddresses(tbPassword.Text.UTF().B64(), eighteenByteRipe: cbShortAddr.Checked);
                    try
                    {
                        Addr = Addr.FromJson<BitmessageAddrListContainer>().addresses[0];
                    }
                    catch
                    {
                        Addr = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please specify a passphrase", "Address Generator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Enabled = true;
            if (!string.IsNullOrWhiteSpace(Addr))
            {
                FillAddr();
                MessageBox.Show($"Address generated: {Addr}", "Address Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbShortAddr_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShortAddr.Checked)
            {
                cbDML.Checked = false;
                cbDML.Enabled = false;
            }
            else
            {
                cbDML.Enabled = cbDeterministic.Checked;
            }
        }

        private void cbRandom_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRandom.Checked)
            {
                tbPassword.Enabled = false;
                cbDML.Enabled = false;
                tbLabel.Enabled = true;
            }
            else
            {
                cbDML.Enabled = !cbShortAddr.Checked;
                tbPassword.Enabled = true;
                tbLabel.Enabled = false;
            }
        }
    }
}
