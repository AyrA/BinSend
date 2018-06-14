using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmApiConfig : Form
    {
        public ApiConfig ApiConfiguration;

        public frmApiConfig(Config C)
        {
            ApiConfiguration = C.ApiSettings;
            InitializeComponent();

            //Prevent vertical sizing
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);

            nudPort.Minimum = IPEndPoint.MinPort;
            nudPort.Maximum = IPEndPoint.MaxPort;
            tbIP.Text = ApiConfiguration.IpOrHostname;
            tbUsername.Text = ApiConfiguration.Username;
            tbPassword.Text = ApiConfiguration.Password;
            nudPort.Value = Math.Max(Math.Min(ApiConfiguration.Port, nudPort.Maximum), nudPort.Minimum);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ApiConfiguration.Username = tbUsername.Text;
            ApiConfiguration.Password = tbPassword.Text;
            ApiConfiguration.IpOrHostname = tbIP.Text;
            ApiConfiguration.Port = (int)nudPort.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var TempConfig = new ApiConfig()
            {
                IpOrHostname = tbIP.Text,
                Password = tbPassword.Text,
                Username = tbUsername.Text,
                Port = (int)nudPort.Value
            };
            var API = Tools.GetRPC(TempConfig);
            BitmessageClientStatus Status;
            try
            {
                var RAW = API.clientStatus();
                Status = RAW.FromJson<BitmessageClientStatus>();
                if (!string.IsNullOrEmpty(Status.softwareName))
                {
                    MessageBox.Show($"API OK. Detected client: {Status.softwareName} {Status.softwareVersion}", "API Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"API Error: clientStatus() returned incomplete data.\r\nThe request itself succeded but the client did not return an expected data structure. Continue at your own risk.\r\nData: {RAW}", "API Test", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to make API request. Reason: {ex.Message}", "API Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
