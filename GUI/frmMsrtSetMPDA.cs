using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestSeting;
using Device.K2601;
using Device.MPDA;
using System.Threading;
using System.IO;

namespace GUI
{
    public partial class frmMsrtSetMPDA : Form
    {

        #region >>>field<<<

        private MPDAControl _mPDA;
        private K2601Control _k2601;

        private PulseParameter _pulseParameter = new PulseParameter();
        private MPDAParameter _mpdaParameter = new MPDAParameter();

        private bool _changeFlag = true;
        private int _repeatTimes;
        private int _repeatWait = 500;
        private List<double> _readData = new List<double>();

        #endregion

        public frmMsrtSetMPDA(MPDAControl mPDA, K2601Control k2601)
        {
            //this.TopLevel = false;
            //this.TopMost = true;
            InitializeComponent();
            this._mPDA = mPDA;
            this._k2601 = k2601;
        }


        #region >>>public method<<<

        #endregion


        #region>>>Event<<<

        private void btnRun_Click(object sender, EventArgs e)
       {


            if (this._changeFlag)
            {
                //K2601
                this._pulseParameter.PulseWidth = (double)nudPulseWidth.Value * 1e-3;//ms
                this._pulseParameter.DutyCycle = (double)nudDutyCycle.Value * 1e-2;//%
                this._pulseParameter.StartValue = nudStartValue.Value * 1e-3m;//mA
                this._pulseParameter.EndValue = nudEndValue.Value * 1e-3m;//mA
                this._pulseParameter.StepValue = nudStepValue.Value * 1e-3m;//mA
                this._pulseParameter.ClampVoltage = (int)nudClampV.Value;
                //MPDA
                this._mpdaParameter.MeasureRange = (byte)(cboMsrtRange.SelectedIndex + 1);
                this._mpdaParameter.IntegrationTimes = (int)(nudMsrtTime.Value);
                this._mpdaParameter.IntervalTimes = 1;
                this._mpdaParameter.DelayTimeSet = (int)(nudDelayTime.Value * 10);
                this._mpdaParameter.TimeBase = 1;
                this._mpdaParameter.BiasEnable = chbBiasEnabled.Checked;
                this._mpdaParameter.BiasVoltage = nudBiasVoltage.Value;
                try
                {
                    this._mPDA.SetAllParameter(this._mpdaParameter);
                    this._k2601.PulseSweep(this._pulseParameter);
                    //this._k2601.Pulse
                    //this._k2601Control.PulseSweep(pulseP);
                    //this._k2601Control.SetTrig2();
                    //btnRun.Enabled = true;

                    //this._k2601Control.LoadFunction_FvMi2(); //SvMi(0.9, 1.5, 1e-3)

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                Thread.Sleep(300);
                this._changeFlag = false;
            }
            btnSave.Enabled = false;
            this._repeatTimes = (int)nudRepeatTimes.Value;
            int numOfRead = DMMParameter.MsrtCount;
            double[] temp = new double[numOfRead];
            for (int i = 0; i < _repeatTimes; i++)
            {
                this._k2601.CommunicationBase.SendCommand("PulseSweep()");
                Thread.Sleep(_repeatWait);
                temp = this._mPDA.ReadRawData(numOfRead);
                for (int j = 0; j < numOfRead; j++)
                {
                    this._readData.Add(temp[j]);
                }
                this._mPDA.ClearBuffer();
                Thread.Sleep(10);
            }
            btnSave.Enabled = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string first;
                string second;
                string newLine;
                first = "TestNum";
                second = "Value(A)";
                newLine = string.Format("{0},{1}", first, second);
                sb.AppendLine(newLine);
                int loopTimes = _repeatTimes * DMMParameter.MsrtCount;
                for (int i = 0; i < loopTimes; i++)
                {

                    first = (i + 1).ToString();
                    second = _readData[i].ToString();
                    newLine = string.Format("{0},{1}", first, second);
                    sb.AppendLine(newLine);
                }
                saveFileDialog1.ShowDialog();
                string filePath = saveFileDialog1.FileName;
                File.WriteAllText(filePath, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {           
            this._changeFlag = true;
        }        

        #region>>>Save Property(Default value)<<<

        private void frmMsrtSetMPDA_Load(object sender, EventArgs e)
        {


           
            //load value to component
            nudPulseWidth.Value = GUI.Properties.Settings.Default.PulseWidth;
            nudDutyCycle.Value = GUI.Properties.Settings.Default.DutyCycle;
            nudStartValue.Value = GUI.Properties.Settings.Default.StartValue;
            nudEndValue.Value = GUI.Properties.Settings.Default.EndValue;
            nudStepValue.Value = GUI.Properties.Settings.Default.StepValue;
            nudClampV.Value = GUI.Properties.Settings.Default.ClanpV;
 

            cboMsrtRange.SelectedIndex = GUI.Properties.Settings.Default.MsrtRange;
            nudMsrtTime.Value = GUI.Properties.Settings.Default.MsrtTime;
            nudDelayTime.Value = GUI.Properties.Settings.Default.DelayTime;
            chbBiasEnabled.Checked = GUI.Properties.Settings.Default.BiasEnabled;
                       
 
            this.nudClampV.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.nudStepValue.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.nudEndValue.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.nudStartValue.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.nudDutyCycle.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.nudPulseWidth.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.chbBiasEnabled.CheckedChanged += new System.EventHandler(this.ValueChanged);
            this.nudBiasVoltage.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.nudDelayTime.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.nudMsrtTime.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.cboMsrtRange.SelectedIndexChanged += new System.EventHandler(this.ValueChanged);

            nudBiasVoltage.Value = GUI.Properties.Settings.Default.Bias;

        }

        private void frmMsrtSetMPDA_FormClosed(object sender, FormClosedEventArgs e)
       {
           //save component value
            GUI.Properties.Settings.Default.PulseWidth = nudPulseWidth.Value;
            GUI.Properties.Settings.Default.DutyCycle = nudDutyCycle.Value;
            GUI.Properties.Settings.Default.StartValue = nudStartValue.Value;
            GUI.Properties.Settings.Default.EndValue = nudEndValue.Value;
            GUI.Properties.Settings.Default.StepValue = nudStepValue.Value;
            GUI.Properties.Settings.Default.ClanpV = nudClampV.Value;

            GUI.Properties.Settings.Default.MsrtRange = cboMsrtRange.SelectedIndex;
            GUI.Properties.Settings.Default.MsrtTime = nudMsrtTime.Value;
            GUI.Properties.Settings.Default.DelayTime = nudDelayTime.Value;
            GUI.Properties.Settings.Default.BiasEnabled = chbBiasEnabled.Checked;
            GUI.Properties.Settings.Default.Bias = nudBiasVoltage.Value;
            GUI.Properties.Settings.Default.Save();

           
       }


        #endregion

        #endregion

        
    }
}
