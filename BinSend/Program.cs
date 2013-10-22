using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BinSend
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            //We debug in a temp location so we do not mess stuff up
            if (!Directory.Exists(@"C:\TEMP\BINSEND"))
            {
                Directory.CreateDirectory(@"C:\TEMP\BINSEND");
            }
            Directory.SetCurrentDirectory(@"C:\TEMP\BINSEND");
#endif
            //location for file parts
            if (!Directory.Exists("parts"))
            {
                Directory.CreateDirectory("parts");
            }
            //check for templates
            if (!Directory.Exists(Templates.DIR_TEMPLATE) || !new List<string>(Templates.AllTemplates).Contains("Default"))
            {
                Templates.createStructure();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmBinSend(new Template("Default")));
        }
    }
}
