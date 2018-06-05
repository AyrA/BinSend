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
    public partial class frmTemplate : Form
    {
        public int SelectedIndex;
        public Template[] Templates;
        private List<Template> WorkingTemplates;

        public frmTemplate(Template[] Templates, int Selected)
        {
            WorkingTemplates = new List<Template>(this.Templates = Templates);
            InitializeComponent();
            SetTemplateList(Selected);
        }

        private void SetTemplateList(int Select = -1)
        {
            cbEncoding.Items.Clear();
            cbEncoding.Items.AddRange(WorkingTemplates.Select(m => (object)m.Name).ToArray());
            if (Select > -1 && cbEncoding.Items.Count > 0)
            {
                cbEncoding.SelectedIndex = Math.Max(Math.Min(Select, cbEncoding.Items.Count - 1), 0);
            }
            SelectedIndex = Select;
        }

        private void MoveUp()
        {
            int Sel = cbEncoding.SelectedIndex;
            if (Sel > 0)
            {
                Swap(Sel, Sel - 1);
            }
        }

        private void MoveDown()
        {
            int Sel = cbEncoding.SelectedIndex;
            if (Sel < cbEncoding.Items.Count - 1)
            {
                Swap(Sel, Sel + 1);
            }
        }

        private void Swap(int A, int B)
        {
            var T = WorkingTemplates[A];
            WorkingTemplates[A] = WorkingTemplates[B];
            WorkingTemplates[B] = T;
            SetTemplateList(A);
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
            int Sel = cbEncoding.SelectedIndex;
            if (Sel >= 0)
            {
                WorkingTemplates.RemoveAt(Sel);
            }
            SetTemplateList(Sel);
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
            }
            SetTemplateList(WorkingTemplates.Count - 1);
        }

        private void lbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Sel = lbTemplate.SelectedIndex;
            if (Sel >= 0)
            {
                tbBody.Text = WorkingTemplates[Sel].Content;
                cbEncoding.SelectedIndex = (int)WorkingTemplates[Sel].Encoding;
            }
        }

        private void tbBody_TextChanged(object sender, EventArgs e)
        {
            var Sel = cbEncoding.SelectedIndex;
            if (Sel > -1)
            {
                var T = WorkingTemplates[Sel];
                T.Content = tbBody.Text;
                WorkingTemplates[Sel] = T;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Templates = WorkingTemplates.ToArray();
            Close();
        }
    }
}
