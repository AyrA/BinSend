using BinSend.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmApiConfig : Form
    {
        public ApiConfig ApiConfiguration;

        private const string KEYS_INSTALLED = @"%APPDATA%\PyBitmessage\keys.dat";

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

        private void btnAuto_Click(object sender, EventArgs e)
        {
            var Dir = Environment.ExpandEnvironmentVariables(KEYS_INSTALLED);
            //Prefer Keys.dat in the current directory
            if (File.Exists("keys.dat"))
            {
                OFD.InitialDirectory = Environment.CurrentDirectory;
            }
            //Check for installed version instead
            else if (Directory.Exists(Dir))
            {
                OFD.InitialDirectory = Dir;
            }
#if DEBUG
            OFD.InitialDirectory = @"C:\Projects\PyBitmessage\src";
#endif
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                INI I = null;
                using (var FS = File.OpenText(OFD.FileName))
                {
                    if (FS.BaseStream.Length > 10000000)
                    {
                        MessageBox.Show("This file looks like it's too large to be a bitmessage keys container. It will not be processed.", "File too large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        try
                        {
                            I = new INI(FS);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"We were unable to read {OFD.FileName}\r\nError: {ex.Message}", "File read error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                if (I != null)
                {
                    var Main = I["bitmessagesettings"];
                    if (Main != null)
                    {
                        var NeedUpdate = false;
                        var ApiStatus = Main["apienabled"];
                        var ApiAddr = Main["apiinterface"];
                        var ApiPort = Main["apiport"];
                        var ApiUsername = Main["apiusername"];
                        var ApiPassword = Main["apipassword"];

                        //API Status Flag
                        if (ApiStatus != null)
                        {
                            if (ApiStatus.Value.ToLower() != "true")
                            {
                                ApiStatus.Value = "true";
                                NeedUpdate = true;
                            }
                        }
                        else
                        {
                            ApiStatus = Main.Set("apienabled", "true", true);
                            NeedUpdate = true;
                        }

                        //API Listening Interface
                        if (ApiAddr != null)
                        {
                            tbIP.Text = ApiAddr.Value == "0.0.0.0" ? "127.0.0.1" : ApiAddr.Value;
                        }
                        else
                        {
                            ApiAddr = Main.Set("apiinterface", "127.0.0.1", true);
                            tbIP.Text = "127.0.0.1";
                            NeedUpdate = true;
                        }

                        //API Listening Port
                        if (ApiPort != null)
                        {
                            int Port = 0;
                            if (int.TryParse(ApiPort.Value, out Port) && Port > 0 && Port < ushort.MaxValue)
                            {
                                nudPort.Value = Port;
                            }
                            else
                            {
                                ApiPort.Value = (nudPort.Value = 8442).ToString();
                                NeedUpdate = true;
                            }
                        }
                        else
                        {
                            ApiPort = Main.Set("apiport", "8442", true);
                            nudPort.Value = 8442;
                            NeedUpdate = true;
                        }

                        //API Username
                        if (ApiUsername == null || string.IsNullOrEmpty(ApiUsername.Value))
                        {
                            var UN = Tools.Prompt("Please set an API username", "No API username was set", "username");
                            if (string.IsNullOrEmpty(UN))
                            {
                                MessageBox.Show("Invalid username. keys.dat will not be changed", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            ApiUsername = Main.Set("apiusername", UN, true);
                            NeedUpdate = true;
                        }
                        tbUsername.Text = ApiUsername.Value;

                        //API Password
                        if (ApiPassword == null || string.IsNullOrEmpty(ApiPassword.Value))
                        {
                            var PW = Tools.Prompt("Please set an API password", "No API password was set", Tools.RandomString(16));
                            if (string.IsNullOrEmpty(PW))
                            {
                                MessageBox.Show("Invalid password. keys.dat will not be changed", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            ApiPassword = Main.Set("apipassword", PW, true);
                            NeedUpdate = true;
                        }
                        tbPassword.Text = ApiPassword.Value;


                        //Check if changes to keys.dat are necessary
                        if (NeedUpdate)
                        {
                            if (MessageBox.Show($@"Your keys.dat needs updating because is misses values or they were invalid.

Please confirm these new API values:
API Enabled: Yes
Listen Interface: {ApiAddr.Value}
Listen Port: {ApiPort.Value}
API Username: {ApiUsername.Value}
API Password: {string.Empty.PadRight(ApiPassword.Value.Length, '*')}

→ MAKE SURE BITMESSAGE IS NOT RUNNING BEFORE CONTINUING ←

Save these values to keys.dat?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                try
                                {
                                    File.Copy(OFD.FileName, Path.ChangeExtension(OFD.FileName, "bak"), true);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Unable to backup 'keys.dat' to 'keys.bak'. Ensure the directory with keys.dat is writable and not full. Your changes will not be saved\r\n\r\nReason: {ex.Message}", "keys.dat Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                using (var FS = File.Create(OFD.FileName))
                                {
                                    using (var SW = new StreamWriter(FS, new UTF8Encoding(false)))
                                    {
                                        I.Write(SW);
                                    }
                                }
                                MessageBox.Show("API enabled and Configuration saved. Please start bitmessage now and test the settings.\r\n'keys.bak' was created in case something goes wrong", "API Enabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Operation aborted by user", "Changes discarded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("It looks like the API is already enabled and all values are properly set. The fields have been populated with these values.\r\nPlease ensure bitmessage is running and test the settings now.", "API already enabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is no 'bitmessagesettings' section in this file. If this really is a bitmessage configuration, ensure that bitmessage has been started and exited successfully at least once, then try again.", "Unable to read Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Tools.ShowHelp(Resources.HELP_API);
        }

        private void frmApiConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tools.CloseHelp();
        }
    }
}
