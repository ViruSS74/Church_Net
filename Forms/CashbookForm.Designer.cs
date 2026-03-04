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
            this.pButtons = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.lblPreview = new System.Windows.Forms.Label();
            this.tabCashbook.SuspendLayout();
            this.tabTitul.SuspendLayout();
            this.pButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCashbook
            // 
            this.tabCashbook.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCashbook.Controls.Add(this.tabTitul);
            this.tabCashbook.Controls.Add(this.tab1day);
            this.tabCashbook.Controls.Add(this.tabP2_31day);
            this.tabCashbook.Location = new System.Drawing.Point(0, 0);
            this.tabCashbook.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabCashbook.Name = "tabCashbook";
            this.tabCashbook.SelectedIndex = 0;
            this.tabCashbook.Size = new System.Drawing.Size(1575, 676);
            this.tabCashbook.TabIndex = 4;
            // 
            // tabTitul
            // 
            this.tabTitul.Controls.Add(this.lblPreview);
            this.tabTitul.Location = new System.Drawing.Point(4, 30);
            this.tabTitul.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabTitul.Name = "tabTitul";
            this.tabTitul.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabTitul.Size = new System.Drawing.Size(1567, 642);
            this.tabTitul.TabIndex = 0;
            this.tabTitul.Text = "Титульный";
            this.tabTitul.UseVisualStyleBackColor = true;
            // 
            // tab1day
            // 
            this.tab1day.Location = new System.Drawing.Point(4, 26);
            this.tab1day.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tab1day.Name = "tab1day";
            this.tab1day.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tab1day.Size = new System.Drawing.Size(1567, 646);
            this.tab1day.TabIndex = 1;
            this.tab1day.Text = "1";
            this.tab1day.UseVisualStyleBackColor = true;
            // 
            // tabP2_31day
            // 
            this.tabP2_31day.Location = new System.Drawing.Point(4, 30);
            this.tabP2_31day.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabP2_31day.Name = "tabP2_31day";
            this.tabP2_31day.Size = new System.Drawing.Size(1567, 642);
            this.tabP2_31day.TabIndex = 2;
            this.tabP2_31day.Text = "2";
            this.tabP2_31day.UseVisualStyleBackColor = true;
            // 
            // pButtons
            // 
            this.pButtons.Controls.Add(this.btnPrint);
            this.pButtons.Controls.Add(this.btnClose);
            this.pButtons.Controls.Add(this.btnView);
            this.pButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pButtons.Location = new System.Drawing.Point(0, 683);
            this.pButtons.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pButtons.Name = "pButtons";
            this.pButtons.Size = new System.Drawing.Size(1578, 158);
            this.pButtons.TabIndex = 5;
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::ChurchBudget.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(314, 48);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2, 6, 2, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(219, 62);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Печать";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ChurchBudget.Properties.Resources.exit;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(1684, 48);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 6, 2, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(219, 62);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Image = global::ChurchBudget.Properties.Resources.search;
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(30, 48);
            this.btnView.Margin = new System.Windows.Forms.Padding(2, 6, 2, 6);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(219, 62);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "Просмотр";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPreview.ForeColor = System.Drawing.Color.Red;
            this.lblPreview.Location = new System.Drawing.Point(417, 277);
            this.lblPreview.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(411, 31);
            this.lblPreview.TabIndex = 4;
            this.lblPreview.Text = "Форма находится в разработке";
            // 
            // CashbookForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1578, 841);
            this.Controls.Add(this.pButtons);
            this.Controls.Add(this.tabCashbook);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "CashbookForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Кассовая книга";
            this.tabCashbook.ResumeLayout(false);
            this.tabTitul.ResumeLayout(false);
            this.tabTitul.PerformLayout();
            this.pButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabCashbook;
        private System.Windows.Forms.TabPage tabTitul;
        private System.Windows.Forms.TabPage tab1day;
        private System.Windows.Forms.TabPage tabP2_31day;
        private System.Windows.Forms.Panel pButtons;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label lblPreview;
    }
}