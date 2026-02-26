using System;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    internal static class Program
    {
        public static string DbPath = System.IO.Path.Combine(Application.StartupPath, @"Data\church.db");

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 2. Теперь здесь будет вызываться реальная форма из папки Forms
            Application.Run(new MainForm());
        }
    }
}