using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmTemplateEditor : Form
    {
        public frmTemplateEditor()
        {
            InitializeComponent();
            cbEncoding.DataSource = Enum.GetValues(typeof(EncodingFormat));

            DialogResult = DialogResult.Cancel;
        }

        public frmTemplateEditor(Template T)
        {
            InitializeComponent();
            cbEncoding.DataSource = Enum.GetValues(typeof(EncodingFormat));

            tbName.Text = T.Name;
            tbFrom.Text = T.From;
            tbTo.Text = T.To;
            tbText.Text = T.Text;
            tbSubject.Text = T.Subject;
            nudKB.Value = T.ChunkSize;
            cbEncoding.SelectedItem = T.ProposedFormat;
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Template T = new Template();
            T.Name = tbName.Text;
            T.From = tbFrom.Text;
            T.To = tbTo.Text;
            T.Text = tbText.Text;
            T.Subject = tbSubject.Text;
            T.ChunkSize = (int)nudKB.Value;
            T.ProposedFormat = (EncodingFormat)cbEncoding.SelectedItem;
            if (T.Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Cannot save template\r\nMake sure its name is a valid file name", "Error saving template", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.D0:
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                    case Keys.D4:
                    case Keys.D5:
                    case Keys.D6:
                        int s = tbText.SelectionStart;
                        tbText.Text = string.Format("{0}{1}{2}{3}{4}",
                            tbText.Text.Substring(0, s),
                            "{",
                            (int)e.KeyCode - (int)Keys.D0,
                            "}",
                            tbText.Text.Substring(s));
                        tbText.SelectionStart = s + 3;
                        break;
                }
            }
        }
    }
}
