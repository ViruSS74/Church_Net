namespace ChurchBudget.Forms
{
    partial class ExpensesDocForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpensesDocForm));
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReceiptPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tvCategories = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pbReceipt = new System.Windows.Forms.PictureBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnNewDoc = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.Splitter();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveDoc = new System.Windows.Forms.Button();
            this.btnAttachReceipt = new System.Windows.Forms.Button();
            this.dtpDocDate = new System.Windows.Forms.DateTimePicker();
            this.lblDateTitl = new System.Windows.Forms.Label();
            this.txtDocNumber = new System.Windows.Forms.TextBox();
            this.lblDocNumberTitle = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItems
            // 
            this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategory,
            this.colAmount,
            this.colDescription,
            this.colReceiptPath});
            this.dgvItems.Location = new System.Drawing.Point(3, 0);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersWidth = 51;
            this.dgvItems.RowTemplate.Height = 24;
            this.dgvItems.Size = new System.Drawing.Size(614, 542);
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
            // colReceiptPath
            // 
            this.colReceiptPath.HeaderText = "Column1";
            this.colReceiptPath.MinimumWidth = 6;
            this.colReceiptPath.Name = "colReceiptPath";
            this.colReceiptPath.Visible = false;
            this.colReceiptPath.Width = 125;
            // 
            // tvCategories
            // 
            this.tvCategories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvCategories.Location = new System.Drawing.Point(3, 3);
            this.tvCategories.Name = "tvCategories";
            this.tvCategories.Size = new System.Drawing.Size(409, 536);
            this.tvCategories.TabIndex = 0;
            this.tvCategories.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvCategories_NodeMouseDoubleClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(5, 80);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvCategories);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pbReceipt);
            this.splitContainer1.Panel2.Controls.Add(this.dgvItems);
            this.splitContainer1.Size = new System.Drawing.Size(1238, 542);
            this.splitContainer1.SplitterDistance = 411;
            this.splitContainer1.TabIndex = 7;
            // 
            // pbReceipt
            // 
            this.pbReceipt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbReceipt.Location = new System.Drawing.Point(623, -13);
            this.pbReceipt.Name = "pbReceipt";
            this.pbReceipt.Size = new System.Drawing.Size(200, 200);
            this.pbReceipt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbReceipt.TabIndex = 1;
            this.pbReceipt.TabStop = false;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotal.Location = new System.Drawing.Point(667, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(150, 24);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Итого: 0.00 руб.";
            // 
            // btnNewDoc
            // 
            this.btnNewDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewDoc.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNewDoc.Image = global::ChurchBudget.Properties.Resources.add;
            this.btnNewDoc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewDoc.Location = new System.Drawing.Point(3, 3);
            this.btnNewDoc.Name = "btnNewDoc";
            this.btnNewDoc.Size = new System.Drawing.Size(180, 40);
            this.btnNewDoc.TabIndex = 2;
            this.btnNewDoc.Text = "Новый документ";
            this.btnNewDoc.UseVisualStyleBackColor = true;
            this.btnNewDoc.Click += new System.EventHandler(this.btnNewDoc_Click);
            // 
            // splitMain
            // 
            this.splitMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Size = new System.Drawing.Size(3, 621);
            this.splitMain.TabIndex = 6;
            this.splitMain.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(521, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveDoc
            // 
            this.btnSaveDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveDoc.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSaveDoc.Image = global::ChurchBudget.Properties.Resources.save;
            this.btnSaveDoc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveDoc.Location = new System.Drawing.Point(189, 3);
            this.btnSaveDoc.Name = "btnSaveDoc";
            this.btnSaveDoc.Size = new System.Drawing.Size(140, 40);
            this.btnSaveDoc.TabIndex = 1;
            this.btnSaveDoc.Text = "Сохранить";
            this.btnSaveDoc.UseVisualStyleBackColor = true;
            this.btnSaveDoc.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAttachReceipt
            // 
            this.btnAttachReceipt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAttachReceipt.Image = global::ChurchBudget.Properties.Resources.check;
            this.btnAttachReceipt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAttachReceipt.Location = new System.Drawing.Point(335, 3);
            this.btnAttachReceipt.Name = "btnAttachReceipt";
            this.btnAttachReceipt.Size = new System.Drawing.Size(180, 40);
            this.btnAttachReceipt.TabIndex = 5;
            this.btnAttachReceipt.Text = "Прикрепить чек";
            this.btnAttachReceipt.UseVisualStyleBackColor = true;
            this.btnAttachReceipt.Click += new System.EventHandler(this.btnAttachReceipt_Click);
            // 
            // dtpDocDate
            // 
            this.dtpDocDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpDocDate.Location = new System.Drawing.Point(248, 37);
            this.dtpDocDate.Name = "dtpDocDate";
            this.dtpDocDate.Size = new System.Drawing.Size(200, 29);
            this.dtpDocDate.TabIndex = 3;
            // 
            // lblDateTitl
            // 
            this.lblDateTitl.AutoSize = true;
            this.lblDateTitl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTitl.Location = new System.Drawing.Point(244, 10);
            this.lblDateTitl.Name = "lblDateTitl";
            this.lblDateTitl.Size = new System.Drawing.Size(51, 21);
            this.lblDateTitl.TabIndex = 2;
            this.lblDateTitl.Text = "Дата:";
            // 
            // txtDocNumber
            // 
            this.txtDocNumber.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtDocNumber.Location = new System.Drawing.Point(32, 38);
            this.txtDocNumber.Name = "txtDocNumber";
            this.txtDocNumber.Size = new System.Drawing.Size(177, 29);
            this.txtDocNumber.TabIndex = 1;
            // 
            // lblDocNumberTitle
            // 
            this.lblDocNumberTitle.AutoSize = true;
            this.lblDocNumberTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocNumberTitle.Location = new System.Drawing.Point(28, 10);
            this.lblDocNumberTitle.Name = "lblDocNumberTitle";
            this.lblDocNumberTitle.Size = new System.Drawing.Size(156, 21);
            this.lblDocNumberTitle.TabIndex = 0;
            this.lblDocNumberTitle.Text = "Номер документа:";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeader.Controls.Add(this.dtpDocDate);
            this.pnlHeader.Controls.Add(this.lblDateTitl);
            this.pnlHeader.Controls.Add(this.txtDocNumber);
            this.pnlHeader.Controls.Add(this.lblDocNumberTitle);
            this.pnlHeader.Location = new System.Drawing.Point(5, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1258, 75);
            this.pnlHeader.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnNewDoc);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveDoc);
            this.flowLayoutPanel1.Controls.Add(this.btnAttachReceipt);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Controls.Add(this.lblTotal);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 621);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1264, 60);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // ExpensesDocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ExpensesDocForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExpensesDocForm";
            this.Load += new System.EventHandler(this.ExpensesDocForm_Load);
            this.Click += new System.EventHandler(this.ExpensesDocForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.TreeView tvCategories;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Splitter splitMain;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnNewDoc;
        private System.Windows.Forms.Button btnSaveDoc;
        private System.Windows.Forms.DateTimePicker dtpDocDate;
        private System.Windows.Forms.Label lblDateTitl;
        private System.Windows.Forms.TextBox txtDocNumber;
        private System.Windows.Forms.Label lblDocNumberTitle;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pbReceipt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReceiptPath;
        private System.Windows.Forms.Button btnAttachReceipt;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}