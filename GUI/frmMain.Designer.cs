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
            this.btnZero = new System.Windows.Forms.Button();
            this.pgb = new System.Windows.Forms.ProgressBar();
            this.btnOffset = new System.Windows.Forms.Button();
            this.btnMPDAConnect = new System.Windows.Forms.Button();
            this.btnSMUConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMPDAStatus = new System.Windows.Forms.Label();
            this.lblSMUStatus = new System.Windows.Forms.Label();
            this.btnCurrent = new System.Windows.Forms.Button();
            this.btnBias = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.Font = new System.Drawing.Font("新細明體", 14F);
            this.txtStatus.Location = new System.Drawing.Point(12, 228);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(447, 271);
            this.txtStatus.TabIndex = 0;
            // 
            // btnZero
            // 
            this.btnZero.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnZero.Location = new System.Drawing.Point(240, 28);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(89, 36);
            this.btnZero.TabIndex = 1;
            this.btnZero.Text = "零點校正";
            this.btnZero.UseVisualStyleBackColor = true;
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            // 
            // pgb
            // 
            this.pgb.Location = new System.Drawing.Point(12, 199);
            this.pgb.Name = "pgb";
            this.pgb.Size = new System.Drawing.Size(447, 23);
            this.pgb.TabIndex = 2;
            // 
            // btnOffset
            // 
            this.btnOffset.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnOffset.Location = new System.Drawing.Point(348, 28);
            this.btnOffset.Name = "btnOffset";
            this.btnOffset.Size = new System.Drawing.Size(89, 36);
            this.btnOffset.TabIndex = 3;
            this.btnOffset.Text = "Offset校正";
            this.btnOffset.UseVisualStyleBackColor = true;
            this.btnOffset.Click += new System.EventHandler(this.btnOffset_Click);
            // 
            // btnMPDAConnect
            // 
            this.btnMPDAConnect.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnMPDAConnect.Location = new System.Drawing.Point(12, 28);
            this.btnMPDAConnect.Name = "btnMPDAConnect";
            this.btnMPDAConnect.Size = new System.Drawing.Size(106, 36);
            this.btnMPDAConnect.TabIndex = 4;
            this.btnMPDAConnect.Text = "MPDA連線";
            this.btnMPDAConnect.UseVisualStyleBackColor = true;
            this.btnMPDAConnect.Click += new System.EventHandler(this.btnMPDAConnect_Click);
            // 
            // btnSMUConnect
            // 
            this.btnSMUConnect.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnSMUConnect.Location = new System.Drawing.Point(12, 85);
            this.btnSMUConnect.Name = "btnSMUConnect";
            this.btnSMUConnect.Size = new System.Drawing.Size(106, 36);
            this.btnSMUConnect.TabIndex = 5;
            this.btnSMUConnect.Text = "SMU連線";
            this.btnSMUConnect.UseVisualStyleBackColor = true;
            this.btnSMUConnect.Click += new System.EventHandler(this.btnSMUConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14F);
            this.label1.Location = new System.Drawing.Point(12, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "狀態 : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14F);
            this.label2.Location = new System.Drawing.Point(12, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "MPDA :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 14F);
            this.label3.Location = new System.Drawing.Point(236, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "SMU :";
            // 
            // lblMPDAStatus
            // 
            this.lblMPDAStatus.AutoSize = true;
            this.lblMPDAStatus.Font = new System.Drawing.Font("新細明體", 14F);
            this.lblMPDAStatus.Location = new System.Drawing.Point(89, 167);
            this.lblMPDAStatus.Name = "lblMPDAStatus";
            this.lblMPDAStatus.Size = new System.Drawing.Size(53, 19);
            this.lblMPDAStatus.TabIndex = 9;
            this.lblMPDAStatus.Text = "label4";
            // 
            // lblSMUStatus
            // 
            this.lblSMUStatus.AutoSize = true;
            this.lblSMUStatus.Font = new System.Drawing.Font("新細明體", 14F);
            this.lblSMUStatus.Location = new System.Drawing.Point(300, 167);
            this.lblSMUStatus.Name = "lblSMUStatus";
            this.lblSMUStatus.Size = new System.Drawing.Size(53, 19);
            this.lblSMUStatus.TabIndex = 10;
            this.lblSMUStatus.Text = "label5";
            // 
            // btnCurrent
            // 
            this.btnCurrent.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnCurrent.Location = new System.Drawing.Point(240, 85);
            this.btnCurrent.Name = "btnCurrent";
            this.btnCurrent.Size = new System.Drawing.Size(89, 36);
            this.btnCurrent.TabIndex = 11;
            this.btnCurrent.Text = "電流校正";
            this.btnCurrent.UseVisualStyleBackColor = true;
            this.btnCurrent.Click += new System.EventHandler(this.btnCurrent_Click);
            // 
            // btnBias
            // 
            this.btnBias.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnBias.Location = new System.Drawing.Point(348, 85);
            this.btnBias.Name = "btnBias";
            this.btnBias.Size = new System.Drawing.Size(89, 36);
            this.btnBias.TabIndex = 12;
            this.btnBias.Text = "Bias校正";
            this.btnBias.UseVisualStyleBackColor = true;
            this.btnBias.Click += new System.EventHandler(this.btnBias_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 516);
            this.Controls.Add(this.btnBias);
            this.Controls.Add(this.btnCurrent);
            this.Controls.Add(this.lblSMUStatus);
            this.Controls.Add(this.lblMPDAStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSMUConnect);
            this.Controls.Add(this.btnMPDAConnect);
            this.Controls.Add(this.btnOffset);
            this.Controls.Add(this.pgb);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.txtStatus);
            this.Name = "frmMain";
            this.Text = "Calibration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.ProgressBar pgb;
        private System.Windows.Forms.Button btnOffset;
        private System.Windows.Forms.Button btnMPDAConnect;
        private System.Windows.Forms.Button btnSMUConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMPDAStatus;
        private System.Windows.Forms.Label lblSMUStatus;
        private System.Windows.Forms.Button btnCurrent;
        private System.Windows.Forms.Button btnBias;
    }
}