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
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.cmbPersonal = new System.Windows.Forms.ComboBox();
            this.lblMP = new System.Windows.Forms.Label();
            this.lblTreasurerLabel = new System.Windows.Forms.Label();
            this.PanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.cmbPersonal);
            this.PanelBottom.Controls.Add(this.lblMP);
            this.PanelBottom.Controls.Add(this.lblTreasurerLabel);
            this.PanelBottom.Location = new System.Drawing.Point(0, 687);
            this.PanelBottom.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(1578, 85);
            this.PanelBottom.TabIndex = 3;
            // 
            // cmbPersonal
            // 
            this.cmbPersonal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPersonal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbPersonal.FormattingEnabled = true;
            this.cmbPersonal.Location = new System.Drawing.Point(455, 6);
            this.cmbPersonal.Margin = new System.Windows.Forms.Padding(5);
            this.cmbPersonal.Name = "cmbPersonal";
            this.cmbPersonal.Size = new System.Drawing.Size(302, 28);
            this.cmbPersonal.TabIndex = 3;
            // 
            // lblMP
            // 
            this.lblMP.AutoSize = true;
            this.lblMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMP.Location = new System.Drawing.Point(26, 49);
            this.lblMP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMP.Name = "lblMP";
            this.lblMP.Size = new System.Drawing.Size(57, 25);
            this.lblMP.TabIndex = 2;
            this.lblMP.Text = "М.П.";
            // 
            // lblTreasurerLabel
            // 
            this.lblTreasurerLabel.AutoSize = true;
            this.lblTreasurerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTreasurerLabel.Location = new System.Drawing.Point(26, 11);
            this.lblTreasurerLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTreasurerLabel.Name = "lblTreasurerLabel";
            this.lblTreasurerLabel.Size = new System.Drawing.Size(270, 24);
            this.lblTreasurerLabel.TabIndex = 0;
            this.lblTreasurerLabel.Text = "Казначей:      ______________";
            this.lblTreasurerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DocPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.PanelBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "DocPreviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Формирование документа доходы/расходы";
            this.Load += new System.EventHandler(this.DocPreviewForm_Load);
            this.PanelBottom.ResumeLayout(false);
            this.PanelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Label lblMP;
        private System.Windows.Forms.Label lblTreasurerLabel;
        private System.Windows.Forms.ComboBox cmbPersonal;
    }
}