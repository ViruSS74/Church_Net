using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using ChurchBudget.Forms;

namespace ChurchBudget.Forms
{
    public partial class MainForm : Form
    {
        private Label lblOrgName;
        // Объявляем новый PictureBox, который будем создавать сами
        private PictureBox pictureBoxCustom;

        public MainForm()
        {
            InitializeComponent();

            // Привязываем событие Load
            this.Load += new System.EventHandler(this.MainForm_Load);

            lblStatus.Text = "Система готова";
            ImageHelper.ApplyToButtons(this, 24);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. СОЗДАЕМ PICTUREBOX ПРОГРАММНО (ЯДЕРНЫЙ ВАРИАНТ)
            // Скрываем старый из дизайнера, чтобы он не мешал
            if (pictureBox1 != null) pictureBox1.Visible = false;

            pictureBoxCustom = new PictureBox();
            pictureBoxCustom.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxCustom.BackColor = Color.Transparent;

            // Пытаемся загрузить картинку
            string imgFileName = "main.png";
            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imgFileName);

            if (File.Exists(imgPath))
            {
                pictureBoxCustom.Image = Image.FromFile(imgPath);
                System.Diagnostics.Debug.WriteLine($"[OK] Картинка загружена: {imgPath}");
            }
            else
            {
                // Пробуем альтернативные имена
                string[] alternatives = { "main_bg.jpeg", "background.jpg", "nikolay.png" };
                foreach (string alt in alternatives)
                {
                    string altPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, alt);
                    if (File.Exists(altPath))
                    {
                        pictureBoxCustom.Image = Image.FromFile(altPath);
                        System.Diagnostics.Debug.WriteLine($"[OK] Найдена альтернатива: {altPath}");
                        break;
                    }
                }

                if (pictureBoxCustom.Image == null)
                    System.Diagnostics.Debug.WriteLine("[ERROR] Картинка НЕ найдена!");
            }

            // Добавляем наш новый PictureBox на форму
            if (pictureBoxCustom.Image != null)
            {
                this.Controls.Add(pictureBoxCustom);
                pictureBoxCustom.BringToFront(); // Поднимаем над меню и статусом
            }

            // 2. ЦЕНТРИРОВАНИЕ И МАСШТАБИРОВАНИЕ
            CenterPictureBox();

            // 3. БД И НАДПИСЬ
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\church.db");
            if (File.Exists(dbPath))
            {
                toolStripStatusLabelDBStatus.Text = "БД: Подключена";
                toolStripStatusLabelDBStatus.ForeColor = Color.DarkGreen;
                LoadOrganizationName(dbPath);
            }
            else
            {
                toolStripStatusLabelDBStatus.Text = "БД: Файл не найден!";
                toolStripStatusLabelDBStatus.ForeColor = Color.Red;
            }
        }

        private void CenterPictureBox()
        {
            if (pictureBoxCustom == null || pictureBoxCustom.Image == null) return;

            int availableWidth = this.ClientSize.Width;
            // Вычитаем высоту меню, статус-бара И место для надписи (примерно 40px)
            int topMargin = menuStrip1.Height + 50;
            int availableHeight = this.ClientSize.Height - topMargin - statusStrip1.Height;

            float ratioX = (float)availableWidth / pictureBoxCustom.Image.Width;
            float ratioY = (float)availableHeight / pictureBoxCustom.Image.Height;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(pictureBoxCustom.Image.Width * ratio);
            int newHeight = (int)(pictureBoxCustom.Image.Height * ratio);

            pictureBoxCustom.Size = new Size(newWidth, newHeight);

            // Центрируем по горизонтали
            int x = (this.ClientSize.Width - newWidth) / 2;

            // ✅ ОПУСКАЕМ КАРТИНКУ НИЖЕ, ЧТОБЫ ОСВОБОДИТЬ МЕСТО ДЛЯ НАДПИСИ
            // topMargin уже включает место для меню и надписи
            int y = topMargin + (availableHeight - newHeight) / 2;

            pictureBoxCustom.Location = new Point(x, y);

            UpdateLabelPosition();
        }

        private void UpdateLabelPosition()
        {
            if (lblOrgName != null && pictureBoxCustom != null && pictureBoxCustom.Visible)
            {
                // Центрируем надпись по горизонтали относительно картинки
                int x = (pictureBoxCustom.Width - lblOrgName.PreferredWidth) / 2;

                // ✅ СТАВИМ НАДПИСЬ РОВНО НАД КАРТИНКОЙ (с небольшим отступом)
                // Отрицательное значение поднимает её выше верхнего края PictureBox
                int y = -35;

                lblOrgName.Location = new Point(pictureBoxCustom.Left + x, pictureBoxCustom.Top + y);
            }
        }

        private void LoadOrganizationName(string dbPath)
        {
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = "SELECT name FROM organizations LIMIT 1";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string orgName = result.ToString();

                            lblOrgName = new Label();
                            lblOrgName.Text = orgName;
                            lblOrgName.Font = new Font("Arial", 20, FontStyle.Bold);
                            lblOrgName.ForeColor = Color.White;
                            lblOrgName.BackColor = Color.FromArgb(100, 0, 0, 0); // Полупрозрачный фон
                            lblOrgName.AutoSize = true;

                            this.Controls.Add(lblOrgName);
                            lblOrgName.BringToFront();

                            UpdateLabelPosition();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка загрузки названия: " + ex.Message);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.IsHandleCreated && pictureBoxCustom != null && pictureBoxCustom.Image != null)
            {
                CenterPictureBox();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }
        private void PerformBackupAndExit() { CreateBackup(); Application.Exit(); }
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
                    var filesToDelete = new DirectoryInfo(archiveFolder)
                                            .GetFiles("*.db")
                                            .OrderByDescending(f => f.CreationTime)
                                            .Skip(10)
                                            .ToList();
                    foreach (var file in filesToDelete) file.Delete();
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
        private void NewIncomeToolStripMenuItem_Click(object sender, EventArgs e) { new IncomeForm().ShowDialog(); }
        private void NewExpensesToolStripMenuItem_Click(object sender, EventArgs e) { new ExpensesDocForm().ShowDialog(); }
        private void OpeningBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int orgId = 1;
            var form = new OpeningBalanceForm(orgId, Program.DbPath);
            form.ShowDialog();
        }
        private void ListOfDocsToolStripMenuItem_Click(object sender, EventArgs e) { new ListOfDocsForm().ShowDialog(); }
        private void CashbookToolStripMenuItem_Click(object sender, EventArgs e) { new CashbookForm().ShowDialog(); }
        private void FinanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string connectionString = Program.DbPath;
            int currentOrgId = GetCurrentOrganizationId(connectionString);
            var reportForm = new FinanceReportForm(currentOrgId, connectionString);
            reportForm.ShowDialog();
        }
        private int GetCurrentOrganizationId(string connString)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT id FROM organizations LIMIT 1";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return Convert.ToInt32(result);
                    }
                }
            }
            catch { }
            return 1;
        }
        private void OrganizationDirToolStripMenuItem_Click(object sender, EventArgs e) { new OrganizationDirForm().ShowDialog(); }
        private void EmployeeDirToolStripMenuItem_Click(object sender, EventArgs e) { new EmployeeDirForm().ShowDialog(); }
        private void IDDocsDirToolStripMenuItem_Click(object sender, EventArgs e) { new IDDocsDirForm().ShowDialog(); }
        private void TypesIDDocsDirToolStripMenuItem_Click(object sender, EventArgs e) { new ViewsOfIDDocsDirForm().ShowDialog(); }
        private void IncomeCatDirToolStripMenuItem_Click(object sender, EventArgs e) { new IncomeCatDirForm().ShowDialog(); }
        private void ExpensesCatDirToolStripMenuItem_Click(object sender, EventArgs e) { new ExpensesCatDirForm().ShowDialog(); }
        private void TypesOfDocsToolStripMenuItem_Click(object sender, EventArgs e) { new TypesOfDocsDirForm().ShowDialog(); }
        private void ConstantDirToolStripMenuItem_Click(object sender, EventArgs e) { new ConstantDirForm(Program.DbPath).ShowDialog(); }
        private void ArchiveDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateBackup())
                MessageBox.Show("Резервная копия успешно создана в папке Data\\Archive", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void CleanDBwithParametrsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new CleanDatabaseForm(Program.DbPath)) { form.ShowDialog(); }
        }
        private void HelpOfProgToolStripMenuItem_Click(object sender, EventArgs e) { new HelpForm().ShowDialog(); }
        private void AbpoutBoxToolStripMenuItem_Click(object sender, EventArgs e) { new AboutBoxForm().ShowDialog(); }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                 "Вы действительно хотите выйти из программы?\nПеред выходом будет создана резервная копия БД.",
                 "Подтверждение выхода",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);
            if (result == DialogResult.Yes) { CreateBackup(); }
            else { e.Cancel = true; }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabelDate.Text = "Сегодня: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}