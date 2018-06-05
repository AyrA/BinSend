using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BinSend
{
    public static class Program
    {
        private static string _appPath;

        /// <summary>
        /// Gets the Directory of this executable
        /// </summary>
        public static string AppPath
        {
            get
            {
                if (_appPath == null)
                {
                    using (var P = Process.GetCurrentProcess())
                    {
                        _appPath = Path.GetDirectoryName(P.MainModule.FileName);
                    }
                }
                return _appPath;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var C = Config.Load(AppPath);

            if (C.Valid())
            {
                try
                {
                    var Status = Tools.GetRPC(C.ApiSettings).clientStatus().FromJson<BitmessageClientStatus>();
                    if (!string.IsNullOrEmpty(Status.softwareName))
                    {
                        Application.Run(new frmMain(C));
                    }
                    else if (MessageBox.Show("API returned unexpected data. Reconfigure now? Selecting ''No'' will exit", "API Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Configure();
                    }
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show($@"Unable to make API request.
Make sure that bitmessage is running.
Message: {ex.Message}

Edit Configuratio now? Selecting ''No'' will exit", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        Configure();
                    }
                }
            }
            else
            {
                Configure();
            }
        }

        /// <summary>
        /// Configures the API
        /// </summary>
        private static void Configure()
        {
            var C = Config.GetDefaults();
            MessageBox.Show("Invalid Configuration. Please set your API values", "Invalid API settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            var f = new frmApiConfig(C);
            Application.Run(f);
            if (f.DialogResult == DialogResult.OK)
            {
                C.ApiSettings = f.ApiConfiguration;
                C.Save(AppPath);
                Application.Restart();
            }

        }
    }
}
