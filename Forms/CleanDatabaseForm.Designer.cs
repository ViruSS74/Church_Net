namespace ChurchBudget.Forms
{
    partial class CleanDatabaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanDatabaseForm));
            this.lblMode = new System.Windows.Forms.Label();
            this.rbDocumentsOnly = new System.Windows.Forms.RadioButton();
            this.rbDirectories = new System.Windows.Forms.RadioButton();
            this.rbFullClean = new System.Windows.Forms.RadioButton();
            this.lblDetails = new System.Windows.Forms.Label();
            this.chkIncomeDocs = new System.Windows.Forms.CheckBox();
            this.chkExpenseDocs = new System.Windows.Forms.CheckBox();
            this.chkCashOrders = new System.Windows.Forms.CheckBox();
            this.chkPersonal = new System.Windows.Forms.CheckBox();
            this.chkIdDocs = new System.Windows.Forms.CheckBox();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCount = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkOrganizations = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMode.Location = new System.Drawing.Point(20, 7);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(137, 21);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "Режим очистки:";
            // 
            // rbDocumentsOnly
            // 
            this.rbDocumentsOnly.AutoSize = true;
            this.rbDocumentsOnly.Location = new System.Drawing.Point(24, 28);
            this.rbDocumentsOnly.Name = "rbDocumentsOnly";
            this.rbDocumentsOnly.Size = new System.Drawing.Size(259, 25);
            this.rbDocumentsOnly.TabIndex = 1;
            this.rbDocumentsOnly.TabStop = true;
            this.rbDocumentsOnly.Text = "Только документы (безопасно)  ";
            this.rbDocumentsOnly.UseVisualStyleBackColor = true;
            this.rbDocumentsOnly.Click += new System.EventHandler(this.rbDocumentsOnly_CheckedChanged);
            // 
            // rbDirectories
            // 
            this.rbDirectories.AutoSize = true;
            this.rbDirectories.Location = new System.Drawing.Point(24, 54);
            this.rbDirectories.Name = "rbDirectories";
            this.rbDirectories.Size = new System.Drawing.Size(224, 25);
            this.rbDirectories.TabIndex = 2;
            this.rbDirectories.TabStop = true;
            this.rbDirectories.Text = "Справочники + документы";
            this.rbDirectories.UseVisualStyleBackColor = true;
            this.rbDirectories.Click += new System.EventHandler(this.rbDirectories_CheckedChanged);
            // 
            // rbFullClean
            // 
            this.rbFullClean.AutoSize = true;
            this.rbFullClean.Location = new System.Drawing.Point(24, 84);
            this.rbFullClean.Name = "rbFullClean";
            this.rbFullClean.Size = new System.Drawing.Size(227, 25);
            this.rbFullClean.TabIndex = 3;
            this.rbFullClean.TabStop = true;
            this.rbFullClean.Text = "Полная очистка (ОПАСНО!)";
            this.rbFullClean.UseVisualStyleBackColor = true;
            this.rbFullClean.Click += new System.EventHandler(this.rbFullClean_CheckedChanged);
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDetails.Location = new System.Drawing.Point(19, 127);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(184, 21);
            this.lblDetails.TabIndex = 4;
            this.lblDetails.Text = "Детальная настройка:";
            // 
            // chkIncomeDocs
            // 
            this.chkIncomeDocs.AutoSize = true;
            this.chkIncomeDocs.Location = new System.Drawing.Point(24, 153);
            this.chkIncomeDocs.Name = "chkIncomeDocs";
            this.chkIncomeDocs.Size = new System.Drawing.Size(179, 25);
            this.chkIncomeDocs.TabIndex = 5;
            this.chkIncomeDocs.Text = "Документы доходов ";
            this.chkIncomeDocs.UseVisualStyleBackColor = true;
            // 
            // chkExpenseDocs
            // 
            this.chkExpenseDocs.AutoSize = true;
            this.chkExpenseDocs.Location = new System.Drawing.Point(24, 177);
            this.chkExpenseDocs.Name = "chkExpenseDocs";
            this.chkExpenseDocs.Size = new System.Drawing.Size(181, 25);
            this.chkExpenseDocs.TabIndex = 6;
            this.chkExpenseDocs.Text = "Документы расходов";
            this.chkExpenseDocs.UseVisualStyleBackColor = true;
            // 
            // chkCashOrders
            // 
            this.chkCashOrders.AutoSize = true;
            this.chkCashOrders.Location = new System.Drawing.Point(24, 202);
            this.chkCashOrders.Name = "chkCashOrders";
            this.chkCashOrders.Size = new System.Drawing.Size(156, 25);
            this.chkCashOrders.TabIndex = 7;
            this.chkCashOrders.Text = "Кассовые ордера ";
            this.chkCashOrders.UseVisualStyleBackColor = true;
            // 
            // chkPersonal
            // 
            this.chkPersonal.AutoSize = true;
            this.chkPersonal.Location = new System.Drawing.Point(24, 226);
            this.chkPersonal.Name = "chkPersonal";
            this.chkPersonal.Size = new System.Drawing.Size(116, 25);
            this.chkPersonal.TabIndex = 8;
            this.chkPersonal.Text = "Сотрудники";
            this.chkPersonal.UseVisualStyleBackColor = true;
            // 
            // chkIdDocs
            // 
            this.chkIdDocs.AutoSize = true;
            this.chkIdDocs.Location = new System.Drawing.Point(24, 275);
            this.chkIdDocs.Name = "chkIdDocs";
            this.chkIdDocs.Size = new System.Drawing.Size(136, 25);
            this.chkIdDocs.TabIndex = 9;
            this.chkIdDocs.Text = "ИД документы";
            this.chkIdDocs.UseVisualStyleBackColor = true;
            // 
            // lblConfirm
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConfirm.Location = new System.Drawing.Point(24, 318);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(335, 21);
            this.lblConfirm.TabIndex = 10;
            this.lblConfirm.Text = "Подтверждение (введите \"УДАЛИТЬ\"):    ";
            // 
            // txtConfirm
            // 
            this.txtConfirm.Location = new System.Drawing.Point(28, 348);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(144, 29);
            this.txtConfirm.TabIndex = 11;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLog.Location = new System.Drawing.Point(28, 404);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(133, 21);
            this.lblLog.TabIndex = 12;
            this.lblLog.Text = "Лог операций:  ";
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(28, 429);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(544, 166);
            this.rtbLog.TabIndex = 13;
            this.rtbLog.Text = "";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnCount);
            this.flowLayoutPanel1.Controls.Add(this.btnClean);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 611);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(584, 50);
            this.flowLayoutPanel1.TabIndex = 14;
            // 
            // btnCount
            // 
            this.btnCount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCount.Image = global::ChurchBudget.Properties.Resources.check;
            this.btnCount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCount.Location = new System.Drawing.Point(3, 3);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(140, 40);
            this.btnCount.TabIndex = 0;
            this.btnCount.Text = "Подсчитать";
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // btnClean
            // 
            this.btnClean.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClean.Image = global::ChurchBudget.Properties.Resources.clean_db;
            this.btnClean.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClean.Location = new System.Drawing.Point(149, 3);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(140, 40);
            this.btnClean.TabIndex = 1;
            this.btnClean.Text = "Очистить";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(295, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 40);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkOrganizations
            // 
            this.chkOrganizations.AutoSize = true;
            this.chkOrganizations.Location = new System.Drawing.Point(24, 252);
            this.chkOrganizations.Name = "chkOrganizations";
            this.chkOrganizations.Size = new System.Drawing.Size(125, 25);
            this.chkOrganizations.TabIndex = 15;
            this.chkOrganizations.Text = "Организации";
            this.chkOrganizations.UseVisualStyleBackColor = true;
            // 
            // CleanDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(584, 661);
            this.Controls.Add(this.chkOrganizations);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.txtConfirm);
            this.Controls.Add(this.lblConfirm);
            this.Controls.Add(this.chkIdDocs);
            this.Controls.Add(this.chkPersonal);
            this.Controls.Add(this.chkCashOrders);
            this.Controls.Add(this.chkExpenseDocs);
            this.Controls.Add(this.chkIncomeDocs);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.rbFullClean);
            this.Controls.Add(this.rbDirectories);
            this.Controls.Add(this.rbDocumentsOnly);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CleanDatabaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Очистка базы данных";
            this.Load += new System.EventHandler(this.CleanDatabaseForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.RadioButton rbDocumentsOnly;
        private System.Windows.Forms.RadioButton rbDirectories;
        private System.Windows.Forms.RadioButton rbFullClean;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.CheckBox chkIncomeDocs;
        private System.Windows.Forms.CheckBox chkExpenseDocs;
        private System.Windows.Forms.CheckBox chkCashOrders;
        private System.Windows.Forms.CheckBox chkPersonal;
        private System.Windows.Forms.CheckBox chkIdDocs;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkOrganizations;
    }
}