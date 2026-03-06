namespace ChurchBudget.Forms
{
    partial class OrderInForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderInForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.tabPKO = new System.Windows.Forms.TabControl();
            this.tabPrintForm = new System.Windows.Forms.TabPage();
            this.ppControl = new System.Windows.Forms.PrintPreviewControl();
            this.printPKOTitle = new System.Drawing.Printing.PrintDocument();
            this.tabData = new System.Windows.Forms.TabPage();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.tabEdit = new System.Windows.Forms.TabPage();
            this.txtEditAppendix = new System.Windows.Forms.TextBox();
            this.lblApendix = new System.Windows.Forms.Label();
            this.txtEditBasis = new System.Windows.Forms.TextBox();
            this.lblBasis = new System.Windows.Forms.Label();
            this.lblRecipient = new System.Windows.Forms.Label();
            this.cmbRecipient = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPKO.SuspendLayout();
            this.tabPrintForm.SuspendLayout();
            this.tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.tabEdit.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSave.Image = global::ChurchBudget.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(292, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 40);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
            // tabPKO
            // 
            this.tabPKO.Controls.Add(this.tabPrintForm);
            this.tabPKO.Controls.Add(this.tabData);
            this.tabPKO.Controls.Add(this.tabEdit);
            this.tabPKO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPKO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPKO.Location = new System.Drawing.Point(0, 0);
            this.tabPKO.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPKO.Name = "tabPKO";
            this.tabPKO.SelectedIndex = 0;
            this.tabPKO.Size = new System.Drawing.Size(1264, 681);
            this.tabPKO.TabIndex = 1;
            // 
            // tabPrintForm
            // 
            this.tabPrintForm.BackColor = System.Drawing.Color.White;
            this.tabPrintForm.Controls.Add(this.flowLayoutPanel1);
            this.tabPrintForm.Controls.Add(this.ppControl);
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
            this.ppControl.Document = this.printPKOTitle;
            this.ppControl.Location = new System.Drawing.Point(0, 0);
            this.ppControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ppControl.Name = "ppControl";
            this.ppControl.Size = new System.Drawing.Size(1260, 700);
            this.ppControl.TabIndex = 0;
            this.ppControl.Zoom = 1D;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.dgvData);
            this.tabData.Location = new System.Drawing.Point(4, 26);
            this.tabData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabData.Size = new System.Drawing.Size(1256, 651);
            this.tabData.TabIndex = 1;
            this.tabData.Text = "Данные";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersWidth = 51;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.Size = new System.Drawing.Size(1264, 650);
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
            this.tabEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
            this.txtEditAppendix.Location = new System.Drawing.Point(239, 163);
            this.txtEditAppendix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEditAppendix.Name = "txtEditAppendix";
            this.txtEditAppendix.Size = new System.Drawing.Size(254, 23);
            this.txtEditAppendix.TabIndex = 11;
            // 
            // lblApendix
            // 
            this.lblApendix.AutoSize = true;
            this.lblApendix.Location = new System.Drawing.Point(50, 167);
            this.lblApendix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApendix.Name = "lblApendix";
            this.lblApendix.Size = new System.Drawing.Size(101, 17);
            this.lblApendix.TabIndex = 10;
            this.lblApendix.Text = "Приложение";
            // 
            // txtEditBasis
            // 
            this.txtEditBasis.Location = new System.Drawing.Point(239, 85);
            this.txtEditBasis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEditBasis.Name = "txtEditBasis";
            this.txtEditBasis.Size = new System.Drawing.Size(984, 23);
            this.txtEditBasis.TabIndex = 9;
            // 
            // lblBasis
            // 
            this.lblBasis.AutoSize = true;
            this.lblBasis.Location = new System.Drawing.Point(50, 89);
            this.lblBasis.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBasis.Name = "lblBasis";
            this.lblBasis.Size = new System.Drawing.Size(90, 17);
            this.lblBasis.TabIndex = 8;
            this.lblBasis.Text = "Основание";
            // 
            // lblRecipient
            // 
            this.lblRecipient.AutoSize = true;
            this.lblRecipient.Location = new System.Drawing.Point(45, 30);
            this.lblRecipient.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecipient.Name = "lblRecipient";
            this.lblRecipient.Size = new System.Drawing.Size(94, 17);
            this.lblRecipient.TabIndex = 7;
            this.lblRecipient.Text = "Принято от";
            // 
            // cmbRecipient
            // 
            this.cmbRecipient.FormattingEnabled = true;
            this.cmbRecipient.Location = new System.Drawing.Point(239, 30);
            this.cmbRecipient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbRecipient.Name = "cmbRecipient";
            this.cmbRecipient.Size = new System.Drawing.Size(478, 25);
            this.cmbRecipient.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnView);
            this.flowLayoutPanel1.Controls.Add(this.btnPrint);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 587);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1248, 60);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // OrderInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.tabPKO);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "OrderInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форма бланка ПКО";
            this.tabPKO.ResumeLayout(false);
            this.tabPrintForm.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.tabEdit.ResumeLayout(false);
            this.tabEdit.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.TabControl tabPKO;
        private System.Windows.Forms.TabPage tabPrintForm;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Drawing.Printing.PrintDocument printPKOTitle;
        private System.Windows.Forms.PrintPreviewControl ppControl;
        private System.Windows.Forms.TabPage tabEdit;
        private System.Windows.Forms.TextBox txtEditAppendix;
        private System.Windows.Forms.Label lblApendix;
        private System.Windows.Forms.TextBox txtEditBasis;
        private System.Windows.Forms.Label lblBasis;
        private System.Windows.Forms.Label lblRecipient;
        private System.Windows.Forms.ComboBox cmbRecipient;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}