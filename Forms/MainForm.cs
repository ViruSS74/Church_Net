using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            lblStatus.Text = "Система готова";
            toolStripStatusLabelDate.Text = "Сегодня: " + DateTime.Now.ToString("dd.MM.yyyy");
        }

        // --- ОБРАБОТКА МЕНЮ ФАЙЛ ---
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerformBackupAndExit();
        }

        // --- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ (ЛОГИКА БЭКАПА) ---
        private void PerformBackupAndExit()
        {
            CreateBackup();
            Application.Exit();
        }

        private bool CreateBackup()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string sourceFile = Path.Combine(baseDir, "Data\\church.db");
            string archiveFolder = Path.Combine(baseDir, "Data\\Archive");

            try
            {
                if (File.Exists(sourceFile))
                {
                    if (!Directory.Exists(archiveFolder)) Directory.CreateDirectory(archiveFolder);

                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string destFile = Path.Combine(archiveFolder, "church_backup_" + timestamp + ".db");
                    File.Copy(sourceFile, destFile, true);

                    // Очистка старых копий (оставляем последние 10)
                    var filesToDelete = new DirectoryInfo(archiveFolder)
                                            .GetFiles("*.db")
                                            .OrderByDescending(f => f.CreationTime)
                                            .Skip(10)
                                            .ToList();

                    foreach (var file in filesToDelete)
                    {
                        file.Delete();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка бэкапа: " + ex.Message);
                return false;
            }
        }

        // --- ОБРАБОТКА МЕНЮ ДОКУМЕНТ ---
        private void NewIncomeToolStripMenuItem_Click(object sender, EventArgs e) { new IncomeForm().ShowDialog(); }
        private void NewExpensesToolStripMenuItem_Click(object sender, EventArgs e) { new ExpensesDocForm().ShowDialog(); }

        // --- ОБРАБОТКА МЕНЮ ОТЧЕТЫ ---
        private void ListOfDocsToolStripMenuItem_Click(object sender, EventArgs e) { new ListOfDocsForm().ShowDialog(); }
        private void CashbookToolStripMenuItem_Click(object sender, EventArgs e) { new CashbookForm().ShowDialog(); }
        private void FinanceReportToolStripMenuItem_Click(object sender, EventArgs e) { new FinanceReportForm().ShowDialog(); }

        // --- ОБРАБОТКА МЕНЮ СПРАВОЧНИКИ ---
        private void OrganizationDirToolStripMenuItem_Click(object sender, EventArgs e) { new OrganizationDirForm().ShowDialog(); }
        private void EmployeeDirToolStripMenuItem_Click(object sender, EventArgs e) { new EmployeeDirForm().ShowDialog(); }
        private void IDDocsDirToolStripMenuItem_Click(object sender, EventArgs e) { new IDDocsDirForm().ShowDialog(); }
        private void TypesIDDocsDirToolStripMenuItem_Click(object sender, EventArgs e) { new ViewsOfIDDocsDirForm().ShowDialog(); }
        private void IncomeCatDirToolStripMenuItem_Click(object sender, EventArgs e) { new IncomeCatDirForm().ShowDialog(); }
        private void ExpensesCatDirToolStripMenuItem_Click(object sender, EventArgs e) { new ExpensesCatDirForm().ShowDialog(); }
        private void TypesOfDocsToolStripMenuItem_Click(object sender, EventArgs e) { new TypesOfDocsDirForm().ShowDialog(); }

        // --- ОБРАБОТКА МЕНЮ СЕРВИС ---
        private void ArchiveDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateBackup())
            {
                MessageBox.Show("Резервная копия успешно создана в папке Data\\Archive", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RestoreDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = Path.Combine(baseDir, "Data\\church.db");
            string archiveFolder = Path.Combine(baseDir, "Data\\Archive");

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (Directory.Exists(archiveFolder)) ofd.InitialDirectory = archiveFolder;
                ofd.Filter = "SQLite Database (*.db)|*.db";
                ofd.Title = "Выберите файл для восстановления";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("Текущая база данных будет заменена. Продолжить?", "Подтверждение",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Copy(ofd.FileName, dbPath, true);
                            MessageBox.Show("Восстановление завершено. Приложение будет перезапущено.", "Готово");
                            Application.Restart();
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("Ошибка: База данных занята. Закройте все окна и попробуйте снова.", "Доступ заблокирован");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                        }
                    }
                }
            }
        }

        // --- ОБРАБОТКА МЕНЮ СПРАВКА ---
        private void HelpOfProgToolStripMenuItem_Click(object sender, EventArgs e) { new HelpForm().ShowDialog(); }
        private void AbpoutBoxToolStripMenuItem_Click(object sender, EventArgs e) { new AboutBoxForm().ShowDialog(); }
    }
}
