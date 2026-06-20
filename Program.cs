using System;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    internal static class Program
    {
        // Путь к файлу БД (чистый путь, без "Data Source=")
        public static string DbFilePath =
            Path.Combine(Application.StartupPath, @"Data\church.db");

        // Connection string для SQLite
        public static string DbPath =
            string.Format("Data Source={0};Version=3;", DbFilePath);

        [STAThread]
        static void Main()
        {
            // Создаём папку для чеков, если её нет
            string receiptsPath = Path.Combine(Application.StartupPath, "Receipts");
            if (!Directory.Exists(receiptsPath))
            {
                Directory.CreateDirectory(receiptsPath);
            }
            string archivePath = Path.Combine(Application.StartupPath, "Data", "Archive");
            if (!Directory.Exists(archivePath))
            {
                Directory.CreateDirectory(archivePath);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}