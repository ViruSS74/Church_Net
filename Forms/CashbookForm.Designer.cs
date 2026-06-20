namespace ChurchBudget.Forms
{
    partial class CashbookForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashbookForm));
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblYear = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.lblMonth = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.pnlMarkers = new System.Windows.Forms.Panel();
            this.lblAttention = new System.Windows.Forms.Label();
            this.lblPink = new System.Windows.Forms.Label();
            this.lblPinkText = new System.Windows.Forms.Label();
            this.lblPagesBefore = new System.Windows.Forms.Label();
            this.chkLastInMonth = new System.Windows.Forms.CheckBox();
            this.lblBlueText = new System.Windows.Forms.Label();
            this.chkLastInYear = new System.Windows.Forms.CheckBox();
            this.lblGreenText = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlMarkers.SuspendLayout();
            this.SuspendLayout();
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 631);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1264, 50);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblYear.Location = new System.Drawing.Point(10, 15);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(41, 21);
            this.lblYear.TabIndex = 0;
            this.lblYear.Text = "Год:";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbYear.Location = new System.Drawing.Point(60, 12);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(80, 29);
            this.cmbYear.TabIndex = 1;
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMonth.Location = new System.Drawing.Point(150, 15);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(65, 21);
            this.lblMonth.TabIndex = 2;
            this.lblMonth.Text = "Месяц:";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbMonth.Location = new System.Drawing.Point(220, 12);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(150, 29);
            this.cmbMonth.TabIndex = 3;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.Image = global::ChurchBudget.Properties.Resources.search;
            this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerate.Location = new System.Drawing.Point(390, 10);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnGenerate.Size = new System.Drawing.Size(200, 40);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Сформировать";
            this.btnGenerate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPrev.Image = global::ChurchBudget.Properties.Resources.prev;
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.Location = new System.Drawing.Point(606, 9);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(180, 40);
            this.btnPrev.TabIndex = 5;
            this.btnPrev.Text = "Предыдущий";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPage.Location = new System.Drawing.Point(808, 20);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(120, 20);
            this.lblPage.TabIndex = 6;
            this.lblPage.Text = "Страница 1 из 1";
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNext.Image = global::ChurchBudget.Properties.Resources.next;
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNext.Location = new System.Drawing.Point(947, 9);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(180, 40);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "Следующий";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(10, 154);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(1244, 471);
            this.webBrowser.TabIndex = 8;
            // 
            // pnlMarkers
            // 
            this.pnlMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMarkers.BackColor = System.Drawing.Color.LightGray;
            this.pnlMarkers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMarkers.Controls.Add(this.lblAttention);
            this.pnlMarkers.Controls.Add(this.lblPink);
            this.pnlMarkers.Controls.Add(this.lblPinkText);
            this.pnlMarkers.Controls.Add(this.lblPagesBefore);
            this.pnlMarkers.Controls.Add(this.chkLastInMonth);
            this.pnlMarkers.Controls.Add(this.lblBlueText);
            this.pnlMarkers.Controls.Add(this.chkLastInYear);
            this.pnlMarkers.Controls.Add(this.lblGreenText);
            this.pnlMarkers.Location = new System.Drawing.Point(10, 60);
            this.pnlMarkers.Name = "pnlMarkers";
            this.pnlMarkers.Size = new System.Drawing.Size(1244, 95);
            this.pnlMarkers.TabIndex = 9;
            this.pnlMarkers.Visible = false;
            // 
            // lblAttention
            // 
            this.lblAttention.AutoSize = true;
            this.lblAttention.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAttention.ForeColor = System.Drawing.Color.DarkRed;
            this.lblAttention.Location = new System.Drawing.Point(5, 3);
            this.lblAttention.Name = "lblAttention";
            this.lblAttention.Size = new System.Drawing.Size(91, 15);
            this.lblAttention.TabIndex = 0;
            this.lblAttention.Text = "ВНИМАНИЕ !!!";
            // 
            // lblPink
            // 
            this.lblPink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(105)))), ((int)(((byte)(180)))));
            this.lblPink.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPink.Location = new System.Drawing.Point(5, 22);
            this.lblPink.Name = "lblPink";
            this.lblPink.Size = new System.Drawing.Size(20, 20);
            this.lblPink.TabIndex = 1;
            // 
            // lblPinkText
            // 
            this.lblPinkText.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblPinkText.Location = new System.Drawing.Point(30, 20);
            this.lblPinkText.MaximumSize = new System.Drawing.Size(450, 50);
            this.lblPinkText.Name = "lblPinkText";
            this.lblPinkText.Size = new System.Drawing.Size(450, 40);
            this.lblPinkText.TabIndex = 2;
            this.lblPinkText.Text = "В данную ячейку внесите номер последнего заполненного листа кассовой книги за пре" +
    "дыдущий месяц отчетного года";
            this.lblPinkText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPagesBefore
            // 
            this.lblPagesBefore.BackColor = System.Drawing.Color.White;
            this.lblPagesBefore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPagesBefore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPagesBefore.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblPagesBefore.Location = new System.Drawing.Point(10, 60);
            this.lblPagesBefore.Name = "lblPagesBefore";
            this.lblPagesBefore.Size = new System.Drawing.Size(30, 25);
            this.lblPagesBefore.TabIndex = 3;
            this.lblPagesBefore.Text = "0";
            this.lblPagesBefore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkLastInMonth
            // 
            this.chkLastInMonth.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLastInMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.chkLastInMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkLastInMonth.Location = new System.Drawing.Point(500, 22);
            this.chkLastInMonth.Name = "chkLastInMonth";
            this.chkLastInMonth.Size = new System.Drawing.Size(20, 20);
            this.chkLastInMonth.TabIndex = 4;
            this.chkLastInMonth.UseVisualStyleBackColor = false;
            // 
            // lblBlueText
            // 
            this.lblBlueText.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblBlueText.Location = new System.Drawing.Point(525, 22);
            this.lblBlueText.MaximumSize = new System.Drawing.Size(350, 40);
            this.lblBlueText.Name = "lblBlueText";
            this.lblBlueText.Size = new System.Drawing.Size(350, 35);
            this.lblBlueText.TabIndex = 5;
            this.lblBlueText.Text = "Поставьте Х если данный лист кассовой книги является последним в текущем месяце";
            // 
            // chkLastInYear
            // 
            this.chkLastInYear.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLastInYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))));
            this.chkLastInYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkLastInYear.Location = new System.Drawing.Point(500, 60);
            this.chkLastInYear.Name = "chkLastInYear";
            this.chkLastInYear.Size = new System.Drawing.Size(20, 20);
            this.chkLastInYear.TabIndex = 6;
            this.chkLastInYear.UseVisualStyleBackColor = false;
            // 
            // lblGreenText
            // 
            this.lblGreenText.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblGreenText.Location = new System.Drawing.Point(525, 60);
            this.lblGreenText.MaximumSize = new System.Drawing.Size(350, 30);
            this.lblGreenText.Name = "lblGreenText";
            this.lblGreenText.Size = new System.Drawing.Size(350, 30);
            this.lblGreenText.TabIndex = 7;
            this.lblGreenText.Text = "Поставьте Х если данный лист кассовой книги является последним в текущем году";
            // 
            // CashbookForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pnlMarkers);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cmbMonth);
            this.Controls.Add(this.lblMonth);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "CashbookForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Кассовая книга";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlMarkers.ResumeLayout(false);
            this.pnlMarkers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Panel pnlMarkers;
        private System.Windows.Forms.Label lblAttention;
        private System.Windows.Forms.Label lblPink;
        private System.Windows.Forms.Label lblPinkText;
        private System.Windows.Forms.Label lblPagesBefore;
        private System.Windows.Forms.CheckBox chkLastInMonth;
        private System.Windows.Forms.Label lblBlueText;
        private System.Windows.Forms.CheckBox chkLastInYear;
        private System.Windows.Forms.Label lblGreenText;
    }
}