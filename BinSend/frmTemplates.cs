using System;
using System.Windows.Forms;
using System.IO;

namespace BinSend
{
    public partial class frmTemplates : Form
    {
        public delegate void TemplateSelectedHandler(Template T);

        public event TemplateSelectedHandler TemplateSelected;

        public frmTemplates()
        {
            InitializeComponent();
            TemplateSelected += new TemplateSelectedHandler(frmTemplates_TemplateSelected);
            listTemplates();
        }

        private void listTemplates()
        {
            lbTemplates.Items.Clear();
            foreach (string s in Templates.AllTemplates)
            {
                lbTemplates.Items.Add(s);
            }
        }

        private void frmTemplates_TemplateSelected(Template T)
        {
            Close();
        }

        private void lbTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTemplates.SelectedIndex >= 0)
            {
                Template T = new Template(lbTemplates.SelectedItem.ToString());
                lblContent.Text = T.Text;
                lblEncoding.Text = "Format: "+T.ProposedFormat.ToString();
                lblFrom.Text = "From: " + T.From;
                lblSubject.Text = "Subject: " + T.Subject;
                lblTo.Text = "To: " + T.To;
            }
        }

        private void lbTemplates_DoubleClick(object sender, EventArgs e)
        {
            if (lbTemplates.SelectedIndex >= 0)
            {
                TemplateSelected(new Template(lbTemplates.SelectedItem.ToString()));
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lbTemplates.SelectedIndex >= 0)
            {
                if (new frmTemplateEditor(new Template(lbTemplates.SelectedItem.ToString())).ShowDialog() == DialogResult.OK)
                {
                    listTemplates();
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (new frmTemplateEditor().ShowDialog() == DialogResult.OK)
            {
                listTemplates();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbTemplates.SelectedIndex >= 0)
            {
                if (lbTemplates.SelectedItem.ToString().ToLower() == "default")
                {
                    MessageBox.Show("You cannot delete the default template but you may change it.\r\nIf you want to restore *ALL* default templates, either delete the template folder, or default.txt in it.", "Default template", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (MessageBox.Show("Delete the selected template?", "Delete template", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(string.Format(@"{0}\{1}.txt", Templates.DIR_TEMPLATE, lbTemplates.SelectedItem.ToString()));
                            listTemplates();
                        }
                        catch
                        {
                            MessageBox.Show("Cannot delete the selected template\r\nIts file might be protected or in use.\r\nPlease try again later or delete it manually.", "Delete Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
            }
        }
    }
}
