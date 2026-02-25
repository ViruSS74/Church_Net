namespace ChurchBudget.Forms
{
    partial class DocPreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocPreviewForm));
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.dgvPrint = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblActionType = new System.Windows.Forms.Label();
            this.lblDocDate = new System.Windows.Forms.Label();
            this.cmbDocType = new System.Windows.Forms.ComboBox();
            this.lblDocDateLabel = new System.Windows.Forms.Label();
            this.lblOrgDetails = new System.Windows.Forms.Label();
            this.lblOrgName = new System.Windows.Forms.Label();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.cmbPersonal = new System.Windows.Forms.ComboBox();
            this.lblMP = new System.Windows.Forms.Label();
            this.lblTreasurerLabel = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrint)).BeginInit();
            this.panel2.SuspendLayout();
            this.PanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.btnPrint);
            this.panelButtons.Controls.Add(this.btnView);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 753);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1262, 100);
            this.panelButtons.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(1110, 46);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 40);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::ChurchBudget.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(213, 46);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(140, 40);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Печать";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnView
            // 
            this.btnView.Image = global::ChurchBudget.Properties.Resources.search;
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(25, 46);
            this.btnView.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(140, 40);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "Просмотр";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dgvPrint
            // 
            this.dgvPrint.AllowUserToAddRows = false;
            this.dgvPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPrint.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrint.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPrint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrint.Location = new System.Drawing.Point(17, 211);
            this.dgvPrint.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.dgvPrint.Name = "dgvPrint";
            this.dgvPrint.RowHeadersVisible = false;
            this.dgvPrint.RowHeadersWidth = 51;
            this.dgvPrint.RowTemplate.Height = 24;
            this.dgvPrint.Size = new System.Drawing.Size(602, 401);
            this.dgvPrint.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblActionType);
            this.panel2.Controls.Add(this.lblDocDate);
            this.panel2.Controls.Add(this.cmbDocType);
            this.panel2.Controls.Add(this.lblDocDateLabel);
            this.panel2.Controls.Add(this.lblOrgDetails);
            this.panel2.Controls.Add(this.lblOrgName);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1260, 213);
            this.panel2.TabIndex = 2;
            // 
            // lblActionType
            // 
            this.lblActionType.AutoSize = true;
            this.lblActionType.Location = new System.Drawing.Point(13, 172);
            this.lblActionType.Name = "lblActionType";
            this.lblActionType.Size = new System.Drawing.Size(200, 20);
            this.lblActionType.TabIndex = 5;
            this.lblActionType.Text = "Принято: / Потрачено:";
            // 
            // lblDocDate
            // 
            this.lblDocDate.AutoSize = true;
            this.lblDocDate.Location = new System.Drawing.Point(47, 128);
            this.lblDocDate.Name = "lblDocDate";
            this.lblDocDate.Size = new System.Drawing.Size(233, 20);
            this.lblDocDate.TabIndex = 4;
            this.lblDocDate.Text = "Дата создания документа";
            // 
            // cmbDocType
            // 
            this.cmbDocType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbDocType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDocType.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbDocType.FormattingEnabled = true;
            this.cmbDocType.Items.AddRange(new object[] {
            "РАПОРТИЧКА",
            "Расходы"});
            this.cmbDocType.Location = new System.Drawing.Point(14, 75);
            this.cmbDocType.Name = "cmbDocType";
            this.cmbDocType.Size = new System.Drawing.Size(209, 37);
            this.cmbDocType.TabIndex = 3;
            // 
            // lblDocDateLabel
            // 
            this.lblDocDateLabel.AutoSize = true;
            this.lblDocDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDocDateLabel.Location = new System.Drawing.Point(11, 128);
            this.lblDocDateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDocDateLabel.Name = "lblDocDateLabel";
            this.lblDocDateLabel.Size = new System.Drawing.Size(33, 20);
            this.lblDocDateLabel.TabIndex = 2;
            this.lblDocDateLabel.Text = "за:";
            // 
            // lblOrgDetails
            // 
            this.lblOrgDetails.AutoSize = true;
            this.lblOrgDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOrgDetails.Location = new System.Drawing.Point(64, 43);
            this.lblOrgDetails.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOrgDetails.Name = "lblOrgDetails";
            this.lblOrgDetails.Size = new System.Drawing.Size(208, 20);
            this.lblOrgDetails.TabIndex = 1;
            this.lblOrgDetails.Text = "детали об организации";
            // 
            // lblOrgName
            // 
            this.lblOrgName.AutoSize = true;
            this.lblOrgName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOrgName.Location = new System.Drawing.Point(21, 12);
            this.lblOrgName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOrgName.Name = "lblOrgName";
            this.lblOrgName.Size = new System.Drawing.Size(241, 25);
            this.lblOrgName.TabIndex = 0;
            this.lblOrgName.Text = "Название организации";
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.cmbPersonal);
            this.PanelBottom.Controls.Add(this.lblMP);
            this.PanelBottom.Controls.Add(this.lblTreasurerLabel);
            this.PanelBottom.Location = new System.Drawing.Point(0, 663);
            this.PanelBottom.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(1262, 90);
            this.PanelBottom.TabIndex = 3;
            // 
            // cmbPersonal
            // 
            this.cmbPersonal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPersonal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbPersonal.FormattingEnabled = true;
            this.cmbPersonal.Location = new System.Drawing.Point(364, 6);
            this.cmbPersonal.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPersonal.Name = "cmbPersonal";
            this.cmbPersonal.Size = new System.Drawing.Size(242, 33);
            this.cmbPersonal.TabIndex = 3;
            // 
            // lblMP
            // 
            this.lblMP.AutoSize = true;
            this.lblMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMP.Location = new System.Drawing.Point(21, 48);
            this.lblMP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMP.Name = "lblMP";
            this.lblMP.Size = new System.Drawing.Size(86, 38);
            this.lblMP.TabIndex = 2;
            this.lblMP.Text = "М.П.";
            // 
            // lblTreasurerLabel
            // 
            this.lblTreasurerLabel.AutoSize = true;
            this.lblTreasurerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTreasurerLabel.Location = new System.Drawing.Point(21, 16);
            this.lblTreasurerLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTreasurerLabel.Name = "lblTreasurerLabel";
            this.lblTreasurerLabel.Size = new System.Drawing.Size(345, 29);
            this.lblTreasurerLabel.TabIndex = 0;
            this.lblTreasurerLabel.Text = "Казначей:      ______________";
            this.lblTreasurerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotal.Location = new System.Drawing.Point(497, 616);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(80, 25);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Итого:";
            // 
            // DocPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1262, 853);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgvPrint);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DocPreviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Формирование документа доходы/расходы";
            this.Load += new System.EventHandler(this.DocPreviewForm_Load);
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrint)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PanelBottom.ResumeLayout(false);
            this.PanelBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.DataGridView dgvPrint;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblOrgName;
        private System.Windows.Forms.Label lblDocDateLabel;
        private System.Windows.Forms.Label lblOrgDetails;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Label lblMP;
        private System.Windows.Forms.Label lblTreasurerLabel;
        private System.Windows.Forms.ComboBox cmbPersonal;
        private System.Windows.Forms.ComboBox cmbDocType;
        private System.Windows.Forms.Label lblDocDate;
        private System.Windows.Forms.Label lblActionType;
        private System.Windows.Forms.Label lblTotal;
    }
}