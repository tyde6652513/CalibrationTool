using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Device.Communication;
using Device.MPDA;
using TestSeting;


namespace GUI
{
    public partial class frmMain : Form
    {

        public Dictionary<int, string> Range =
        new Dictionary<int, string>()
        {
            /// 1:100mA; 2:10mA; 3:1mA; 4:100uA; 5:10uA;
            /// 6:1uA; 7:100nA; 8:10nA; 9:1nA
            {1, "100mA"}, {2, "10mA"},
            {3, "1mA"}, {4, "100uA"},
            {5, "10uA"}, {6, "1uA"},
            {7, "100nA"}, {8, "10nA"},
            {9, "1nA"}
        };

        #region >>>field<<<

        private SubFormAgent _subFormAgent = new SubFormAgent();

        //other
        private TcpSetting _tcpSet;
        private MPDAControl _mpdaControl;
        private CallibrationData _callibrationData = new CallibrationData();

        //
        const string dirPath = "CaliData";
        const string filePath = "CaliData.xml";

        #endregion

        #region >>>property<<<
        #endregion

        #region >>>Constructor<<<

        public frmMain()
        {
            InitializeComponent();
            if (! (Directory.Exists(dirPath)) )
            {
                Directory.CreateDirectory(dirPath);
            }
            if (! ( File.Exists(dirPath + @"\" + filePath) ))
            {
                this.SerializeXML();
            }
        }

        #endregion

        #region >>>private method<<<

        private void connect() 
        {
            while (true)
            {
                this._tcpSet = this._subFormAgent.MPDAConnect();
                try
                {
                    this._mpdaControl = new MPDAControl(this._tcpSet);
                    break;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void updataStatus() 
        {
            this.pgb.PerformStep();
            txtStatus.Text += "......Ok\r\n";
            txtStatus.SelectionStart = txtStatus.Text.Length;
            txtStatus.ScrollToCaret();
            this.Refresh();
        }

        private void nextStatus(string str) 
        {
            txtStatus.Text += str;
            txtStatus.SelectionStart = txtStatus.Text.Length;
            txtStatus.ScrollToCaret();
            this.Refresh();
        }

        private void SerializeXML()
        {
            using (FileStream oFileStream = new FileStream(dirPath + @"\" + filePath, FileMode.Create))
            {
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(CallibrationData));
                oXmlSerializer.Serialize(oFileStream, this._callibrationData);
                oFileStream.Close();
            }
        }

        private CallibrationData DeSerializeXML()
        {
            using (FileStream oFileStream = new FileStream(dirPath + @"\CaliData.xml", FileMode.Open))
            {
                CallibrationData o = null;
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(CallibrationData));
                o = (CallibrationData)oXmlSerializer.Deserialize(oFileStream);
                oFileStream.Close();
                return o;
            }
        }

        private void ZeroCalibration() 
        {
            byte[] temp;
            // progress bar
            this.pgb.Visible = true;
            this.pgb.Minimum = 0;
            this.pgb.Maximum = 9;
            this.pgb.Value = 0;
            this.pgb.Step = 1;
            //
            try
            {
                this.nextStatus("清除Flash");
                this._mpdaControl.Communication.SendCmd(new byte[2] { 0x47, 0x03 });
                if (this._mpdaControl.Communication.ReturnBytes[1] != 0x01)
                {
                    MessageBox.Show("清除Flash失敗");
                }
                this.updataStatus();
                //
                this.nextStatus("清除RAM");
                this._mpdaControl.Communication.SendCmd(new byte[2] { 0x47, 0x04 });
                if (this._mpdaControl.Communication.ReturnBytes[1] != 0x01)
                {
                    MessageBox.Show("清除RAM失敗");
                }
                this.updataStatus();
                //
                this.nextStatus("切換檔位到1mA");
                this._mpdaControl.SetMsrRange(3);
                this.updataStatus();
                //
                this._subFormAgent.MessageBox("短路電路板上的Jumper，進行ADC Driver零點校正 完成後按下一步");
                //
                this.nextStatus("設定量測時間1.6s");
                this._mpdaControl.Communication.SendCmd(new byte[5] { 0x93, 0x00, 0x18, 0x6A, 0x00 });
                if (this._mpdaControl.Communication.ReturnBytes[1] != 0x01)
                {
                    MessageBox.Show("量測設定失敗");
                }
                this.updataStatus();
                //
                this.nextStatus("量測，存取量測值");
                this._mpdaControl.InternalTrigger();
                temp = this._mpdaControl.ReadData(1)[0];
                this._callibrationData.ZeroCalibrate.TestItem[0] = new Item();
                this._callibrationData.ZeroCalibrate.TestItem[0].Address = 0x14;
                this._callibrationData.ZeroCalibrate.TestItem[0].Value = temp;
                this.updataStatus();
                //           
                this._subFormAgent.MessageBox("拔去電路板上的Jumper，短路Input端(BNC內外層短路) 完成後按下一步");
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                //
                this.nextStatus("量測，存取量測值");
                this._mpdaControl.InternalTrigger();
                temp = this._mpdaControl.ReadData(1)[0];
                this._callibrationData.ZeroCalibrate.TestItem[1] = new Item();
                this._callibrationData.ZeroCalibrate.TestItem[1].Address = 0x15;
                this._callibrationData.ZeroCalibrate.TestItem[1].Value = temp;
                this.updataStatus();
                //
                this.nextStatus("將校正值寫入RAM");
                for (int i = 0; i < this._callibrationData.ZeroCalibrate.TestItem.Length; i++)
                {
                    this._mpdaControl.SetToRam(this._callibrationData.ZeroCalibrate.TestItem[i].Address, this._callibrationData.ZeroCalibrate.TestItem[i].Value.Take(4).ToArray());
                }
                this.updataStatus();
                //
                this.nextStatus("將校正值寫入Flash");
                this._mpdaControl.Communication.SendCmd(new byte[2] { 0x47, 0x02 });
                this.updataStatus();
                //
                this._subFormAgent.MessageBox("零點校正完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
         }

        private void OffsetCalibration() 
        {
            //try
            //{
                byte[] temp;
                // progress bar
                this.pgb.Visible = true;
                this.pgb.Minimum = 0;
                this.pgb.Maximum = 29;
                this.pgb.Value = 1;
                this.pgb.Step = 1;
                txtStatus.Text = string.Empty;
                this.Refresh();
                //
                for (int i = 1; i < 10; i++)
                {
                    this.nextStatus("切換檔位到" + Range[i]);
                    this._mpdaControl.SetMsrRange((byte) i);
                    this.updataStatus();
                    //
                    this.nextStatus("Input Offset調整");
                    this._mpdaControl.OffsetAdjust();
                    this.updataStatus();
                    //
                    this.nextStatus("存取調整值");
                    temp = this._mpdaControl.ReadOffset();
                    this._callibrationData.OffsetCalibrate.TestItem[i-1] = new Item();
                    this._callibrationData.OffsetCalibrate.TestItem[i-1].Address = (byte) (0x16 + (i-1));
                    this._callibrationData.OffsetCalibrate.TestItem[i-1].Value = temp;
                    this.updataStatus();
                    //
                }
                //
                this.nextStatus("將校正值寫入RAM");
                for (int i = 0; i < this._callibrationData.OffsetCalibrate.TestItem.Length; i++)
                {
                    this._mpdaControl.SetToRam(this._callibrationData.OffsetCalibrate.TestItem[i].Address, this._callibrationData.OffsetCalibrate.TestItem[i].Value);
                }
                this.updataStatus();
                //
                this.nextStatus("將校正值寫入Flash");
                this._mpdaControl.Communication.SendCmd(new byte[2] { 0x47, 0x02 });
                this.updataStatus();
                //
                this._subFormAgent.MessageBox("零點校正完成");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}           
        }

        #endregion

        #region >>>Public method<<<
        #endregion

        #region >>>Event<<<

        private void btnZero_Click(object sender, EventArgs e)
        {
            this._callibrationData = this.DeSerializeXML();
            this.ZeroCalibration();           
            this.SerializeXML();
        }

        private void btnOffset_Click(object sender, EventArgs e)
        {
            this._callibrationData = this.DeSerializeXML();
            this.OffsetCalibration();
            this.SerializeXML();
        }




        #endregion

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.connect();
        }
    }
}
