namespace GUI
{
    partial class frmMain
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
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.pgb = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMPDAStatus = new System.Windows.Forms.Label();
            this.lblSMUStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mspTool = new System.Windows.Forms.MenuStrip();
            this.連線ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMPDAConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSMUConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.校正ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmZero = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOffset = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmBias = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWriteData = new System.Windows.Forms.ToolStripMenuItem();
            this.mspTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.Font = new System.Drawing.Font("新細明體", 14F);
            this.txtStatus.Location = new System.Drawing.Point(12, 135);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(362, 305);
            this.txtStatus.TabIndex = 0;
            // 
            // pgb
            // 
            this.pgb.Location = new System.Drawing.Point(12, 106);
            this.pgb.Name = "pgb";
            this.pgb.Size = new System.Drawing.Size(362, 23);
            this.pgb.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14F);
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "MPDA :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 14F);
            this.label3.Location = new System.Drawing.Point(208, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "SMU :";
            // 
            // lblMPDAStatus
            // 
            this.lblMPDAStatus.AutoSize = true;
            this.lblMPDAStatus.Font = new System.Drawing.Font("新細明體", 14F);
            this.lblMPDAStatus.Location = new System.Drawing.Point(89, 74);
            this.lblMPDAStatus.Name = "lblMPDAStatus";
            this.lblMPDAStatus.Size = new System.Drawing.Size(53, 19);
            this.lblMPDAStatus.TabIndex = 9;
            this.lblMPDAStatus.Text = "label4";
            // 
            // lblSMUStatus
            // 
            this.lblSMUStatus.AutoSize = true;
            this.lblSMUStatus.Font = new System.Drawing.Font("新細明體", 14F);
            this.lblSMUStatus.Location = new System.Drawing.Point(272, 74);
            this.lblSMUStatus.Name = "lblSMUStatus";
            this.lblSMUStatus.Size = new System.Drawing.Size(53, 19);
            this.lblSMUStatus.TabIndex = 10;
            this.lblSMUStatus.Text = "label5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14F);
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "狀態 : ";
            // 
            // mspTool
            // 
            this.mspTool.BackColor = System.Drawing.SystemColors.Control;
            this.mspTool.Font = new System.Drawing.Font("Microsoft JhengHei UI", 14F);
            this.mspTool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.mspTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.連線ToolStripMenuItem,
            this.校正ToolStripMenuItem});
            this.mspTool.Location = new System.Drawing.Point(0, 0);
            this.mspTool.Name = "mspTool";
            this.mspTool.Size = new System.Drawing.Size(380, 32);
            this.mspTool.TabIndex = 13;
            this.mspTool.Text = "menuStrip1";
            // 
            // 連線ToolStripMenuItem
            // 
            this.連線ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMPDAConnect,
            this.tsmSMUConnect});
            this.連線ToolStripMenuItem.Name = "連線ToolStripMenuItem";
            this.連線ToolStripMenuItem.Size = new System.Drawing.Size(60, 28);
            this.連線ToolStripMenuItem.Text = "連線";
            // 
            // tsmMPDAConnect
            // 
            this.tsmMPDAConnect.Name = "tsmMPDAConnect";
            this.tsmMPDAConnect.Size = new System.Drawing.Size(136, 28);
            this.tsmMPDAConnect.Text = "MPDA";
            this.tsmMPDAConnect.Click += new System.EventHandler(this.tsmMPDAConnect_Click);
            // 
            // tsmSMUConnect
            // 
            this.tsmSMUConnect.Name = "tsmSMUConnect";
            this.tsmSMUConnect.Size = new System.Drawing.Size(136, 28);
            this.tsmSMUConnect.Text = "SMU";
            this.tsmSMUConnect.Click += new System.EventHandler(this.tsmSMUConnect_Click);
            // 
            // 校正ToolStripMenuItem
            // 
            this.校正ToolStripMenuItem.AutoToolTip = true;
            this.校正ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmZero,
            this.tsmOffset,
            this.tsmCurrent,
            this.tsmBias,
            this.tsmWriteData});
            this.校正ToolStripMenuItem.Name = "校正ToolStripMenuItem";
            this.校正ToolStripMenuItem.Size = new System.Drawing.Size(60, 28);
            this.校正ToolStripMenuItem.Text = "校正";
            // 
            // tsmZero
            // 
            this.tsmZero.Name = "tsmZero";
            this.tsmZero.Size = new System.Drawing.Size(180, 28);
            this.tsmZero.Text = "零點校正";
            this.tsmZero.Click += new System.EventHandler(this.tsmZero_Click);
            // 
            // tsmOffset
            // 
            this.tsmOffset.Name = "tsmOffset";
            this.tsmOffset.Size = new System.Drawing.Size(180, 28);
            this.tsmOffset.Text = "Offset校正";
            this.tsmOffset.Click += new System.EventHandler(this.tsmOffset_Click);
            // 
            // tsmCurrent
            // 
            this.tsmCurrent.Name = "tsmCurrent";
            this.tsmCurrent.Size = new System.Drawing.Size(180, 28);
            this.tsmCurrent.Text = "電流校正";
            this.tsmCurrent.Click += new System.EventHandler(this.tsmCurrent_Click);
            // 
            // tsmBias
            // 
            this.tsmBias.Name = "tsmBias";
            this.tsmBias.Size = new System.Drawing.Size(180, 28);
            this.tsmBias.Text = "Bias校正";
            this.tsmBias.Click += new System.EventHandler(this.tsmBias_Click);
            // 
            // tsmWriteData
            // 
            this.tsmWriteData.Name = "tsmWriteData";
            this.tsmWriteData.Size = new System.Drawing.Size(180, 28);
            this.tsmWriteData.Text = "寫入校正檔";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(380, 446);
            this.Controls.Add(this.lblSMUStatus);
            this.Controls.Add(this.lblMPDAStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pgb);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.mspTool);
            this.MainMenuStrip = this.mspTool;
            this.Name = "frmMain";
            this.Text = "Calibration";
            this.mspTool.ResumeLayout(false);
            this.mspTool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ProgressBar pgb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMPDAStatus;
        private System.Windows.Forms.Label lblSMUStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip mspTool;
        private System.Windows.Forms.ToolStripMenuItem 連線ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmMPDAConnect;
        private System.Windows.Forms.ToolStripMenuItem tsmSMUConnect;
        private System.Windows.Forms.ToolStripMenuItem 校正ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmZero;
        private System.Windows.Forms.ToolStripMenuItem tsmOffset;
        private System.Windows.Forms.ToolStripMenuItem tsmCurrent;
        private System.Windows.Forms.ToolStripMenuItem tsmBias;
        private System.Windows.Forms.ToolStripMenuItem tsmWriteData;
    }
}