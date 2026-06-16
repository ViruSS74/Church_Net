using System;
using System.Windows.Forms;
using System.IO;  // ← Добавили для Path

namespace ChurchBudget.Forms  // ← Оставляем как есть (всё работает)
{
    internal static class Program
    {
        // ✅ Используем string.Format вместо $ (совместимо с .NET 3.5)
        // ✅ Имя поля DbPath (как в остальных файлах)        
        public static string DbPath =
            string.Format("Data Source={0};Version=3;",
                Path.Combine(Application.StartupPath, @"Data\church.db"));

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