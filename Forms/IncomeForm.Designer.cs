namespace ChurchBudget.Forms
{
    partial class IncomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncomeForm));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.dtpDocDate = new System.Windows.Forms.DateTimePicker();
            this.lblDateTitl = new System.Windows.Forms.Label();
            this.txtDocNumber = new System.Windows.Forms.TextBox();
            this.lblDocNumberTitle = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNewDoc = new System.Windows.Forms.Button();
            this.btnSaveDoc = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvCategories = new System.Windows.Forms.TreeView();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeader.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.dtpDocDate);
            this.pnlHeader.Controls.Add(this.lblDateTitl);
            this.pnlHeader.Controls.Add(this.txtDocNumber);
            this.pnlHeader.Controls.Add(this.lblDocNumberTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(3, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1261, 81);
            this.pnlHeader.TabIndex = 0;
            // 
            // dtpDocDate
            // 
            this.dtpDocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpDocDate.Location = new System.Drawing.Point(221, 35);
            this.dtpDocDate.Margin = new System.Windows.Forms.Padding(2);
            this.dtpDocDate.Name = "dtpDocDate";
            this.dtpDocDate.Size = new System.Drawing.Size(151, 23);
            this.dtpDocDate.TabIndex = 3;
            // 
            // lblDateTitl
            // 
            this.lblDateTitl.AutoSize = true;
            this.lblDateTitl.Font = new System.Drawing.Font("Microsoft YaHei", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDateTitl.Location = new System.Drawing.Point(218, 8);
            this.lblDateTitl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDateTitl.Name = "lblDateTitl";
            this.lblDateTitl.Size = new System.Drawing.Size(54, 22);
            this.lblDateTitl.TabIndex = 2;
            this.lblDateTitl.Text = "Дата:";
            // 
            // txtDocNumber
            // 
            this.txtDocNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtDocNumber.Location = new System.Drawing.Point(24, 35);
            this.txtDocNumber.Margin = new System.Windows.Forms.Padding(2);
            this.txtDocNumber.Name = "txtDocNumber";
            this.txtDocNumber.Size = new System.Drawing.Size(165, 23);
            this.txtDocNumber.TabIndex = 1;
            // 
            // lblDocNumberTitle
            // 
            this.lblDocNumberTitle.AutoSize = true;
            this.lblDocNumberTitle.Font = new System.Drawing.Font("Microsoft YaHei", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDocNumberTitle.Location = new System.Drawing.Point(21, 8);
            this.lblDocNumberTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDocNumberTitle.Name = "lblDocNumberTitle";
            this.lblDocNumberTitle.Size = new System.Drawing.Size(168, 22);
            this.lblDocNumberTitle.TabIndex = 0;
            this.lblDocNumberTitle.Text = "Номер документа:";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotal.Location = new System.Drawing.Point(743, 594);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(150, 24);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Итого: 0.00 руб.";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(310, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNewDoc
            // 
            this.btnNewDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewDoc.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNewDoc.Image = global::ChurchBudget.Properties.Resources.add;
            this.btnNewDoc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewDoc.Location = new System.Drawing.Point(146, 2);
            this.btnNewDoc.Margin = new System.Windows.Forms.Padding(2);
            this.btnNewDoc.Name = "btnNewDoc";
            this.btnNewDoc.Size = new System.Drawing.Size(160, 40);
            this.btnNewDoc.TabIndex = 2;
            this.btnNewDoc.Text = "Новый документ";
            this.btnNewDoc.UseVisualStyleBackColor = true;
            this.btnNewDoc.Click += new System.EventHandler(this.btnNewDoc_Click);
            // 
            // btnSaveDoc
            // 
            this.btnSaveDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveDoc.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDoc.Image = global::ChurchBudget.Properties.Resources.save;
            this.btnSaveDoc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveDoc.Location = new System.Drawing.Point(2, 2);
            this.btnSaveDoc.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveDoc.Name = "btnSaveDoc";
            this.btnSaveDoc.Size = new System.Drawing.Size(140, 40);
            this.btnSaveDoc.TabIndex = 1;
            this.btnSaveDoc.Text = "Сохранить";
            this.btnSaveDoc.UseVisualStyleBackColor = true;
            this.btnSaveDoc.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // splitMain
            // 
            this.splitMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Margin = new System.Windows.Forms.Padding(2);
            this.splitMain.Name = "splitMain";
            this.splitMain.Size = new System.Drawing.Size(3, 681);
            this.splitMain.TabIndex = 2;
            this.splitMain.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(9, 89);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvCategories);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvItems);
            this.splitContainer1.Size = new System.Drawing.Size(1251, 508);
            this.splitContainer1.SplitterDistance = 477;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 3;
            // 
            // tvCategories
            // 
            this.tvCategories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCategories.Location = new System.Drawing.Point(0, 0);
            this.tvCategories.Margin = new System.Windows.Forms.Padding(2);
            this.tvCategories.Name = "tvCategories";
            this.tvCategories.Size = new System.Drawing.Size(477, 508);
            this.tvCategories.TabIndex = 0;
            this.tvCategories.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvCategories_NodeMouseDoubleClick);
            // 
            // dgvItems
            // 
            this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategory,
            this.colAmount,
            this.colDescription});
            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.Location = new System.Drawing.Point(0, 0);
            this.dgvItems.Margin = new System.Windows.Forms.Padding(2);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersWidth = 51;
            this.dgvItems.RowTemplate.Height = 24;
            this.dgvItems.Size = new System.Drawing.Size(771, 508);
            this.dgvItems.TabIndex = 0;
            // 
            // colCategory
            // 
            this.colCategory.HeaderText = "Категория";
            this.colCategory.MinimumWidth = 8;
            this.colCategory.Name = "colCategory";
            this.colCategory.Width = 125;
            // 
            // colAmount
            // 
            this.colAmount.HeaderText = "Сумма";
            this.colAmount.MinimumWidth = 8;
            this.colAmount.Name = "colAmount";
            this.colAmount.Width = 125;
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "Примечание";
            this.colDescription.MinimumWidth = 8;
            this.colDescription.Name = "colDescription";
            this.colDescription.Width = 125;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnSaveDoc);
            this.flowLayoutPanel1.Controls.Add(this.btnNewDoc);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 621);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1261, 60);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // IncomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitMain);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "IncomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форма доходов";
            this.Load += new System.EventHandler(this.IncomeForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblDocNumberTitle;
        private System.Windows.Forms.TextBox txtDocNumber;
        private System.Windows.Forms.DateTimePicker dtpDocDate;
        private System.Windows.Forms.Label lblDateTitl;
        private System.Windows.Forms.Splitter splitMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvCategories;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnNewDoc;
        private System.Windows.Forms.Button btnSaveDoc;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}