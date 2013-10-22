using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CookComputing.XmlRpc;

namespace BinSend
{
    public partial class frmAPI : Form
    {
        public frmAPI()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            BitAPI BA = (BitAPI)XmlRpcProxyGen.Create(typeof(BitAPI));
            BA.Url = string.Format("http://{0}/", tbRemote.Text);
            BA.Headers.Add("Authorization", "Basic " + JsonConverter.B64enc(string.Format("{0}:{1}", tbUsername.Text, tbPWD.Text)));

            try
            {
                string s = BA.helloWorld("API", "working");
                if (s == "API-working")
                {
                    MessageBox.Show("API settings are correct", "API check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Did not get the expected answer\r\n\r\n==API ANSWER==\r\n" + s, "API check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Got an error:\r\n" + ex.Message, "API not working", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            QuickSettings.Set("API-ADDR", tbRemote.Text);
            QuickSettings.Set("API-NAME", tbUsername.Text);
            QuickSettings.Set("API-PASS", tbPWD.Text);
            this.DialogResult = DialogResult.OK;
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
        }
    }
}
