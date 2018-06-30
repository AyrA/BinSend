using BinSend.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmTemplate : Form
    {
        public int SelectedIndex;
        private int InitialSelected;
        public Template[] Templates;
        private List<Template> WorkingTemplates;

        public frmTemplate(Template[] Templates, int Selected)
        {
            InitialSelected = Selected;
            WorkingTemplates = new List<Template>(this.Templates = Templates);
            InitializeComponent();
            foreach (var E in Enum.GetValues(typeof(EncodingType)))
            {
                cbEncoding.Items.Add(E);
            }
            SetTemplateList(Selected);
            DialogResult = DialogResult.Cancel;
        }

        private void SetTemplateList(int Select = -1)
        {
            cbEncoding.SelectedIndex = 0;
            lbTemplate.Items.Clear();
            lbTemplate.Items.AddRange(WorkingTemplates.Select(m => (object)m.Name).ToArray());
            if (Select > -1 && lbTemplate.Items.Count > 0)
            {
                lbTemplate.SelectedIndex = Math.Max(Math.Min(Select, lbTemplate.Items.Count - 1), 0);
            }
            else
            {
                tbBody.Text = "";
            }
            SelectedIndex = Select;
        }

        private void MoveUp()
        {
            int Sel = lbTemplate.SelectedIndex;
            if (Sel > 0)
            {
                Swap(Sel, Sel - 1);
            }
        }

        private void MoveDown()
        {
            int Sel = lbTemplate.SelectedIndex;
            if (Sel < lbTemplate.Items.Count - 1)
            {
                Swap(Sel, Sel + 1);
            }
        }

        private void Swap(int A, int B)
        {
            var T = WorkingTemplates[A];
            WorkingTemplates[A] = WorkingTemplates[B];
            WorkingTemplates[B] = T;
            SetTemplateList(B);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            MoveUp();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            MoveDown();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int Sel = lbTemplate.SelectedIndex;
            if (Sel >= 0)
            {
                WorkingTemplates.RemoveAt(Sel);
                SetTemplateList(Sel);
                if (WorkingTemplates.Count == 0)
                {
                    MessageBox.Show("You removed all Templates. Saving now will restore the defaults.", "No Templates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var S = Tools.Prompt("Please enter the new Template name", "Template name", "new");
            if (S != null)
            {
                if (WorkingTemplates.Any(m => m.Name.ToLower() == S.ToLower()))
                {
                    MessageBox.Show("This Template name already exists", "New Template", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    WorkingTemplates.Add(new Template()
                    {
                        Name = S,
                        Content = "",
                        Encoding = EncodingType.Base64
                    });
                }
                SetTemplateList(WorkingTemplates.Count - 1);
            }
        }

        private void lbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Sel = lbTemplate.SelectedIndex;
            if (Sel >= 0)
            {
                SelectedIndex = Sel;
                tbBody.Text = WorkingTemplates[Sel].Content;
                cbEncoding.SelectedItem = WorkingTemplates[Sel].Encoding;
            }
        }

        private void tbBody_TextChanged(object sender, EventArgs e)
        {
            var Sel = lbTemplate.SelectedIndex;
            if (Sel > -1 && WorkingTemplates.Count > Sel)
            {
                var T = WorkingTemplates[Sel];
                T.Content = tbBody.Text;
                WorkingTemplates[Sel] = T;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTemplates();
            MessageBox.Show("Your changes have been saved", "Changes saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveTemplates()
        {
            Templates = WorkingTemplates.ToArray();
        }

        private void cbEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            var SelE = cbEncoding.SelectedIndex;
            var SelT = lbTemplate.SelectedIndex;
            if (SelE > -1 && SelT > -1 && WorkingTemplates.Count > SelT)
            {
                var T = WorkingTemplates[SelT];
                T.Encoding = (EncodingType)cbEncoding.SelectedItem;
                WorkingTemplates[SelT] = T;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Templates.SequenceEqual(WorkingTemplates))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                switch (MessageBox.Show("There are unsaved changes. Save changes before closing?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                {
                    case DialogResult.Yes:
                        SaveTemplates();
                        DialogResult = DialogResult.OK;
                        Close();
                        break;
                    case DialogResult.No:
                        SelectedIndex = InitialSelected;
                        Close();
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Tools.ShowHelp(Resources.HELP_Template);
        }

        private void frmTemplate_FormClosed(object sender, FormClosedEventArgs e)
        {
            Tools.CloseHelp();
        }
    }
}
