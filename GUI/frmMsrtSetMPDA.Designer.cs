namespace GUI
{
    partial class frmMsrtSetMPDA
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
            this.pnlK2601 = new System.Windows.Forms.Panel();
            this.nudClampV = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.nudStepValue = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.nudEndValue = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nudStartValue = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudDutyCycle = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudPulseWidth = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chbBiasEnabled = new System.Windows.Forms.CheckBox();
            this.nudBiasVoltage = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.nudDelayTime = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.nudMsrtTime = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cboMsrtRange = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.MPDA = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.nudRepeatTimes = new System.Windows.Forms.NumericUpDown();
            this.btnRun = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlK2601.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClampV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDutyCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPulseWidth)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBiasVoltage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMsrtTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRepeatTimes)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlK2601
            // 
            this.pnlK2601.Controls.Add(this.nudClampV);
            this.pnlK2601.Controls.Add(this.label13);
            this.pnlK2601.Controls.Add(this.label14);
            this.pnlK2601.Controls.Add(this.nudStepValue);
            this.pnlK2601.Controls.Add(this.label10);
            this.pnlK2601.Controls.Add(this.label11);
            this.pnlK2601.Controls.Add(this.nudEndValue);
            this.pnlK2601.Controls.Add(this.label8);
            this.pnlK2601.Controls.Add(this.label9);
            this.pnlK2601.Controls.Add(this.nudStartValue);
            this.pnlK2601.Controls.Add(this.label5);
            this.pnlK2601.Controls.Add(this.label7);
            this.pnlK2601.Controls.Add(this.nudDutyCycle);
            this.pnlK2601.Controls.Add(this.label1);
            this.pnlK2601.Controls.Add(this.label3);
            this.pnlK2601.Controls.Add(this.nudPulseWidth);
            this.pnlK2601.Controls.Add(this.label6);
            this.pnlK2601.Controls.Add(this.label2);
            this.pnlK2601.Controls.Add(this.label4);
            this.pnlK2601.Location = new System.Drawing.Point(12, 8);
            this.pnlK2601.Name = "pnlK2601";
            this.pnlK2601.Size = new System.Drawing.Size(287, 359);
            this.pnlK2601.TabIndex = 0;
            // 
            // nudClampV
            // 
            this.nudClampV.Font = new System.Drawing.Font("新細明體", 9F);
            this.nudClampV.Location = new System.Drawing.Point(104, 227);
            this.nudClampV.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudClampV.Name = "nudClampV";
            this.nudClampV.Size = new System.Drawing.Size(62, 22);
            this.nudClampV.TabIndex = 33;
            this.nudClampV.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(182, 229);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(19, 16);
            this.label13.TabIndex = 32;
            this.label13.Text = "V";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(3, 229);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 16);
            this.label14.TabIndex = 31;
            this.label14.Text = "Clamp V :";
            // 
            // nudStepValue
            // 
            this.nudStepValue.Font = new System.Drawing.Font("新細明體", 9F);
            this.nudStepValue.Location = new System.Drawing.Point(105, 189);
            this.nudStepValue.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.nudStepValue.Name = "nudStepValue";
            this.nudStepValue.Size = new System.Drawing.Size(62, 22);
            this.nudStepValue.TabIndex = 30;
            this.nudStepValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(183, 191);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 16);
            this.label10.TabIndex = 29;
            this.label10.Text = "mA";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(4, 191);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 16);
            this.label11.TabIndex = 28;
            this.label11.Text = "Step Value :";
            // 
            // nudEndValue
            // 
            this.nudEndValue.Font = new System.Drawing.Font("新細明體", 9F);
            this.nudEndValue.Location = new System.Drawing.Point(105, 153);
            this.nudEndValue.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudEndValue.Name = "nudEndValue";
            this.nudEndValue.Size = new System.Drawing.Size(62, 22);
            this.nudEndValue.TabIndex = 27;
            this.nudEndValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(183, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "mA";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(4, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "End Value :";
            // 
            // nudStartValue
            // 
            this.nudStartValue.Font = new System.Drawing.Font("新細明體", 9F);
            this.nudStartValue.Location = new System.Drawing.Point(105, 116);
            this.nudStartValue.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.nudStartValue.Name = "nudStartValue";
            this.nudStartValue.Size = new System.Drawing.Size(62, 22);
            this.nudStartValue.TabIndex = 24;
            this.nudStartValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(183, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "mA";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(4, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "Start Value :";
            // 
            // nudDutyCycle
            // 
            this.nudDutyCycle.DecimalPlaces = 1;
            this.nudDutyCycle.Font = new System.Drawing.Font("新細明體", 9F);
            this.nudDutyCycle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudDutyCycle.Location = new System.Drawing.Point(105, 73);
            this.nudDutyCycle.Name = "nudDutyCycle";
            this.nudDutyCycle.Size = new System.Drawing.Size(62, 22);
            this.nudDutyCycle.TabIndex = 21;
            this.nudDutyCycle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(183, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(4, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "Duty Cycle : ";
            // 
            // nudPulseWidth
            // 
            this.nudPulseWidth.DecimalPlaces = 2;
            this.nudPulseWidth.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.nudPulseWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudPulseWidth.Location = new System.Drawing.Point(104, 40);
            this.nudPulseWidth.Name = "nudPulseWidth";
            this.nudPulseWidth.Size = new System.Drawing.Size(62, 22);
            this.nudPulseWidth.TabIndex = 18;
            this.nudPulseWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(182, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "ms";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(3, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Pulse Width : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "K2601 :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chbBiasEnabled);
            this.panel2.Controls.Add(this.nudBiasVoltage);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.nudDelayTime);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.nudMsrtTime);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.cboMsrtRange);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.MPDA);
            this.panel2.Location = new System.Drawing.Point(305, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(285, 196);
            this.panel2.TabIndex = 1;
            // 
            // chbBiasEnabled
            // 
            this.chbBiasEnabled.AutoSize = true;
            this.chbBiasEnabled.Font = new System.Drawing.Font("新細明體", 12F);
            this.chbBiasEnabled.Location = new System.Drawing.Point(7, 155);
            this.chbBiasEnabled.Name = "chbBiasEnabled";
            this.chbBiasEnabled.Size = new System.Drawing.Size(54, 20);
            this.chbBiasEnabled.TabIndex = 28;
            this.chbBiasEnabled.Text = "Bias";
            this.chbBiasEnabled.UseVisualStyleBackColor = true;
            // 
            // nudBiasVoltage
            // 
            this.nudBiasVoltage.DecimalPlaces = 1;
            this.nudBiasVoltage.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.nudBiasVoltage.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudBiasVoltage.Location = new System.Drawing.Point(105, 153);
            this.nudBiasVoltage.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBiasVoltage.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudBiasVoltage.Name = "nudBiasVoltage";
            this.nudBiasVoltage.Size = new System.Drawing.Size(74, 22);
            this.nudBiasVoltage.TabIndex = 27;
            this.nudBiasVoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label19.Location = new System.Drawing.Point(183, 155);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(19, 16);
            this.label19.TabIndex = 26;
            this.label19.Text = "V";
            // 
            // nudDelayTime
            // 
            this.nudDelayTime.DecimalPlaces = 1;
            this.nudDelayTime.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.nudDelayTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudDelayTime.Location = new System.Drawing.Point(105, 116);
            this.nudDelayTime.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.nudDelayTime.Name = "nudDelayTime";
            this.nudDelayTime.Size = new System.Drawing.Size(74, 22);
            this.nudDelayTime.TabIndex = 24;
            this.nudDelayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label17.Location = new System.Drawing.Point(183, 118);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(22, 16);
            this.label17.TabIndex = 23;
            this.label17.Text = "us";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label18.Location = new System.Drawing.Point(4, 118);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(88, 16);
            this.label18.TabIndex = 22;
            this.label18.Text = "Delay time : ";
            // 
            // nudMsrtTime
            // 
            this.nudMsrtTime.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.nudMsrtTime.Location = new System.Drawing.Point(105, 71);
            this.nudMsrtTime.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.nudMsrtTime.Name = "nudMsrtTime";
            this.nudMsrtTime.Size = new System.Drawing.Size(74, 22);
            this.nudMsrtTime.TabIndex = 21;
            this.nudMsrtTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label15.Location = new System.Drawing.Point(183, 73);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(22, 16);
            this.label15.TabIndex = 20;
            this.label15.Text = "us";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(4, 73);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 16);
            this.label16.TabIndex = 19;
            this.label16.Text = "Msrt time : ";
            // 
            // cboMsrtRange
            // 
            this.cboMsrtRange.Font = new System.Drawing.Font("新細明體", 9F);
            this.cboMsrtRange.FormattingEnabled = true;
            this.cboMsrtRange.Items.AddRange(new object[] {
            "100mA",
            "10mA",
            "1mA",
            "100uA",
            "10uA",
            "1uA",
            "100nA",
            "10nA"});
            this.cboMsrtRange.Location = new System.Drawing.Point(105, 40);
            this.cboMsrtRange.Name = "cboMsrtRange";
            this.cboMsrtRange.Size = new System.Drawing.Size(74, 20);
            this.cboMsrtRange.TabIndex = 12;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(4, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 16);
            this.label12.TabIndex = 11;
            this.label12.Text = "Msrt Range : ";
            // 
            // MPDA
            // 
            this.MPDA.AutoSize = true;
            this.MPDA.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MPDA.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MPDA.Location = new System.Drawing.Point(3, 10);
            this.MPDA.Name = "MPDA";
            this.MPDA.Size = new System.Drawing.Size(77, 19);
            this.MPDA.TabIndex = 9;
            this.MPDA.Text = "MPDA :";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label21.Location = new System.Drawing.Point(3, 46);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(94, 16);
            this.label21.TabIndex = 34;
            this.label21.Text = "Repeat time : ";
            // 
            // nudRepeatTimes
            // 
            this.nudRepeatTimes.Location = new System.Drawing.Point(97, 40);
            this.nudRepeatTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRepeatTimes.Name = "nudRepeatTimes";
            this.nudRepeatTimes.Size = new System.Drawing.Size(43, 22);
            this.nudRepeatTimes.TabIndex = 33;
            this.nudRepeatTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnRun
            // 
            this.btnRun.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnRun.Location = new System.Drawing.Point(442, 334);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(68, 33);
            this.btnRun.TabIndex = 32;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.nudRepeatTimes);
            this.panel1.Location = new System.Drawing.Point(305, 210);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 118);
            this.panel1.TabIndex = 29;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label20.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label20.Location = new System.Drawing.Point(6, 10);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(88, 19);
            this.label20.TabIndex = 29;
            this.label20.Text = "Test Set :";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("新細明體", 14F);
            this.btnSave.Location = new System.Drawing.Point(522, 334);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 33);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmMsrtSetMPDA
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(618, 374);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlK2601);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMsrtSetMPDA";
            this.Text = "PIV Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMsrtSetMPDA_FormClosed);
            this.Load += new System.EventHandler(this.frmMsrtSetMPDA_Load);
            this.pnlK2601.ResumeLayout(false);
            this.pnlK2601.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClampV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDutyCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPulseWidth)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBiasVoltage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMsrtTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRepeatTimes)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlK2601;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudPulseWidth;
        private System.Windows.Forms.NumericUpDown nudEndValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudStartValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudDutyCycle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudStepValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboMsrtRange;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label MPDA;
        private System.Windows.Forms.NumericUpDown nudClampV;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudMsrtTime;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown nudDelayTime;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chbBiasEnabled;
        private System.Windows.Forms.NumericUpDown nudBiasVoltage;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown nudRepeatTimes;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}