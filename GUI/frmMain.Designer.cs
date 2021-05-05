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
            this.btnConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(12, 115);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(447, 271);
            this.txtStatus.TabIndex = 0;
            // 
            // btnZero
            // 
            this.btnZero.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnZero.Location = new System.Drawing.Point(12, 24);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(89, 36);
            this.btnZero.TabIndex = 1;
            this.btnZero.Text = "零點校正";
            this.btnZero.UseVisualStyleBackColor = true;
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            // 
            // pgb
            // 
            this.pgb.Location = new System.Drawing.Point(12, 86);
            this.pgb.Name = "pgb";
            this.pgb.Size = new System.Drawing.Size(425, 23);
            this.pgb.TabIndex = 2;
            // 
            // btnOffset
            // 
            this.btnOffset.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnOffset.Location = new System.Drawing.Point(117, 24);
            this.btnOffset.Name = "btnOffset";
            this.btnOffset.Size = new System.Drawing.Size(89, 36);
            this.btnOffset.TabIndex = 3;
            this.btnOffset.Text = "Offset校正";
            this.btnOffset.UseVisualStyleBackColor = true;
            this.btnOffset.Click += new System.EventHandler(this.btnOffset_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnConnect.Location = new System.Drawing.Point(370, 24);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(89, 36);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "連線";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 398);
            this.Controls.Add(this.btnConnect);
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
        private System.Windows.Forms.Button btnConnect;
    }
}