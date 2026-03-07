namespace ChurchBudget.Forms
{
    partial class CashbookForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashbookForm));
            this.tabCashbook = new System.Windows.Forms.TabControl();
            this.tabTitul = new System.Windows.Forms.TabPage();
            this.tab1day = new System.Windows.Forms.TabPage();
            this.tabP2_31day = new System.Windows.Forms.TabPage();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAttention = new System.Windows.Forms.Label();
            this.tabCashbook.SuspendLayout();
            this.tabTitul.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCashbook
            // 
            this.tabCashbook.Controls.Add(this.tabTitul);
            this.tabCashbook.Controls.Add(this.tab1day);
            this.tabCashbook.Controls.Add(this.tabP2_31day);
            this.tabCashbook.Location = new System.Drawing.Point(0, 0);
            this.tabCashbook.Margin = new System.Windows.Forms.Padding(5);
            this.tabCashbook.Name = "tabCashbook";
            this.tabCashbook.SelectedIndex = 0;
            this.tabCashbook.Size = new System.Drawing.Size(1262, 616);
            this.tabCashbook.TabIndex = 4;
            // 
            // tabTitul
            // 
            this.tabTitul.Controls.Add(this.lblAttention);
            this.tabTitul.Location = new System.Drawing.Point(4, 30);
            this.tabTitul.Margin = new System.Windows.Forms.Padding(5);
            this.tabTitul.Name = "tabTitul";
            this.tabTitul.Padding = new System.Windows.Forms.Padding(5);
            this.tabTitul.Size = new System.Drawing.Size(1254, 582);
            this.tabTitul.TabIndex = 0;
            this.tabTitul.Text = "Титульный";
            this.tabTitul.UseVisualStyleBackColor = true;
            // 
            // tab1day
            // 
            this.tab1day.Location = new System.Drawing.Point(4, 30);
            this.tab1day.Margin = new System.Windows.Forms.Padding(5);
            this.tab1day.Name = "tab1day";
            this.tab1day.Padding = new System.Windows.Forms.Padding(5);
            this.tab1day.Size = new System.Drawing.Size(1254, 582);
            this.tab1day.TabIndex = 1;
            this.tab1day.Text = "1 день";
            this.tab1day.UseVisualStyleBackColor = true;
            // 
            // tabP2_31day
            // 
            this.tabP2_31day.Location = new System.Drawing.Point(4, 30);
            this.tabP2_31day.Margin = new System.Windows.Forms.Padding(5);
            this.tabP2_31day.Name = "tabP2_31day";
            this.tabP2_31day.Size = new System.Drawing.Size(1254, 582);
            this.tabP2_31day.TabIndex = 2;
            this.tabP2_31day.Text = "2 - 31 день";
            this.tabP2_31day.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::ChurchBudget.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(146, 6);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2, 6, 2, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(140, 40);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Печать";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Image = global::ChurchBudget.Properties.Resources.search;
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(2, 6);
            this.btnView.Margin = new System.Windows.Forms.Padding(2, 6, 2, 6);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(140, 40);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "Просмотр";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(290, 6);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 6, 2, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 40);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnView);
            this.flowLayoutPanel1.Controls.Add(this.btnPrint);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 621);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1264, 60);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // lblAttention
            // 
            this.lblAttention.AutoSize = true;
            this.lblAttention.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAttention.ForeColor = System.Drawing.Color.Red;
            this.lblAttention.Location = new System.Drawing.Point(384, 266);
            this.lblAttention.Name = "lblAttention";
            this.lblAttention.Size = new System.Drawing.Size(464, 37);
            this.lblAttention.TabIndex = 0;
            this.lblAttention.Text = "Форма находится в разработке!!!";
            // 
            // CashbookForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tabCashbook);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "CashbookForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Кассовая книга";
            this.tabCashbook.ResumeLayout(false);
            this.tabTitul.ResumeLayout(false);
            this.tabTitul.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabCashbook;
        private System.Windows.Forms.TabPage tabTitul;
        private System.Windows.Forms.TabPage tab1day;
        private System.Windows.Forms.TabPage tabP2_31day;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblAttention;
    }
}