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
            this.PanelButtons = new System.Windows.Forms.Panel();
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
            this.PanelButtons.SuspendLayout();
            this.tabPKO.SuspendLayout();
            this.tabPrintForm.SuspendLayout();
            this.tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelButtons
            // 
            this.PanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelButtons.Controls.Add(this.btnPrint);
            this.PanelButtons.Controls.Add(this.btnClose);
            this.PanelButtons.Controls.Add(this.btnView);
            this.PanelButtons.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PanelButtons.Location = new System.Drawing.Point(2, 726);
            this.PanelButtons.Margin = new System.Windows.Forms.Padding(4);
            this.PanelButtons.Name = "PanelButtons";
            this.PanelButtons.Size = new System.Drawing.Size(1260, 125);
            this.PanelButtons.TabIndex = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::ChurchBudget.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(200, 42);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
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
            this.btnClose.Location = new System.Drawing.Point(1103, 42);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
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
            this.btnView.Location = new System.Drawing.Point(18, 42);
            this.btnView.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
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
            this.tabPKO.Name = "tabPKO";
            this.tabPKO.SelectedIndex = 0;
            this.tabPKO.Size = new System.Drawing.Size(1262, 853);
            this.tabPKO.TabIndex = 1;
            // 
            // tabPrintForm
            // 
            this.tabPrintForm.BackColor = System.Drawing.Color.White;
            this.tabPrintForm.Controls.Add(this.ppControl);
            this.tabPrintForm.Location = new System.Drawing.Point(4, 29);
            this.tabPrintForm.Name = "tabPrintForm";
            this.tabPrintForm.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrintForm.Size = new System.Drawing.Size(1254, 820);
            this.tabPrintForm.TabIndex = 0;
            this.tabPrintForm.Text = "Печатная форма";
            // 
            // ppControl
            // 
            this.ppControl.AutoZoom = false;
            this.ppControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppControl.Document = this.printPKOTitle;
            this.ppControl.Location = new System.Drawing.Point(3, 3);
            this.ppControl.Name = "ppControl";
            this.ppControl.Size = new System.Drawing.Size(1248, 814);
            this.ppControl.TabIndex = 0;
            this.ppControl.Zoom = 1D;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.dgvData);
            this.tabData.Location = new System.Drawing.Point(4, 29);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new System.Windows.Forms.Padding(3);
            this.tabData.Size = new System.Drawing.Size(1252, 686);
            this.tabData.TabIndex = 1;
            this.tabData.Text = "Данные";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(6, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersWidth = 51;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.Size = new System.Drawing.Size(1246, 676);
            this.dgvData.TabIndex = 0;
            // 
            // tabEdit
            // 
            this.tabEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabEdit.Location = new System.Drawing.Point(4, 29);
            this.tabEdit.Name = "tabEdit";
            this.tabEdit.Size = new System.Drawing.Size(1254, 820);
            this.tabEdit.TabIndex = 2;
            this.tabEdit.Text = "Редактирование";
            this.tabEdit.UseVisualStyleBackColor = true;
            // 
            // OrderInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1262, 853);
            this.Controls.Add(this.PanelButtons);
            this.Controls.Add(this.tabPKO);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OrderInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форма бланка ПКО";
            this.PanelButtons.ResumeLayout(false);
            this.tabPKO.ResumeLayout(false);
            this.tabPrintForm.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelButtons;
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
    }
}