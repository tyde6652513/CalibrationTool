
namespace GUI
{
    partial class CaliDataStatus
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chkListStatus = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkListStatus
            // 
            this.chkListStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkListStatus.Enabled = false;
            this.chkListStatus.Font = new System.Drawing.Font("新細明體", 10F);
            this.chkListStatus.FormattingEnabled = true;
            this.chkListStatus.Items.AddRange(new object[] {
            "零點校正",
            "Offset校正",
            "電流校正",
            "電壓校正",
            "QC驗證"});
            this.chkListStatus.Location = new System.Drawing.Point(0, 40);
            this.chkListStatus.Name = "chkListStatus";
            this.chkListStatus.Size = new System.Drawing.Size(143, 90);
            this.chkListStatus.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F);
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "已完成 : ";
            // 
            // CaliDataStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkListStatus);
            this.Name = "CaliDataStatus";
            this.Size = new System.Drawing.Size(146, 135);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chkListStatus;
        private System.Windows.Forms.Label label1;
    }
}
