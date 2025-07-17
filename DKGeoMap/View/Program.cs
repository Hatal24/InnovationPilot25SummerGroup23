using System;
using System.Windows.Forms;

namespace DKGeoMap.View
{
    // Ensure only one Program class exists in this namespace.
    public class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

