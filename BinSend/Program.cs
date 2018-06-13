using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BinSend
{
    public static class Program
    {
        private static string _appPath;
        public static List<BitmessageMsg> MessageCache;


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

            MessageCache = new List<BitmessageMsg>();

            var C = Config.Load(AppPath);

            if (C.Valid())
            {
                BitmessageClientStatus Status = default(BitmessageClientStatus);
                try
                {
                    Status = Tools.GetRPC(C.ApiSettings).clientStatus().FromJson<BitmessageClientStatus>();
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
                if (!string.IsNullOrEmpty(Status.softwareName))
                {
#if DEBUG
                    Application.Run(new frmMain(C));
#else
                    try
                    {
                        Application.Run(new frmMain(C));
                    }
                    catch (Exception ex)
                    {
                        //TODO:Nice Exception handler
                        MessageBox.Show($"{ex.Message}\r\n\r\n{ex.StackTrace}", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
#endif
                }
                else if (MessageBox.Show("API returned unexpected data. Reconfigure now? Selecting ''No'' will exit", "API Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Configure();
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
