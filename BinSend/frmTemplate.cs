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
        private Template[] WorkingTemplates;


        public frmTemplate(Template[] Templates)
        {
            SelectedIndex = 0;
            WorkingTemplates = (Template[])(this.Templates = Templates).Clone();
            InitializeComponent();
            SetTemplateList();
        }

        private void SetTemplateList(int Select = -1)
        {
            cbEncoding.Items.Clear();
            cbEncoding.Items.AddRange(WorkingTemplates.Select(m => (object)m.Name).ToArray());
            if (Select > -1 && cbEncoding.Items.Count > 0)
            {
                cbEncoding.SelectedIndex = Math.Max(Math.Min(Select, cbEncoding.Items.Count - 1), 0);
            }
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
                WorkingTemplates = WorkingTemplates.Where((v, i) => i != Sel).ToArray();
            }
            SetTemplateList(Sel);
        }
    }
}
