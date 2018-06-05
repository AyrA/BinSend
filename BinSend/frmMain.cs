using System;
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
            btnTemplate.Text = $"Template: {Selected.Name}";
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
                    tbToAddr.Text = F.SelectedAddress;
                }
                LoadAddresses();
            }
        }
    }
}
