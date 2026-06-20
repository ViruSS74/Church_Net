using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;

namespace ChurchBudget.Forms
{
    public partial class MainForm : Form
    {
        private Label lblOrgName;
        private PictureBox pictureBoxCustom;

        public MainForm()
        {
            InitializeComponent();
            CreateMenu();
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing; // ← ДОБАВИТЬ
        }

        private void CreateMenu()
        {
            menuStrip1.Items.Clear();

            // === ФАЙЛ ===
            var fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Выход", Properties.Resources.exit, ExitToolStripMenuItem_Click);
            menuStrip1.Items.Add(fileMenu);

            // === ДОКУМЕНТЫ ===
            var docsMenu = new ToolStripMenuItem("Документы");
            docsMenu.DropDownItems.Add("Новый доход", Properties.Resources.income_doc, NewIncomeToolStripMenuItem_Click);
            docsMenu.DropDownItems.Add("Новый расход", Properties.Resources.expense_doc, NewExpensesToolStripMenuItem_Click);
            docsMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            docsMenu.DropDownItems.Add("Ввод начальных остатков", Properties.Resources.opening_balance, OpeningBalanceToolStripMenuItem_Click);
            menuStrip1.Items.Add(docsMenu);

            // === ОТЧЕТЫ ===
            var reportsMenu = new ToolStripMenuItem("Отчеты");
            reportsMenu.DropDownItems.Add("Список документов", Properties.Resources.income_rep, ListOfDocsToolStripMenuItem_Click);
            reportsMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            reportsMenu.DropDownItems.Add("Кассовая книга", Properties.Resources.cashbook, CashbookToolStripMenuItem_Click);
            reportsMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            reportsMenu.DropDownItems.Add("Финансовый отчет", Properties.Resources.finreport, FinanceReportToolStripMenuItem_Click);
            menuStrip1.Items.Add(reportsMenu);

            // === СПРАВОЧНИКИ ===
            var dirsMenu = new ToolStripMenuItem("Справочники");
            dirsMenu.DropDownItems.Add("Организации", Properties.Resources.church, OrganizationDirToolStripMenuItem_Click);
            dirsMenu.DropDownItems.Add("Сотрудники", Properties.Resources.personal, EmployeeDirToolStripMenuItem_Click);
            dirsMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            dirsMenu.DropDownItems.Add("Документы (виды)", Properties.Resources.id_docs, IDDocsDirToolStripMenuItem_Click);
            dirsMenu.DropDownItems.Add("Типы документов", Properties.Resources.id_types, TypesIDDocsDirToolStripMenuItem_Click);
            dirsMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            dirsMenu.DropDownItems.Add("Категории доходов", Properties.Resources.income_cat, IncomeCatDirToolStripMenuItem_Click);
            dirsMenu.DropDownItems.Add("Категории расходов", Properties.Resources.expense_cat, ExpensesCatDirToolStripMenuItem_Click);
            dirsMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            dirsMenu.DropDownItems.Add("Виды документов", Properties.Resources.doc_types, TypesOfDocsToolStripMenuItem_Click);
            dirsMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            dirsMenu.DropDownItems.Add("Константы", Properties.Resources.constants, ConstantDirToolStripMenuItem_Click);
            menuStrip1.Items.Add(dirsMenu);

            // === СЕРВИС ===
            var serviceMenu = new ToolStripMenuItem("Сервис");
            serviceMenu.DropDownItems.Add("Обслуживание базы данных", Properties.Resources.repare_arch, DbMaintenanceToolStripMenuItem_Click);
            serviceMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            serviceMenu.DropDownItems.Add("Архивировать БД", Properties.Resources.arch_db, ArchiveDBToolStripMenuItem_Click);
            serviceMenu.DropDownItems.Add("Восстановить БД", Properties.Resources.repare_db, RestoreDBToolStripMenuItem_Click);
            serviceMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            serviceMenu.DropDownItems.Add("Очистка БД", Properties.Resources.clean_db, CleanDBwithParametrsToolStripMenuItem_Click);
            menuStrip1.Items.Add(serviceMenu);
            
            // === СПРАВКА ===
            var helpMenu = new ToolStripMenuItem("Справка");
            helpMenu.DropDownItems.Add("Справка по программе", Properties.Resources.help, HelpOfProgToolStripMenuItem_Click);
            helpMenu.DropDownItems.Add(new ToolStripSeparator()); // Разделитель
            helpMenu.DropDownItems.Add("О программе (About)", Properties.Resources.about, AbpoutBoxToolStripMenuItem_Click);
            menuStrip1.Items.Add(helpMenu);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. Создаём PictureBox программно
            if (pictureBox1 != null) pictureBox1.Visible = false;

            pictureBoxCustom = new PictureBox();
            pictureBoxCustom.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxCustom.BackColor = Color.Transparent;

            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "main.png");
            if (File.Exists(imgPath))
            {
                pictureBoxCustom.Image = Image.FromFile(imgPath);
                this.Controls.Add(pictureBoxCustom);
                pictureBoxCustom.BringToFront();
            }

            // 2. Центрирование
            CenterPictureBox();

            // 3. БД и надпись
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
            int topMargin = menuStrip1.Height + 50; // Место для надписи
            int availableHeight = this.ClientSize.Height - topMargin - statusStrip1.Height;

            float ratioX = (float)availableWidth / pictureBoxCustom.Image.Width;
            float ratioY = (float)availableHeight / pictureBoxCustom.Image.Height;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(pictureBoxCustom.Image.Width * ratio);
            int newHeight = (int)(pictureBoxCustom.Image.Height * ratio);

            pictureBoxCustom.Size = new Size(newWidth, newHeight);

            int x = (this.ClientSize.Width - newWidth) / 2;
            int y = topMargin + (availableHeight - newHeight) / 2;

            pictureBoxCustom.Location = new Point(x, y);

            UpdateLabelPosition();
        }

        private void UpdateLabelPosition()
        {
            if (lblOrgName != null && pictureBoxCustom != null && pictureBoxCustom.Visible)
            {
                int x = pictureBoxCustom.Left + (pictureBoxCustom.Width - lblOrgName.PreferredWidth) / 2;
                int y = pictureBoxCustom.Top - 35; // Надпись над картинкой

                lblOrgName.Location = new Point(x, y);
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
                            lblOrgName.BackColor = Color.FromArgb(100, 0, 0, 0);
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

        // === Обработчики меню ===
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }
        private void NewIncomeToolStripMenuItem_Click(object sender, EventArgs e) { new IncomeForm().ShowDialog(); }
        private void NewExpensesToolStripMenuItem_Click(object sender, EventArgs e) { new ExpensesDocForm().ShowDialog(); }
        private void OpeningBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new OpeningBalanceForm(1, Program.DbPath);
            form.ShowDialog();
        }
        private void ListOfDocsToolStripMenuItem_Click(object sender, EventArgs e) { new ListOfDocsForm().ShowDialog(); }
        private void CashbookToolStripMenuItem_Click(object sender, EventArgs e) { new CashbookForm().ShowDialog(); }
        private void FinanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var reportForm = new FinanceReportForm(GetCurrentOrganizationId(Program.DbPath), Program.DbPath);
            reportForm.ShowDialog();
        }
        private int GetCurrentOrganizationId(string connString)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("SELECT id FROM organizations LIMIT 1", conn))
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
        private void DbMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new DbUpdateForm(Program.DbFilePath);  // ✅ СТАЛО
            form.ShowDialog();
        }
        private void ArchiveDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateBackup())
                MessageBox.Show("Резервная копия создана в Data\\Archive", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("Заменить текущую БД?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Copy(ofd.FileName, dbPath, true);
                            MessageBox.Show("Восстановлено. Перезапуск...");
                            Application.Restart();
                        }
                        catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
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
                    string destFile = Path.Combine(archiveFolder, "church_backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".db");
                    File.Copy(sourceFile, destFile, true);
                    var filesToDelete = new DirectoryInfo(archiveFolder).GetFiles("*.db").OrderByDescending(f => f.CreationTime).Skip(10).ToList();
                    foreach (var file in filesToDelete) file.Delete();
                    return true;
                }
                return false;
            }
            catch (Exception ex) { MessageBox.Show("Ошибка бэкапа: " + ex.Message); return false; }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Выйти?\nБудет создан бэкап БД.", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                CreateBackup();
            else
                e.Cancel = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabelDate.Text = "Сегодня: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}