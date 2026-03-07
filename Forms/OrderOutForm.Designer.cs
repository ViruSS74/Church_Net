namespace ChurchBudget.Forms
{
    partial class OrderOutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderOutForm));
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.tabRKO = new System.Windows.Forms.TabControl();
            this.tabPrintForm = new System.Windows.Forms.TabPage();
            this.ppControl = new System.Windows.Forms.PrintPreviewControl();
            this.printRKOTitle = new System.Drawing.Printing.PrintDocument();
            this.tabData = new System.Windows.Forms.TabPage();
            this.lblAppendix = new System.Windows.Forms.Label();
            this.lblFIO = new System.Windows.Forms.Label();
            this.cmbPerson = new System.Windows.Forms.ComboBox();
            this.cmbDocs = new System.Windows.Forms.ComboBox();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.tabEdit = new System.Windows.Forms.TabPage();
            this.txtEditAppendix = new System.Windows.Forms.TextBox();
            this.lblApendix = new System.Windows.Forms.Label();
            this.txtEditBasis = new System.Windows.Forms.TextBox();
            this.lblBasis = new System.Windows.Forms.Label();
            this.lblRecipient = new System.Windows.Forms.Label();
            this.cmbRecipient = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabRKO.SuspendLayout();
            this.tabPrintForm.SuspendLayout();
            this.tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.tabEdit.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Image = global::ChurchBudget.Properties.Resources.save;
            this.btnSaveEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveEdit.Location = new System.Drawing.Point(292, 4);
            this.btnSaveEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(140, 40);
            this.btnSaveEdit.TabIndex = 6;
            this.btnSaveEdit.Text = "Сохранить";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::ChurchBudget.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(146, 5);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(140, 40);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Печать";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(438, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 40);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.Image = global::ChurchBudget.Properties.Resources.search;
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(2, 5);
            this.btnView.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(140, 40);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "Просмотр";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // tabRKO
            // 
            this.tabRKO.Controls.Add(this.tabPrintForm);
            this.tabRKO.Controls.Add(this.tabData);
            this.tabRKO.Controls.Add(this.tabEdit);
            this.tabRKO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRKO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabRKO.Location = new System.Drawing.Point(0, 0);
            this.tabRKO.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabRKO.Name = "tabRKO";
            this.tabRKO.SelectedIndex = 0;
            this.tabRKO.Size = new System.Drawing.Size(1264, 681);
            this.tabRKO.TabIndex = 3;
            // 
            // tabPrintForm
            // 
            this.tabPrintForm.BackColor = System.Drawing.Color.White;
            this.tabPrintForm.Controls.Add(this.ppControl);
            this.tabPrintForm.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPrintForm.Location = new System.Drawing.Point(4, 26);
            this.tabPrintForm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPrintForm.Name = "tabPrintForm";
            this.tabPrintForm.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPrintForm.Size = new System.Drawing.Size(1256, 651);
            this.tabPrintForm.TabIndex = 0;
            this.tabPrintForm.Text = "Печатная форма";
            // 
            // ppControl
            // 
            this.ppControl.AutoZoom = false;
            this.ppControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppControl.Document = this.printRKOTitle;
            this.ppControl.Location = new System.Drawing.Point(4, 4);
            this.ppControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ppControl.Name = "ppControl";
            this.ppControl.Size = new System.Drawing.Size(1248, 643);
            this.ppControl.TabIndex = 0;
            this.ppControl.Zoom = 1D;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.lblAppendix);
            this.tabData.Controls.Add(this.lblFIO);
            this.tabData.Controls.Add(this.cmbPerson);
            this.tabData.Controls.Add(this.cmbDocs);
            this.tabData.Controls.Add(this.dgvData);
            this.tabData.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabData.Location = new System.Drawing.Point(4, 26);
            this.tabData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabData.Size = new System.Drawing.Size(1256, 651);
            this.tabData.TabIndex = 1;
            this.tabData.Text = "Данные";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // lblAppendix
            // 
            this.lblAppendix.AutoSize = true;
            this.lblAppendix.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAppendix.Location = new System.Drawing.Point(579, 14);
            this.lblAppendix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAppendix.Name = "lblAppendix";
            this.lblAppendix.Size = new System.Drawing.Size(109, 21);
            this.lblAppendix.TabIndex = 4;
            this.lblAppendix.Text = "Приложение";
            // 
            // lblFIO
            // 
            this.lblFIO.AutoSize = true;
            this.lblFIO.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFIO.Location = new System.Drawing.Point(13, 18);
            this.lblFIO.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFIO.Name = "lblFIO";
            this.lblFIO.Size = new System.Drawing.Size(209, 21);
            this.lblFIO.TabIndex = 3;
            this.lblFIO.Text = "Фамилия Имя Отчество";
            // 
            // cmbPerson
            // 
            this.cmbPerson.FormattingEnabled = true;
            this.cmbPerson.Location = new System.Drawing.Point(233, 14);
            this.cmbPerson.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbPerson.Name = "cmbPerson";
            this.cmbPerson.Size = new System.Drawing.Size(292, 29);
            this.cmbPerson.TabIndex = 2;
            this.cmbPerson.SelectedIndexChanged += new System.EventHandler(this.cmbPerson_SelectedIndexChanged);
            // 
            // cmbDocs
            // 
            this.cmbDocs.FormattingEnabled = true;
            this.cmbDocs.Items.AddRange(new object[] {
            "без товарного чека",
            "товарный чек №",
            "накладная №"});
            this.cmbDocs.Location = new System.Drawing.Point(698, 10);
            this.cmbDocs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbDocs.Name = "cmbDocs";
            this.cmbDocs.Size = new System.Drawing.Size(205, 29);
            this.cmbDocs.TabIndex = 1;
            this.cmbDocs.SelectedIndexChanged += new System.EventHandler(this.cmbDocs_SelectedIndexChanged);
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(0, 63);
            this.dgvData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersWidth = 51;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.Size = new System.Drawing.Size(1264, 528);
            this.dgvData.TabIndex = 0;
            // 
            // tabEdit
            // 
            this.tabEdit.Controls.Add(this.txtEditAppendix);
            this.tabEdit.Controls.Add(this.lblApendix);
            this.tabEdit.Controls.Add(this.txtEditBasis);
            this.tabEdit.Controls.Add(this.lblBasis);
            this.tabEdit.Controls.Add(this.lblRecipient);
            this.tabEdit.Controls.Add(this.cmbRecipient);
            this.tabEdit.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabEdit.Location = new System.Drawing.Point(4, 26);
            this.tabEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabEdit.Name = "tabEdit";
            this.tabEdit.Size = new System.Drawing.Size(1256, 651);
            this.tabEdit.TabIndex = 2;
            this.tabEdit.Text = "Редактирование";
            this.tabEdit.UseVisualStyleBackColor = true;
            // 
            // txtEditAppendix
            // 
            this.txtEditAppendix.Location = new System.Drawing.Point(167, 172);
            this.txtEditAppendix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEditAppendix.Name = "txtEditAppendix";
            this.txtEditAppendix.Size = new System.Drawing.Size(350, 29);
            this.txtEditAppendix.TabIndex = 5;
            // 
            // lblApendix
            // 
            this.lblApendix.AutoSize = true;
            this.lblApendix.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblApendix.Location = new System.Drawing.Point(30, 175);
            this.lblApendix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApendix.Name = "lblApendix";
            this.lblApendix.Size = new System.Drawing.Size(109, 21);
            this.lblApendix.TabIndex = 4;
            this.lblApendix.Text = "Приложение";
            // 
            // txtEditBasis
            // 
            this.txtEditBasis.Location = new System.Drawing.Point(167, 94);
            this.txtEditBasis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEditBasis.Name = "txtEditBasis";
            this.txtEditBasis.Size = new System.Drawing.Size(984, 29);
            this.txtEditBasis.TabIndex = 3;
            // 
            // lblBasis
            // 
            this.lblBasis.AutoSize = true;
            this.lblBasis.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblBasis.Location = new System.Drawing.Point(30, 98);
            this.lblBasis.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBasis.Name = "lblBasis";
            this.lblBasis.Size = new System.Drawing.Size(94, 21);
            this.lblBasis.TabIndex = 2;
            this.lblBasis.Text = "Основание";
            // 
            // lblRecipient
            // 
            this.lblRecipient.AutoSize = true;
            this.lblRecipient.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRecipient.Location = new System.Drawing.Point(30, 38);
            this.lblRecipient.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecipient.Name = "lblRecipient";
            this.lblRecipient.Size = new System.Drawing.Size(110, 21);
            this.lblRecipient.TabIndex = 1;
            this.lblRecipient.Text = "Получатель";
            // 
            // cmbRecipient
            // 
            this.cmbRecipient.FormattingEnabled = true;
            this.cmbRecipient.Location = new System.Drawing.Point(167, 38);
            this.cmbRecipient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbRecipient.Name = "cmbRecipient";
            this.cmbRecipient.Size = new System.Drawing.Size(350, 29);
            this.cmbRecipient.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnView);
            this.flowLayoutPanel1.Controls.Add(this.btnPrint);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveEdit);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 621);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1264, 60);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // OrderOutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tabRKO);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "OrderOutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Расходно-кассовый ордер";
            this.tabRKO.ResumeLayout(false);
            this.tabPrintForm.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.tabEdit.ResumeLayout(false);
            this.tabEdit.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.TabControl tabRKO;
        private System.Windows.Forms.TabPage tabPrintForm;
        private System.Windows.Forms.PrintPreviewControl ppControl;
        private System.Drawing.Printing.PrintDocument printRKOTitle;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.TabPage tabEdit;
        private System.Windows.Forms.Label lblApendix;
        private System.Windows.Forms.TextBox txtEditBasis;
        private System.Windows.Forms.Label lblBasis;
        private System.Windows.Forms.Label lblRecipient;
        private System.Windows.Forms.ComboBox cmbRecipient;
        private System.Windows.Forms.Button btnSaveEdit;
        private System.Windows.Forms.TextBox txtEditAppendix;
        private System.Windows.Forms.ComboBox cmbDocs;
        private System.Windows.Forms.ComboBox cmbPerson;
        private System.Windows.Forms.Label lblFIO;
        private System.Windows.Forms.Label lblAppendix;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}