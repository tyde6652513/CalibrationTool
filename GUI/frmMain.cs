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
using Device.K2601;
using TestSeting;


namespace GUI
{
    public partial class frmMain : Form
    {
        #region >>>Dictionary and Array<<<

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

        public int[] CurValue = new int[21]
        {
            -100, -90, -80, -70, -60, -50,
            -40, -30, -20, -10, 0, 10, 20,
            30, 40, 50, 60, 70, 80, 90, 100
        };

        #endregion

        #region >>>field<<<

        private SubFormAgent _subFormAgent = new SubFormAgent();

        //other
        private TcpSetting _tcpSet;
        private MPDAControl _mpdaControl;
        private K2601Control _k2601Control;
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
            this.lblMPDAStatus.Text = "Disconnected";
            this.lblSMUStatus.Text = "Disconnected";
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

        private void MPDAconnect() 
        {
            this._tcpSet = this._subFormAgent.MPDAConnect();
            if (this._tcpSet != null)
            {
                try
                {
                    this._mpdaControl = new MPDAControl(this._tcpSet);
                    this.lblMPDAStatus.Text = "Connected";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }          
        }

        private void SMUconnect() 
        {
            this._tcpSet = this._subFormAgent.SMUConnect();
            if (this._tcpSet != null)
            {
                try
                {
                    this._k2601Control = new K2601Control(this._tcpSet);
                    this._k2601Control.CommunicationBase.SendCmd("print(5)");
                    Thread.Sleep(50);
                    this._k2601Control.CommunicationBase.Receive(0);                   
                    this.lblSMUStatus.Text = "Connected";
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
            txtStatus.Text = string.Empty;
            this.Refresh();
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
            byte[] temp;
            // progress bar
            this.pgb.Visible = true;
            this.pgb.Minimum = 0;
            this.pgb.Maximum = 30;
            this.pgb.Value = 1;
            this.pgb.Step = 1;
            txtStatus.Text = string.Empty;
            this.Refresh();
            try
            {
                //
                this._subFormAgent.MessageBox("拔去Input短路，接上1000Ω電阻 完成後按下一步");
                //
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CurrentCalibration() 
        {
            // progress bar
            this.pgb.Visible = true;
            this.pgb.Minimum = 0;
            this.pgb.Maximum = 212;
            this.pgb.Value = 1;
            this.pgb.Step = 1;
            txtStatus.Text = string.Empty;
            this.Refresh();
            //
            double[] temp;           
            string SmuReturnData;
            List<double> SmuMsrtValue = new List<double>(); //存取SMU量測I值
            try
            {
                // 載入Script
                this._k2601Control.LoadFunction_FiMi();
                //
                this._subFormAgent.MessageBox("Keithley接線到MPDA，F+接BNC中心，F-接BNC外層");
                //
                this.nextStatus("設定量測時間1.6s");
                this._mpdaControl.Communication.SendCmd(new byte[5] { 0x93, 0x00, 0x18, 0x6A, 0x00 });
                if (this._mpdaControl.Communication.ReturnBytes[1] != 0x01)
                {
                    MessageBox.Show("量測設定失敗");
                }
                this.updataStatus();
                //
                this.nextStatus("設定檔位到" + Range[1]);
                this._mpdaControl.SetMsrRange(1);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -60 ~ 60 (mA)共量測13筆
                for (double i = -6; i <= 6; i++)
                {
                    this.nextStatus("輸出電流" + (i * 10) + "(mA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + (i / 100) + ", 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }               
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range1.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位1資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range1.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range1[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range1[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range1[i].Address = (byte)(0x23 + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[2]);              
                this._mpdaControl.SetMsrRange(2);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -10 ~ 10 (mA)共量測21筆
                SmuMsrtValue.Clear();
                for (double i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + i + "(mA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-3, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range2.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位2資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range2.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range2[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range2[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range2[i].Address = (byte)(0x34 + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[3]);
                this._mpdaControl.SetMsrRange(3);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -1000 ~ 1000 (uA)共量測21筆
                SmuMsrtValue.Clear();
                for (int i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + (i*100) + "(uA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-4, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range3.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位3資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range3.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range3[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range3[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range3[i].Address = (byte)(0x49 + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[4]);
                this._mpdaControl.SetMsrRange(4);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -100 ~ 100 (uA)共量測21筆
                SmuMsrtValue.Clear();
                for (int i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + (i * 10) + "(uA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-5, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range4.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位4資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range4.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range4[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range4[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range4[i].Address = (byte)(0x5E + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[5]);
                this._mpdaControl.SetMsrRange(5);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -10 ~ 10 (uA)共量測21筆
                SmuMsrtValue.Clear();
                for (int i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + i + "(uA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-6, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range5.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位5資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range5.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range5[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range5[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range5[i].Address = (byte)(0x73 + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[6]);
                this._mpdaControl.SetMsrRange(6);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -1000 ~ 1000 (nA)共量測21筆
                SmuMsrtValue.Clear();
                for (int i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + (i*100) + "(nA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-7, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range6.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位6資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range6.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range6[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range6[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range6[i].Address = (byte)(0x88 + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[7]);
                this._mpdaControl.SetMsrRange(7);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -100 ~ 100 (nA)共量測21筆
                SmuMsrtValue.Clear();
                for (int i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + (i * 10) + "(nA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-8, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range7.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位7資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range7.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range7[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range7[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range7[i].Address = (byte)(0x9D + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[8]);
                this._mpdaControl.SetMsrRange(8);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -10 ~ 10 (nA)共量測21筆
                SmuMsrtValue.Clear();
                for (int i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + i + "(nA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-9, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range7.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位8資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range8.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range8[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range8[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range8[i].Address = (byte)(0xB2 + i);
                }
                this.updataStatus();
                //////////
                this.nextStatus("設定檔位到" + Range[9]);
                this._mpdaControl.SetMsrRange(9);
                this.updataStatus();
                //
                this.nextStatus("清除資料");
                this._mpdaControl.ClearBuffer();
                this.updataStatus();
                /// Loop 輸出電流 -1000 ~ 1000 (pA)共量測21筆
                SmuMsrtValue.Clear();
                for (int i = -10; i <= 10; i++)
                {
                    this.nextStatus("輸出電流" + (i*100) + "(pA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCmd("SiMi(" + i + "e-10, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.updataStatus();
                }
                //
                this.nextStatus("MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(this._callibrationData.CurrentCalibrate.Range7.Length);
                this.updataStatus();
                //
                this.nextStatus("暫存檔位9資料");
                for (int i = 0; i < this._callibrationData.CurrentCalibrate.Range9.Length; i++)
                {
                    this._callibrationData.CurrentCalibrate.Range9[i] = new Item2();
                    this._callibrationData.CurrentCalibrate.Range9[i].Value = SmuMsrtValue[i] - temp[i];//差值
                    this._callibrationData.CurrentCalibrate.Range9[i].Address = (byte)(0xB2 + i);
                }
                this.updataStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BiasCalibration() 
        {
            // progress bar
            this.pgb.Visible = true;
            this.pgb.Minimum = 0;
            this.pgb.Maximum = 20;
            this.pgb.Value = 1;
            this.pgb.Step = 1;
            txtStatus.Text = string.Empty;
            this.Refresh();
            //
            int repeatTime = 10;
            List<double> SmuMsrtValue = new List<double>(); //存取SMU量測V值
            //
            this._subFormAgent.MessageBox("換線，F+接到Bias(BNC外層)，F-接到地");
            //
            try
            {
                this.nextStatus("設定SMU為Measure V 模式");
                this._k2601Control.SetMsrtV();
                this.updataStatus();
                //
                this.nextStatus("打開 MPDA Bias");
                this._mpdaControl.SetMsrRange(3, 1, 1, false, true);
                this.updataStatus();
                //
                for (int j = -5; j <= 5; j++)
                {
                    SmuMsrtValue.Clear();
                    this.nextStatus("輸出Bias" + j + "V, SMU傳輸資料");
                    this._mpdaControl.SetBiasVoltage(j);
                    Thread.Sleep(150); //放慢 確保輸出BIAS
                    for (int i = 0; i < repeatTime; i++)
                    {
                        Double.TryParse(this._k2601Control.TrigMsrtV(), out double result);
                        SmuMsrtValue.Add(result);
                    }
                    this.updataStatus();
                    this.nextStatus("暫存資料");
                    this._callibrationData.BiasCalibrate.TestItem[j+5] = new Item2();
                    this._callibrationData.BiasCalibrate.TestItem[j + 5].Address = 0x00; ///暫時填值 日後修改
                    this._callibrationData.BiasCalibrate.TestItem[j + 5].Value = this.Similar(SmuMsrtValue.ToArray(), j);
                    this.updataStatus();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private double Similar(double[] data, double target) 
        {
            double ans = data[0];
            for (int i = 0; i < data.Length-1; i++)
            {
                double temp1 = Math.Abs(ans - target);
                double temp2 = Math.Abs(data[i+1] - target);
                if (temp1 > temp2)
                {
                    ans = data[i + 1];
                }
            }
            return ans;
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

        private void btnCurrent_Click(object sender, EventArgs e)
        {
            this._callibrationData = this.DeSerializeXML();
            this.CurrentCalibration();
            this.SerializeXML();

        }

        private void btnMPDAConnect_Click(object sender, EventArgs e)
        {
            this.MPDAconnect();
        }

        private void btnSMUConnect_Click(object sender, EventArgs e)
        {

            this.SMUconnect();
        }




        #endregion

        private void btnBias_Click(object sender, EventArgs e)
        {
            this.Similar(new double[3] { 1.2, 1.5, 2.8 }, 2);
            this.BiasCalibration();
        }
    }
}
