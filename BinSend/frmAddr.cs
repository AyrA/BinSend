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

        private void FillAddr()
        {
            cbAddr.Items.Clear();
            var AddrList = Tools.GetRPC(A).listAddresses().FromJson<BitmessageAddrInfoContainer>();
            if (AddrList.addresses != null)
            {
                foreach (var Addr in AddrList.addresses)
                {
                    cbAddr.Items.Add($"{Addr.address} [{Addr.label}]");
                }
            }
        }

        private void btnDel_Click(object sender, System.EventArgs e)
        {
            var Addr = cbAddr.SelectedItem;
            if (Addr != null)
            {
                Tools.Log("Delete Address result: " + Tools.GetRPC(A).deleteAddress(Tools.GetBmAddr(Addr.ToString())));
                FillAddr();
            }
        }

        private void btnGenerate_Click(object sender, System.EventArgs e)
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

        private void cbShortAddr_CheckedChanged(object sender, System.EventArgs e)
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

        private void cbRandom_CheckedChanged(object sender, System.EventArgs e)
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
