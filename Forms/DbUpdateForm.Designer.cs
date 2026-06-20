namespace ChurchBudget.Forms
{
    partial class DbUpdateForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbUpdateForm));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grpCheck = new System.Windows.Forms.GroupBox();
            this.lblCheckDesc = new System.Windows.Forms.Label();
            this.btnCheckIntegrity = new System.Windows.Forms.Button();
            this.grpOptimize = new System.Windows.Forms.GroupBox();
            this.lblVacuumDesc = new System.Windows.Forms.Label();
            this.btnVacuum = new System.Windows.Forms.Button();
            this.grpUpdate = new System.Windows.Forms.GroupBox();
            this.lblScriptPath = new System.Windows.Forms.Label();
            this.txtScriptPath = new System.Windows.Forms.TextBox();
            this.btnSelectScript = new System.Windows.Forms.Button();
            this.lblUpdateDesc = new System.Windows.Forms.Label();
            this.btnUpdateStructure = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvLogHistory = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanelLog = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefreshLog = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.lblLogInfo = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grpCheck.SuspendLayout();
            this.grpOptimize.SuspendLayout();
            this.grpUpdate.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogHistory)).BeginInit();
            this.flowLayoutPanelLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 521);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(778, 50);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(3, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(160, 40);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Закрыть";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(778, 521);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tabPage1.Controls.Add(this.grpCheck);
            this.tabPage1.Controls.Add(this.grpOptimize);
            this.tabPage1.Controls.Add(this.grpUpdate);
            this.tabPage1.Controls.Add(this.lblTitle);
            this.tabPage1.Location = new System.Drawing.Point(4, 30);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(770, 487);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Операции";
            // 
            // grpCheck
            // 
            this.grpCheck.Controls.Add(this.lblCheckDesc);
            this.grpCheck.Controls.Add(this.btnCheckIntegrity);
            this.grpCheck.Location = new System.Drawing.Point(23, 370);
            this.grpCheck.Name = "grpCheck";
            this.grpCheck.Size = new System.Drawing.Size(722, 106);
            this.grpCheck.TabIndex = 7;
            this.grpCheck.TabStop = false;
            this.grpCheck.Text = "Проверка целостности";
            // 
            // lblCheckDesc
            // 
            this.lblCheckDesc.AutoSize = true;
            this.lblCheckDesc.Location = new System.Drawing.Point(18, 75);
            this.lblCheckDesc.Name = "lblCheckDesc";
            this.lblCheckDesc.Size = new System.Drawing.Size(362, 21);
            this.lblCheckDesc.TabIndex = 1;
            this.lblCheckDesc.Text = "Проверяет, что все таблицы и поля на месте";
            // 
            // btnCheckIntegrity
            // 
            this.btnCheckIntegrity.Image = global::ChurchBudget.Properties.Resources.repare_db;
            this.btnCheckIntegrity.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckIntegrity.Location = new System.Drawing.Point(18, 28);
            this.btnCheckIntegrity.Name = "btnCheckIntegrity";
            this.btnCheckIntegrity.Size = new System.Drawing.Size(333, 40);
            this.btnCheckIntegrity.TabIndex = 0;
            this.btnCheckIntegrity.Text = "Проверить целостность БД";
            this.btnCheckIntegrity.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCheckIntegrity.UseVisualStyleBackColor = true;
            this.btnCheckIntegrity.Click += new System.EventHandler(this.btnCheckIntegrity_Click);
            // 
            // grpOptimize
            // 
            this.grpOptimize.Controls.Add(this.lblVacuumDesc);
            this.grpOptimize.Controls.Add(this.btnVacuum);
            this.grpOptimize.Location = new System.Drawing.Point(23, 256);
            this.grpOptimize.Name = "grpOptimize";
            this.grpOptimize.Size = new System.Drawing.Size(722, 113);
            this.grpOptimize.TabIndex = 6;
            this.grpOptimize.TabStop = false;
            this.grpOptimize.Text = "Оптимизация";
            // 
            // lblVacuumDesc
            // 
            this.lblVacuumDesc.AutoSize = true;
            this.lblVacuumDesc.Location = new System.Drawing.Point(27, 80);
            this.lblVacuumDesc.Name = "lblVacuumDesc";
            this.lblVacuumDesc.Size = new System.Drawing.Size(334, 21);
            this.lblVacuumDesc.TabIndex = 1;
            this.lblVacuumDesc.Text = "Сжимает базу данных и ускоряет работу";
            // 
            // btnVacuum
            // 
            this.btnVacuum.Image = global::ChurchBudget.Properties.Resources.hourglass;
            this.btnVacuum.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVacuum.Location = new System.Drawing.Point(22, 29);
            this.btnVacuum.Name = "btnVacuum";
            this.btnVacuum.Size = new System.Drawing.Size(333, 40);
            this.btnVacuum.TabIndex = 0;
            this.btnVacuum.Text = "Оптимизировать БД (VACUUM)";
            this.btnVacuum.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVacuum.UseVisualStyleBackColor = true;
            this.btnVacuum.Click += new System.EventHandler(this.btnVacuum_Click);
            // 
            // grpUpdate
            // 
            this.grpUpdate.Controls.Add(this.lblScriptPath);
            this.grpUpdate.Controls.Add(this.txtScriptPath);
            this.grpUpdate.Controls.Add(this.btnSelectScript);
            this.grpUpdate.Controls.Add(this.lblUpdateDesc);
            this.grpUpdate.Controls.Add(this.btnUpdateStructure);
            this.grpUpdate.Location = new System.Drawing.Point(23, 41);
            this.grpUpdate.Name = "grpUpdate";
            this.grpUpdate.Size = new System.Drawing.Size(722, 210);
            this.grpUpdate.TabIndex = 5;
            this.grpUpdate.TabStop = false;
            this.grpUpdate.Text = "Обновление структуры БД";
            // 
            // lblScriptPath
            // 
            this.lblScriptPath.AutoSize = true;
            this.lblScriptPath.Location = new System.Drawing.Point(18, 30);
            this.lblScriptPath.Name = "lblScriptPath";
            this.lblScriptPath.Size = new System.Drawing.Size(123, 21);
            this.lblScriptPath.TabIndex = 4;
            this.lblScriptPath.Text = "Файл скрипта:";
            // 
            // txtScriptPath
            // 
            this.txtScriptPath.BackColor = System.Drawing.SystemColors.Window;
            this.txtScriptPath.Location = new System.Drawing.Point(22, 55);
            this.txtScriptPath.Name = "txtScriptPath";
            this.txtScriptPath.ReadOnly = true;
            this.txtScriptPath.Size = new System.Drawing.Size(545, 29);
            this.txtScriptPath.TabIndex = 5;
            // 
            // btnSelectScript
            // 
            this.btnSelectScript.Image = global::ChurchBudget.Properties.Resources.search;
            this.btnSelectScript.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelectScript.Location = new System.Drawing.Point(576, 52);
            this.btnSelectScript.Name = "btnSelectScript";
            this.btnSelectScript.Size = new System.Drawing.Size(140, 33);
            this.btnSelectScript.TabIndex = 6;
            this.btnSelectScript.Text = "Выбрать";
            this.btnSelectScript.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSelectScript.UseVisualStyleBackColor = true;
            this.btnSelectScript.Click += new System.EventHandler(this.btnSelectScript_Click);
            // 
            // lblUpdateDesc
            // 
            this.lblUpdateDesc.AutoSize = true;
            this.lblUpdateDesc.Location = new System.Drawing.Point(22, 170);
            this.lblUpdateDesc.Name = "lblUpdateDesc";
            this.lblUpdateDesc.Size = new System.Drawing.Size(545, 21);
            this.lblUpdateDesc.TabIndex = 2;
            this.lblUpdateDesc.Text = "Перед применением автоматически создаётся резервная копия БД";
            // 
            // btnUpdateStructure
            // 
            this.btnUpdateStructure.Image = global::ChurchBudget.Properties.Resources.refresh;
            this.btnUpdateStructure.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateStructure.Location = new System.Drawing.Point(22, 100);
            this.btnUpdateStructure.Name = "btnUpdateStructure";
            this.btnUpdateStructure.Size = new System.Drawing.Size(333, 45);
            this.btnUpdateStructure.TabIndex = 0;
            this.btnUpdateStructure.Text = "Применить скрипт обновления";
            this.btnUpdateStructure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdateStructure.UseVisualStyleBackColor = true;
            this.btnUpdateStructure.Click += new System.EventHandler(this.btnUpdateStructure_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(23, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(283, 22);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Обслуживание базы данных";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tabPage2.Controls.Add(this.dgvLogHistory);
            this.tabPage2.Controls.Add(this.flowLayoutPanelLog);
            this.tabPage2.Controls.Add(this.lblLogInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 30);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(770, 487);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "История операций";
            // 
            // dgvLogHistory
            // 
            this.dgvLogHistory.AllowUserToAddRows = false;
            this.dgvLogHistory.AllowUserToDeleteRows = false;
            this.dgvLogHistory.AllowUserToResizeRows = false;
            this.dgvLogHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogHistory.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLogHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvLogHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogHistory.Location = new System.Drawing.Point(9, 6);
            this.dgvLogHistory.Name = "dgvLogHistory";
            this.dgvLogHistory.ReadOnly = true;
            this.dgvLogHistory.RowHeadersVisible = false;
            this.dgvLogHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogHistory.Size = new System.Drawing.Size(751, 380);
            this.dgvLogHistory.TabIndex = 0;
            // 
            // flowLayoutPanelLog
            // 
            this.flowLayoutPanelLog.Controls.Add(this.btnRefreshLog);
            this.flowLayoutPanelLog.Controls.Add(this.btnClearLog);
            this.flowLayoutPanelLog.Location = new System.Drawing.Point(9, 392);
            this.flowLayoutPanelLog.Name = "flowLayoutPanelLog";
            this.flowLayoutPanelLog.Size = new System.Drawing.Size(751, 45);
            this.flowLayoutPanelLog.TabIndex = 2;
            // 
            // btnRefreshLog
            // 
            this.btnRefreshLog.Image = global::ChurchBudget.Properties.Resources.refresh;
            this.btnRefreshLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshLog.Location = new System.Drawing.Point(3, 3);
            this.btnRefreshLog.Name = "btnRefreshLog";
            this.btnRefreshLog.Size = new System.Drawing.Size(160, 40);
            this.btnRefreshLog.TabIndex = 0;
            this.btnRefreshLog.Text = "Обновить";
            this.btnRefreshLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefreshLog.UseVisualStyleBackColor = true;
            this.btnRefreshLog.Click += new System.EventHandler(this.btnRefreshLog_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Image = global::ChurchBudget.Properties.Resources.delete;
            this.btnClearLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearLog.Location = new System.Drawing.Point(169, 3);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(160, 40);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Очистить журнал";
            this.btnClearLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // lblLogInfo
            // 
            this.lblLogInfo.AutoSize = true;
            this.lblLogInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblLogInfo.Location = new System.Drawing.Point(9, 445);
            this.lblLogInfo.Name = "lblLogInfo";
            this.lblLogInfo.Size = new System.Drawing.Size(137, 19);
            this.lblLogInfo.TabIndex = 1;
            this.lblLogInfo.Text = "Загрузка истории...";
            // 
            // DbUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(778, 571);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbUpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Обслуживание базы данных";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.grpCheck.ResumeLayout(false);
            this.grpCheck.PerformLayout();
            this.grpOptimize.ResumeLayout(false);
            this.grpOptimize.PerformLayout();
            this.grpUpdate.ResumeLayout(false);
            this.grpUpdate.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogHistory)).EndInit();
            this.flowLayoutPanelLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox grpCheck;
        private System.Windows.Forms.Label lblCheckDesc;
        private System.Windows.Forms.Button btnCheckIntegrity;
        private System.Windows.Forms.GroupBox grpOptimize;
        private System.Windows.Forms.Label lblVacuumDesc;
        private System.Windows.Forms.Button btnVacuum;
        private System.Windows.Forms.GroupBox grpUpdate;
        private System.Windows.Forms.Label lblUpdateDesc;
        private System.Windows.Forms.Button btnUpdateStructure;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvLogHistory;
        private System.Windows.Forms.Label lblLogInfo;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLog;
        private System.Windows.Forms.Button btnRefreshLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label lblScriptPath;
        private System.Windows.Forms.TextBox txtScriptPath;
        private System.Windows.Forms.Button btnSelectScript;
    }
}