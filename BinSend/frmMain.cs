using BinSend.Properties;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmMain : Form
    {
        private BitmessageRPC RPC;
        private Config C;
        private int SelectedTemplate = 0;

        public frmMain(Config Configuration)
        {
            InitializeComponent();
            C = Configuration;
            RPC = Tools.GetRPC(C.ApiSettings);
            LoadAddresses();
            if (C.Templates == null || C.Templates.Length == 0)
            {
                C.Templates = Template.GetDefaults();
                C.Save(Program.AppPath);
            }
            SelectTemplate(0);
        }

        private void LoadAddresses()
        {
            SuspendLayout();
            var selected = cbFromAddr.SelectedItem == null ? "" : Tools.GetBmAddr(cbFromAddr.SelectedItem.ToString());

            cbToAddr.Items.Clear();
            cbFromAddr.Items.Clear();

            //Single address for send
            cbFromAddr.Items.Add("Random Single <BM-SRND>");
            //Invividual part address
            cbFromAddr.Items.Add("Random Multi <BM-MRND>");

            var Contacts = RPC.listAddressBookEntries().FromJson<BitmessageAddrInfoContainer>();
            if (Contacts.addresses != null)
            {
                foreach (var A in Contacts.addresses)
                {
                    cbToAddr.Items.Add($"{A.address} [{A.label.B64().UTF()}]");
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
                        cbToAddr.Items.Add($"{A.address} [{A.label}]");
                    }
                    cbFromAddr.Items.Add($"{A.label} <{A.address}>");
                    //Reselect our address
                    if (A.address == selected)
                    {
                        cbFromAddr.SelectedIndex = cbFromAddr.Items.Count - 1;
                    }
                }
            }
            if (cbFromAddr.SelectedIndex < 0)
            {
                cbFromAddr.SelectedIndex = 0;
            }
            ResumeLayout();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = OFD.FileName;
            }
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            using (var F = new frmTemplate(C.Templates, SelectedTemplate))
            {
                if (F.ShowDialog() == DialogResult.OK)
                {
                    C.Templates = F.Templates;
                    if (C.Templates == null || C.Templates.Length == 0)
                    {
                        C.Templates = Template.GetDefaults();
                    }
                    C.Save(Program.AppPath);
                }
                SelectTemplate(Math.Max(0, F.SelectedIndex));
            }
        }

        private void SelectTemplate(int Sel)
        {
            SelectedTemplate = Sel;
            var Selected = C.Templates[Sel];
            btnTemplate.Text = $"&Template: {Selected.Name}";
            tbBody.Text = Selected.Content;

        }

        private void btnManageAdd_Click(object sender, EventArgs e)
        {
            using (var F = new frmAddr(C.ApiSettings))
            {
                F.ShowDialog();
                LoadAddresses();
            }
        }

        private void btnAddressBook_Click(object sender, EventArgs e)
        {
            using (var F = new frmAddrBook(C.ApiSettings))
            {
                if (F.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(F.SelectedAddress))
                {
                    cbToAddr.Text = F.SelectedAddress;
                }
                LoadAddresses();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Tools.ShowHelp(Resources.HELP_Main);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (Tools.IsValidFromAddr(cbFromAddr.SelectedItem.ToString()))
            {
                if (cbToAddr.Text == string.Empty || Tools.GetBmAddr(cbToAddr.Text) != null)
                {
                    if (File.Exists(tbFile.Text))
                    {
                        FileStream FS;
                        try
                        {
                            FS = File.OpenRead(tbFile.Text);
                        }
                        catch (Exception ex)
                        {
                            Tools.Log("Unable to open file. Error: " + ex.Message);
                            return;
                        }
                        using (FS)
                        {
                            if (SelectedTemplate >= 0 || SelectedTemplate < C.Templates.Length)
                            {
                                using (var F = new frmSend(FS, tbFile.Text, C.ApiSettings, C.Templates[SelectedTemplate], cbFromAddr.SelectedItem.ToString(), cbToAddr.Text, tbSubject.Text, tbBody.Text, (int)nudChunk.Value, (int)nudTTL.Value))
                                {
                                    Hide();
                                    F.ShowDialog();
                                    Show();
                                    BringToFront();
                                    Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("The selected template does not exist", "Template check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("The file does not exist", "File check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid \"To\" Address", "Address check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid \"From\" Address", "Address check", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
