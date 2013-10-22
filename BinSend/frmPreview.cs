using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmPreview : Form
    {
        private const int MAX_LEN = 10000;
        public frmPreview(string T)
        {
            InitializeComponent();
            tbPreview.Text = (T.Length > MAX_LEN ? T.Substring(0, MAX_LEN) : T);
        }
    }
}
