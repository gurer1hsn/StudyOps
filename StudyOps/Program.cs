using System;
using System.Windows.Forms;

namespace StudyOps
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ✅ Uygulama ARTIK MainForm ile başlayacak
            Application.Run(new MainForm());
        }
    }
}
