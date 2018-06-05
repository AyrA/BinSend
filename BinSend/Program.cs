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
                Application.Run(new frmMain());
            }
            else
            {
                C = Config.GetDefaults();
                MessageBox.Show("Invalid Configuration. Please set your API values", "Invalid API settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
