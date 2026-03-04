namespace ChurchBudget.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.NewIncomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewExpensesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuReportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListOfDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.CashbookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FinanceReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDirsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrganizationDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.EmployeeDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.IDDocsDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TypesIDDocsDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.IncomeCatDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExpensesCatDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.TypesOfDocsDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuServiceoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ArchiveDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.RestoreDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpOfProgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.AbpoutBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelDBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFileToolStripMenuItem,
            this.MenuDocsToolStripMenuItem,
            this.MenuReportsToolStripMenuItem,
            this.MenuDirsToolStripMenuItem,
            this.MenuServiceoolStripMenuItem,
            this.MenuHelpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1264, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuFileToolStripMenuItem
            // 
            this.MenuFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitToolStripMenuItem});
            this.MenuFileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuFileToolStripMenuItem.Name = "MenuFileToolStripMenuItem";
            this.MenuFileToolStripMenuItem.Size = new System.Drawing.Size(64, 25);
            this.MenuFileToolStripMenuItem.Text = "Файл";
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.exit;
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // MenuDocsToolStripMenuItem
            // 
            this.MenuDocsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.NewIncomeToolStripMenuItem,
            this.NewExpensesToolStripMenuItem,
            this.toolStripSeparator2});
            this.MenuDocsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuDocsToolStripMenuItem.Name = "MenuDocsToolStripMenuItem";
            this.MenuDocsToolStripMenuItem.Size = new System.Drawing.Size(113, 25);
            this.MenuDocsToolStripMenuItem.Text = "Документы";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // NewIncomeToolStripMenuItem
            // 
            this.NewIncomeToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.income_doc;
            this.NewIncomeToolStripMenuItem.Name = "NewIncomeToolStripMenuItem";
            this.NewIncomeToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
            this.NewIncomeToolStripMenuItem.Text = "Новый доход";
            this.NewIncomeToolStripMenuItem.Click += new System.EventHandler(this.NewIncomeToolStripMenuItem_Click);
            // 
            // NewExpensesToolStripMenuItem
            // 
            this.NewExpensesToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.expense_doc;
            this.NewExpensesToolStripMenuItem.Name = "NewExpensesToolStripMenuItem";
            this.NewExpensesToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
            this.NewExpensesToolStripMenuItem.Text = "Новый расход";
            this.NewExpensesToolStripMenuItem.Click += new System.EventHandler(this.NewExpensesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(195, 6);
            // 
            // MenuReportsToolStripMenuItem
            // 
            this.MenuReportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ListOfDocsToolStripMenuItem,
            this.toolStripSeparator11,
            this.CashbookToolStripMenuItem,
            this.FinanceReportToolStripMenuItem});
            this.MenuReportsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuReportsToolStripMenuItem.Name = "MenuReportsToolStripMenuItem";
            this.MenuReportsToolStripMenuItem.Size = new System.Drawing.Size(80, 25);
            this.MenuReportsToolStripMenuItem.Text = "Отчеты";
            // 
            // ListOfDocsToolStripMenuItem
            // 
            this.ListOfDocsToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.new_document;
            this.ListOfDocsToolStripMenuItem.Name = "ListOfDocsToolStripMenuItem";
            this.ListOfDocsToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.ListOfDocsToolStripMenuItem.Text = "Список документов";
            this.ListOfDocsToolStripMenuItem.Click += new System.EventHandler(this.ListOfDocsToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(237, 6);
            // 
            // CashbookToolStripMenuItem
            // 
            this.CashbookToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.cashbook;
            this.CashbookToolStripMenuItem.Name = "CashbookToolStripMenuItem";
            this.CashbookToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.CashbookToolStripMenuItem.Text = "Кассовая книга";
            this.CashbookToolStripMenuItem.Click += new System.EventHandler(this.CashbookToolStripMenuItem_Click);
            // 
            // FinanceReportToolStripMenuItem
            // 
            this.FinanceReportToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.finreport;
            this.FinanceReportToolStripMenuItem.Name = "FinanceReportToolStripMenuItem";
            this.FinanceReportToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.FinanceReportToolStripMenuItem.Text = "Финансовый отчет";
            this.FinanceReportToolStripMenuItem.Click += new System.EventHandler(this.FinanceReportToolStripMenuItem_Click);
            // 
            // MenuDirsToolStripMenuItem
            // 
            this.MenuDirsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OrganizationDirToolStripMenuItem,
            this.toolStripSeparator5,
            this.EmployeeDirToolStripMenuItem,
            this.toolStripSeparator6,
            this.IDDocsDirToolStripMenuItem,
            this.TypesIDDocsDirToolStripMenuItem,
            this.toolStripSeparator7,
            this.IncomeCatDirToolStripMenuItem,
            this.ExpensesCatDirToolStripMenuItem,
            this.toolStripSeparator10,
            this.TypesOfDocsDirToolStripMenuItem});
            this.MenuDirsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuDirsToolStripMenuItem.Name = "MenuDirsToolStripMenuItem";
            this.MenuDirsToolStripMenuItem.Size = new System.Drawing.Size(129, 25);
            this.MenuDirsToolStripMenuItem.Text = "Справочники";
            // 
            // OrganizationDirToolStripMenuItem
            // 
            this.OrganizationDirToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.church;
            this.OrganizationDirToolStripMenuItem.Name = "OrganizationDirToolStripMenuItem";
            this.OrganizationDirToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.OrganizationDirToolStripMenuItem.Text = "Организация";
            this.OrganizationDirToolStripMenuItem.Click += new System.EventHandler(this.OrganizationDirToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(252, 6);
            // 
            // EmployeeDirToolStripMenuItem
            // 
            this.EmployeeDirToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.personal;
            this.EmployeeDirToolStripMenuItem.Name = "EmployeeDirToolStripMenuItem";
            this.EmployeeDirToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.EmployeeDirToolStripMenuItem.Text = "Сотрудники";
            this.EmployeeDirToolStripMenuItem.Click += new System.EventHandler(this.EmployeeDirToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(252, 6);
            // 
            // IDDocsDirToolStripMenuItem
            // 
            this.IDDocsDirToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.id_docs;
            this.IDDocsDirToolStripMenuItem.Name = "IDDocsDirToolStripMenuItem";
            this.IDDocsDirToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.IDDocsDirToolStripMenuItem.Text = "ИД документы";
            this.IDDocsDirToolStripMenuItem.Click += new System.EventHandler(this.IDDocsDirToolStripMenuItem_Click);
            // 
            // TypesIDDocsDirToolStripMenuItem
            // 
            this.TypesIDDocsDirToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.id_types;
            this.TypesIDDocsDirToolStripMenuItem.Name = "TypesIDDocsDirToolStripMenuItem";
            this.TypesIDDocsDirToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.TypesIDDocsDirToolStripMenuItem.Text = "Виды ИД документов";
            this.TypesIDDocsDirToolStripMenuItem.Click += new System.EventHandler(this.TypesIDDocsDirToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(252, 6);
            // 
            // IncomeCatDirToolStripMenuItem
            // 
            this.IncomeCatDirToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.income_cat;
            this.IncomeCatDirToolStripMenuItem.Name = "IncomeCatDirToolStripMenuItem";
            this.IncomeCatDirToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.IncomeCatDirToolStripMenuItem.Text = "Категории доходов";
            this.IncomeCatDirToolStripMenuItem.Click += new System.EventHandler(this.IncomeCatDirToolStripMenuItem_Click);
            // 
            // ExpensesCatDirToolStripMenuItem
            // 
            this.ExpensesCatDirToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.expense_cat;
            this.ExpensesCatDirToolStripMenuItem.Name = "ExpensesCatDirToolStripMenuItem";
            this.ExpensesCatDirToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.ExpensesCatDirToolStripMenuItem.Text = "Категории расходов";
            this.ExpensesCatDirToolStripMenuItem.Click += new System.EventHandler(this.ExpensesCatDirToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(252, 6);
            // 
            // TypesOfDocsDirToolStripMenuItem
            // 
            this.TypesOfDocsDirToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.doc_types;
            this.TypesOfDocsDirToolStripMenuItem.Name = "TypesOfDocsDirToolStripMenuItem";
            this.TypesOfDocsDirToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.TypesOfDocsDirToolStripMenuItem.Text = "Типы документов";
            this.TypesOfDocsDirToolStripMenuItem.Click += new System.EventHandler(this.TypesOfDocsToolStripMenuItem_Click);
            // 
            // MenuServiceoolStripMenuItem
            // 
            this.MenuServiceoolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ArchiveDBToolStripMenuItem,
            this.toolStripSeparator8,
            this.RestoreDBToolStripMenuItem});
            this.MenuServiceoolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuServiceoolStripMenuItem.Name = "MenuServiceoolStripMenuItem";
            this.MenuServiceoolStripMenuItem.Size = new System.Drawing.Size(78, 25);
            this.MenuServiceoolStripMenuItem.Text = "Сервис";
            // 
            // ArchiveDBToolStripMenuItem
            // 
            this.ArchiveDBToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.arch_db;
            this.ArchiveDBToolStripMenuItem.Name = "ArchiveDBToolStripMenuItem";
            this.ArchiveDBToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.ArchiveDBToolStripMenuItem.Text = "Архивирование БД";
            this.ArchiveDBToolStripMenuItem.Click += new System.EventHandler(this.ArchiveDBToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(236, 6);
            // 
            // RestoreDBToolStripMenuItem
            // 
            this.RestoreDBToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.repare_db;
            this.RestoreDBToolStripMenuItem.Name = "RestoreDBToolStripMenuItem";
            this.RestoreDBToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.RestoreDBToolStripMenuItem.Text = "Восстановление БД";
            this.RestoreDBToolStripMenuItem.Click += new System.EventHandler(this.RestoreDBToolStripMenuItem_Click);
            // 
            // MenuHelpToolStripMenuItem
            // 
            this.MenuHelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpOfProgToolStripMenuItem,
            this.toolStripSeparator9,
            this.AbpoutBoxToolStripMenuItem});
            this.MenuHelpToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuHelpToolStripMenuItem.Name = "MenuHelpToolStripMenuItem";
            this.MenuHelpToolStripMenuItem.Size = new System.Drawing.Size(88, 25);
            this.MenuHelpToolStripMenuItem.Text = "Справка";
            // 
            // HelpOfProgToolStripMenuItem
            // 
            this.HelpOfProgToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.help;
            this.HelpOfProgToolStripMenuItem.Name = "HelpOfProgToolStripMenuItem";
            this.HelpOfProgToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            this.HelpOfProgToolStripMenuItem.Text = "Справка по программе";
            this.HelpOfProgToolStripMenuItem.Click += new System.EventHandler(this.HelpOfProgToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(264, 6);
            // 
            // AbpoutBoxToolStripMenuItem
            // 
            this.AbpoutBoxToolStripMenuItem.Image = global::ChurchBudget.Properties.Resources.about;
            this.AbpoutBoxToolStripMenuItem.Name = "AbpoutBoxToolStripMenuItem";
            this.AbpoutBoxToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            this.AbpoutBoxToolStripMenuItem.Text = "О программе";
            this.AbpoutBoxToolStripMenuItem.Click += new System.EventHandler(this.AbpoutBoxToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripStatusLabelDBStatus,
            this.toolStripStatusLabelDate});
            this.statusStrip1.Location = new System.Drawing.Point(0, 659);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1264, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabelDBStatus
            // 
            this.toolStripStatusLabelDBStatus.Name = "toolStripStatusLabelDBStatus";
            this.toolStripStatusLabelDBStatus.Size = new System.Drawing.Size(1249, 17);
            this.toolStripStatusLabelDBStatus.Spring = true;
            // 
            // toolStripStatusLabelDate
            // 
            this.toolStripStatusLabelDate.Name = "toolStripStatusLabelDate";
            this.toolStripStatusLabelDate.Size = new System.Drawing.Size(0, 17);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(334, 155);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(607, 418);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "===Церковный бюджет v.3.0.31===";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuDocsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuReportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuDirsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuServiceoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuHelpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDBStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDate;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem NewIncomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewExpensesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem CashbookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FinanceReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OrganizationDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem EmployeeDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem IDDocsDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TypesIDDocsDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem IncomeCatDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExpensesCatDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ArchiveDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem RestoreDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpOfProgToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem AbpoutBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem TypesOfDocsDirToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem ListOfDocsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
    }
}