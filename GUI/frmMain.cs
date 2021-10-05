using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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
        #region >>>Dictionary and ENUM<<<

        public Dictionary<int, string> dicRange =
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

        public enum ECalState 
        {
            ZeroCal,
            OffsetCal,
            CurrentCal,
            BiasCal,
            WriteToRam,
            OutputExcel
        }; 
        

        #endregion

        #region >>>field<<<

        private SubFormAgent _subFormAgent = new SubFormAgent();

        //other
        private TcpSetting _tcpSet;
        private MPDAControl _mpdaControl;
        private K2601Control _k2601Control;
        private CallibrationData _callibrationData = new CallibrationData();
        private ECalState _calState;

        //
        const string dirPath = "CaliData";
        const string filePath = "CaliData.xml";

        #endregion

        #region >>>property<<<

        //委派 -> UI更新元件
        public delegate void DNextStatus(string str);
        public delegate void DIniUI(int min, int max);
        public delegate void DupdataStatus();
        public delegate void DResBtn();

        

        //
        public Thread newThread;

        #endregion

        #region >>>Constructor<<<

        public frmMain()
        {
            InitializeComponent();
            this.saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|Excel 2010|*.xlsx";
            this.RestoreBtn();
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
            if (!(File.Exists(dirPath + @"\" + "CalInf.xml")))
            {
                using (FileStream oFileStream = new FileStream(dirPath + @"\" + "CalInf.xml", FileMode.Create))
                {
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(SerializableDic<string, string>));
                    oXmlSerializer.Serialize(oFileStream, CalDataCenter.CalInf);
                    oFileStream.Close();
                }
            }
        }

        #endregion

        #region >>>private method<<<

        private void ThreadWork() 
        {
            DResBtn dResBtn = new DResBtn(this.RestoreBtn);

            //收掉資料流上多餘的東西
            if (this._mpdaControl != null)
            {
                this._mpdaControl.Communication.Receive(0, 100);
            }
            if (this._k2601Control != null)
            {
                this._k2601Control.CommunicationBase.Receive(0, 100);
            }
            switch (_calState)
            {
                case ECalState.ZeroCal:
                    this.ZeroCalibration();
                    break;
                case ECalState.OffsetCal:
                    this.OffsetCalibration();
                    break;
                case ECalState.CurrentCal:
                    this.CurrentCalibration();
                    break;
                case ECalState.BiasCal:
                    this.BiasCalibration();
                    break;
                case ECalState.WriteToRam:
                    this.WriteToRam();
                    break;
                case ECalState.OutputExcel:
                    this.OutputExcel();
                    break;
                default:
                    break;
            }
            this.Invoke(dResBtn);
        }

        private bool MPDAconnect() 
        {
            this._tcpSet = this._subFormAgent.MPDAConnect();
            if (this._tcpSet != null)
            {
                try
                {
                    this._mpdaControl = new MPDAControl(this._tcpSet);
                    this.lblMPDAStatus.Text = "Connected";
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return false;
        }

        private void SMUconnect() 
        {
            this._tcpSet = this._subFormAgent.SMUConnect();
            if (this._tcpSet != null)
            {
                try
                {
                    this._k2601Control = new K2601Control(this._tcpSet);
                    if (!_k2601Control.Config())
                    {
                        throw new Exception("SMU Config is failed");
                    }
                                     
                    this.lblSMUStatus.Text = "Connected";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CloseDevice() 
        {
            if (this._mpdaControl != null)
            {
                this._mpdaControl.Communication.Close();
                this._mpdaControl = null;
            }
            if (this._k2601Control != null)
            {
                this._k2601Control.CommunicationBase.Close();
                this._k2601Control = null;
            }
            this.lblMPDAStatus.Text = "Disconnected";
            this.lblSMUStatus.Text = "Disconnected";
        }

        private void InitialUI(int min, int max) 
        {
            //
            this.tsmMPDAConnect.Enabled = false;
            this.tsmSMUConnect.Enabled = false;
            this.tsmZero.Enabled = false;
            this.tsmOffset.Enabled = false;
            this.tsmCurrent.Enabled = false;
            this.tsmBias.Enabled = false;
            this.tsmWrite.Enabled = false;
            this.tsmOutputExcel.Enabled = false;
            this.tsmOutputExcel.Enabled = false;
            this.tsmPause.Enabled = true;

            // progress bar

            this.txtStatus.Text = string.Empty;
            this.pgb.Visible = true;
            this.pgb.Minimum = min;
            this.pgb.Maximum = max;
            this.pgb.Value = 1;
            this.pgb.Step = 1;
            this.Refresh();
        }

        private void RestoreBtn() 
        {
            //
            this.tsmMPDAConnect.Enabled = true;
            this.tsmSMUConnect.Enabled = true;
            this.tsmZero.Enabled = true;
            this.tsmOffset.Enabled = true;
            this.tsmCurrent.Enabled = true;
            this.tsmBias.Enabled = true;
            this.tsmWrite.Enabled = true;
            this.tsmOutputExcel.Enabled = true;
            this.tsmPause.Enabled = false;                       
            
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
            using (FileStream oFileStream = new FileStream(dirPath + @"\" + filePath, FileMode.Open))
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
            this._callibrationData = this.DeSerializeXML();
            //實作更新UI委派
            DNextStatus dNextStatus = new DNextStatus(nextStatus);
            DIniUI dIniPgb = new DIniUI(InitialUI);
            DupdataStatus dupdataStatus = new DupdataStatus(updataStatus);
            DResBtn dResBtn = new DResBtn(this.RestoreBtn);

            this.Invoke(dIniPgb, new object[2] { 0, 9 });
            try
            {
                this.Invoke(dNextStatus,"清除Flash");
                this._mpdaControl.Communication.SendCommand(new byte[2] { 0x47, 0x03 });
                if (this._mpdaControl.Communication.ReturnBytes[1] != 0x01)
                {
                    MessageBox.Show("清除Flash失敗");
                }
                this.Invoke(dupdataStatus);
                //
                this._subFormAgent.MessageBox("MPDA重新開機(或按下Reset)");
                //
                if (!this.MPDAconnect())
                {
                    this._mpdaControl.Communication.Close();
                    this._mpdaControl = null;
                    throw new Exception("校正程序終止");
                }
                //
                this.Invoke(dNextStatus,"切換檔位到1mA");
                this._mpdaControl.SetMsrRange(3);
                this.Invoke(dupdataStatus);
                //
                this._subFormAgent.MessageBox("短路電路板上的Jumper，進行ADC Driver零點校正 完成後按下一步");
                //
                this.Invoke(dNextStatus,"設定量測時間1.6s");
                //this._mpdaControl.Communication.SendCommand(new byte[5] { 0x93, 0x00, 0x18, 0x6A, 0x00 });
                this._mpdaControl.SetMsrTime(1600000);
                if (this._mpdaControl.Communication.ReturnBytes[1] != 0x01)
                {
                    MessageBox.Show("量測設定失敗");
                }
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus,"量測，存取量測值");
                this._mpdaControl.InternalTrigger();
                temp = this._mpdaControl.ReadData(1)[0];
                this._callibrationData.ZeroCalibrate.TestItem[0] = new Item();
                this._callibrationData.ZeroCalibrate.TestItem[0].Address = 0x14;
                this._callibrationData.ZeroCalibrate.TestItem[0].Value = temp.Take(4).ToArray();
                this.Invoke(dupdataStatus);
                //           
                this._subFormAgent.MessageBox("拔去電路板上的Jumper，短路Input端(BNC內外層短路) 完成後按下一步");
                //
                this.Invoke(dNextStatus,"清除資料");
                this._mpdaControl.ClearBuffer();
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus,"量測，存取量測值");
                this._mpdaControl.InternalTrigger();
                temp = this._mpdaControl.ReadData(1)[0];
                this._callibrationData.ZeroCalibrate.TestItem[1] = new Item();
                this._callibrationData.ZeroCalibrate.TestItem[1].Address = 0x15;
                this._callibrationData.ZeroCalibrate.TestItem[1].Value = temp.Take(4).ToArray();
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus,"將校正值寫入RAM");
                this._mpdaControl.EnableRam();
                for (int i = 0; i < this._callibrationData.ZeroCalibrate.TestItem.Length; i++)
                {
                    this._mpdaControl.SetToRam(this._callibrationData.ZeroCalibrate.TestItem[i].Address, this._callibrationData.ZeroCalibrate.TestItem[i].Value);
                }
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus,"將校正值寫入Flash");
                this._mpdaControl.Communication.SendCommand(new byte[2] { 0x47, 0x02 });
                this.Invoke(dupdataStatus);
                //
                this._subFormAgent.MessageBox("零點校正完成");
            }
            catch (Exception ex)
            {
                this.Invoke(dResBtn);
                MessageBox.Show(ex.Message);
                this.Invoke(dNextStatus,"零點校正終止");
            }
            this.SerializeXML();
         }

        private void OffsetCalibration() 
        {
            
            
            this._callibrationData = this.DeSerializeXML();
            byte[] temp;
            //實作更新UI委派
            DNextStatus dNextStatus = new DNextStatus(nextStatus);
            DIniUI dIniPgb = new DIniUI(InitialUI);
            DupdataStatus dupdataStatus = new DupdataStatus(updataStatus);
            DResBtn dResBtn = new DResBtn(this.RestoreBtn);

            this.Invoke(dIniPgb, new object[2] { 0, 30 });
            try
            {
                //
                this._subFormAgent.MessageBox("拔去Input短路，接上1000Ω電阻 完成後按下一步");
                //
                //
                for (int i = 1; i < 10; i++)
                {
                    this.Invoke(dNextStatus,"切換檔位到" + dicRange[i]);
                    this._mpdaControl.SetMsrRange((byte) i);
                    this.Invoke(dupdataStatus);
                    //
                    this.Invoke(dNextStatus,"Input Offset調整");
                    this._mpdaControl.OffsetAdjust();
                    this.Invoke(dupdataStatus);
                    //
                    this.Invoke(dNextStatus,"存取調整值");
                    temp = this._mpdaControl.ReadOffset();
                    this._callibrationData.OffsetCalibrate.TestItem[i-1] = new Item();
                    this._callibrationData.OffsetCalibrate.TestItem[i-1].Address = (byte) (0x16 + (i-1));
                    this._callibrationData.OffsetCalibrate.TestItem[i-1].Value = temp;
                    this.Invoke(dupdataStatus);
                    //
                }
                //
                this.Invoke(dNextStatus,"將校正值寫入RAM");
                this._mpdaControl.EnableRam();
                for (int i = 0; i < this._callibrationData.OffsetCalibrate.TestItem.Length; i++)
                {
                    this._mpdaControl.SetToRam(this._callibrationData.OffsetCalibrate.TestItem[i].Address, this._callibrationData.OffsetCalibrate.TestItem[i].Value);
                }
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus,"將校正值寫入Flash");
                this._mpdaControl.Communication.SendCommand(new byte[2] { 0x47, 0x02 });
                this.Invoke(dupdataStatus);
                //
                this._subFormAgent.MessageBox("Offset校正完成");
            }
            catch (Exception ex)
            {
                this.Invoke(dNextStatus,"Offset校正終止");
                this.Invoke(dResBtn);
                MessageBox.Show(ex.Message);
            }
            this.SerializeXML();
        }

        private void CurrentCalibration() 
        {
            this._callibrationData = this.DeSerializeXML();
            //實作更新UI委派
            DNextStatus dNextStatus = new DNextStatus(nextStatus);
            DIniUI dIniPgb = new DIniUI(InitialUI);
            DupdataStatus dupdataStatus = new DupdataStatus(updataStatus);
            DResBtn dResBtn = new DResBtn(this.RestoreBtn);

            this.Invoke(dIniPgb, new object[2] { 0, 212 });

            double[] temp;           
            string SmuReturnData;
            List<double> SmuMsrtValue = new List<double>(); //存取SMU量測I值this.Invoke(dupdataStatus);

            try
            {
                this._k2601Control.CommunicationBase.SendCommand("display.smua.measure.func = display.MEASURE_DCAMPS");
                //
                this._subFormAgent.MessageBox("Keithley接線到MPDA，F+接BNC中心，F-接BNC外層");
                //
                this.Invoke(dNextStatus,"設定量測時間1.6s");
                //this._mpdaControl.Communication.SendCommand(new byte[5] { 0x93, 0x00, 0x18, 0x6A, 0x00 });
                this._mpdaControl.SetMsrTime(1600000);
                if (this._mpdaControl.Communication.ReturnBytes[1] != 0x01)
                {
                    MessageBox.Show("量測設定失敗");
                }
                this.Invoke(dupdataStatus);

                /// Loop 輸出電流 -60 ~ 60 (mA)共量測13筆
                #region >>>-60 ~ 60 (mA)<<<

                //
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[1]);
                this._mpdaControl.SetMsrRange(1);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                this.Invoke(dupdataStatus);

                int cnt = 0;
                for (double i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + (i * 10) + "(mA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + (i / 100) + ", 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    // Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位1資料");
                this._callibrationData.CurrentCalibrate.Range1.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0x23 + i);

                    this._callibrationData.CurrentCalibrate.Range1.Add(item2);
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -10 ~ 10 (mA)共量測21筆
                #region >>>-10 ~ 10 (mA)<<<

                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[2]);
                this._mpdaControl.SetMsrRange(2);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (double i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + i + "(mA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-3, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    cnt++;
                    this.Invoke(dupdataStatus);
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位2資料");
                this._callibrationData.CurrentCalibrate.Range2.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0x34 + i);

                    this._callibrationData.CurrentCalibrate.Range2.Add(item2);
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -1000 ~ 1000 (uA)共量測21筆
                #region >>>-1000 ~ 1000 (uA)<<<

                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[3]);
                this._mpdaControl.SetMsrRange(3);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (int i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + (i * 100) + "(uA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-4, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位3資料");
                this._callibrationData.CurrentCalibrate.Range3.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0x49 + i);

                    this._callibrationData.CurrentCalibrate.Range3.Add(item2);                   
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -100 ~ 100 (uA)共量測21筆
                #region >>>-100 ~ 100 (uA)<<<

                
                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[4]);
                this._mpdaControl.SetMsrRange(4);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (int i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + (i * 10) + "(uA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-5, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位4資料");
                this._callibrationData.CurrentCalibrate.Range4.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0x5E + i);

                    this._callibrationData.CurrentCalibrate.Range4.Add(item2);
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -10 ~ 10 (uA)共量測21筆
                #region >>>-10 ~ 10 (uA)<<<

                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[5]);
                this._mpdaControl.SetMsrRange(5);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (int i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + i + "(uA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-6, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位5資料");
                this._callibrationData.CurrentCalibrate.Range5.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0x73 + i);

                    this._callibrationData.CurrentCalibrate.Range5.Add(item2);
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -1000 ~ 1000 (nA)共量測21筆
                #region >>>-1000 ~ 1000 (nA)<<<

                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[6]);
                this._mpdaControl.SetMsrRange(6);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (int i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + (i * 100) + "(nA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-7, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位6資料");
                this._callibrationData.CurrentCalibrate.Range6.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0x88 + i);

                    this._callibrationData.CurrentCalibrate.Range6.Add(item2);
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -100 ~ 100 (nA)共量測21筆
                #region >>>-100 ~ 100 (nA)<<<

                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[7]);
                this._mpdaControl.SetMsrRange(7);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (int i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + (i * 10) + "(nA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-8, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位7資料");
                this._callibrationData.CurrentCalibrate.Range7.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0x9D + i);

                    this._callibrationData.CurrentCalibrate.Range7.Add(item2);
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -10 ~ 10 (nA)共量測21筆
                #region >>>-10 ~ 10 (nA)<<<

                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[8]);
                this._mpdaControl.SetMsrRange(8);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (int i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + i + "(nA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-9, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位8資料");
                this._callibrationData.CurrentCalibrate.Range8.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0xB2 + i);

                    this._callibrationData.CurrentCalibrate.Range8.Add(item2);
                }
                this.Invoke(dupdataStatus);

                #endregion

                /// Loop 輸出電流 -1000 ~ 1000 (pA)共量測21筆
                #region >>>-1000 ~ 1000 (pA)<<<

                //////////
                this.Invoke(dNextStatus, "設定檔位到" + dicRange[9]);
                this._mpdaControl.SetMsrRange(9);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "清除資料");
                this._mpdaControl.ClearBuffer();
                SmuMsrtValue.Clear();
                this.Invoke(dupdataStatus);

                cnt = 0;
                for (int i = -10; i <= 10; i++)
                {
                    this.Invoke(dNextStatus, "輸出電流" + (i * 100) + "(pA), 開始量測");
                    this._k2601Control.CommunicationBase.SendCommand("SiMi(" + i + "e-10, 2)");
                    this._mpdaControl.InternalTrigger();
                    SmuReturnData = this._k2601Control.CommunicationBase.Receive(0);
                    SmuReturnData = SmuReturnData.Replace("\r\n", "");
                    Double.TryParse(SmuReturnData, out double result);
                    SmuMsrtValue.Add(result);
                    //Thread.Sleep(1000); //確保輸出關閉 再打下一次電流
                    this.Invoke(dupdataStatus);
                    cnt++;
                }
                //
                this.Invoke(dNextStatus, "MPDA傳輸資料");
                temp = this._mpdaControl.ReadRawData(cnt);
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus, "暫存檔位9資料");
                this._callibrationData.CurrentCalibrate.Range9.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    Item2 item2 = new Item2();
                    item2.SMUMsrtValue = SmuMsrtValue[i];
                    item2.MPDAMsrtValue = temp[i];
                    item2.DiffValue = SmuMsrtValue[i] - temp[i];//差值
                    item2.Address = (byte)(0xC7 + i);

                    this._callibrationData.CurrentCalibrate.Range9.Add(item2);
                }
                this.Invoke(dupdataStatus); 

                #endregion

                this._subFormAgent.MessageBox("電流校正完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Invoke(dResBtn);
                this.Invoke(dNextStatus,"電流校正終止");
            }
            this.SerializeXML();
            
        }

        private void BiasCalibration() 
        {
            this._callibrationData = this.DeSerializeXML();
            //實作更新UI委派
            DNextStatus dNextStatus = new DNextStatus(nextStatus);
            DIniUI dIniPgb = new DIniUI(InitialUI);
            DupdataStatus dupdataStatus = new DupdataStatus(updataStatus);
            DResBtn dResBtn = new DResBtn(this.RestoreBtn);

            this.Invoke(dIniPgb, new object[2] { 0, 45 });
            // 變數
            double calRange1 = 0.1d;

            List<byte[]> byteCmd = new List<byte[]>(); //存取送給MPDA的CMD
            List<double> SmuMsrt = new List<double>(); //存取SMU量測值
            List<double> difSmuMsrt = new List<double>(); //存取與SMU量測之差值
            //
            this._subFormAgent.MessageBox("換線，F+接到Bias(BNC外層)，F-接到地");
            //
            try
            {
                this.Invoke(dNextStatus,"設定SMU為Measure V 模式");
                this._k2601Control.SetMsrtV();
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus,"打開 MPDA Bias");
                this._mpdaControl.Bias = true;//this._mpdaControl.SetMsrRange(3, 1, 1, false, true);
                this.Invoke(dupdataStatus);
                //
                for (int j = -10; j <= 10; j++)
                {
                    difSmuMsrt.Clear();
                    SmuMsrt.Clear();
                    byteCmd.Clear();
                    this.Invoke(dNextStatus,"輸出Bias" + j + "V, SMU傳輸資料");
                    int increaseFlag = 0;
                    for (double i = -calRange1; i <= calRange1; i = i+0.005d)
                    {
                        double BiasValue = (double) (j + i);
                        this._mpdaControl.SetBiasVoltage(BiasValue);
                        Thread.Sleep(50); //放慢 確保輸出BIAS

                        //檢查 SMU 是否正常返回 若斷線會傳回 No Response
                        if (!Double.TryParse(this._k2601Control.TrigMsrtV(), out double result))
                        {
                            throw new Exception("SMU No Return Value");
                        }
                        
                        SmuMsrt.Add(result);
                        difSmuMsrt.Add(Math.Abs(result - j));
                        if (difSmuMsrt.Count >= 2)
                        {
                            if (difSmuMsrt[difSmuMsrt.Count - 1] > difSmuMsrt[difSmuMsrt.Count - 2])
                            {
                                increaseFlag++;
                            }
                        }                                             
                        byte[] temp = new byte[4] { 0x00, 0x00,
                                      this._mpdaControl.ByteCmd[1], this._mpdaControl.ByteCmd[2] };
                        byteCmd.Add(temp);
                        if (increaseFlag > 3) //表示已經過最接近值 可不用在往下搜尋
                        {
                            break;
                        }
                    }
                    this.Invoke(dupdataStatus);
                    this.Invoke(dNextStatus,"暫存資料");
                    int minIndex = Array.IndexOf(difSmuMsrt.ToArray(), difSmuMsrt.Min());
                    this._callibrationData.BiasCalibrate.TestItem[j+10] = new Item1();
                    this._callibrationData.BiasCalibrate.TestItem[j + 10].Address = (byte) (0xDC + j + 10); //0xDC 開始
                    this._callibrationData.BiasCalibrate.TestItem[j + 10].SMUMsrtValue = SmuMsrt[minIndex];
                    this._callibrationData.BiasCalibrate.TestItem[j + 10].ByteCommand = byteCmd[minIndex];
                    this.Invoke(dupdataStatus);
                }
                //
                this.Invoke(dNextStatus,"關閉 SMU 輸出");
                this._k2601Control.CommunicationBase.SendCommand("smua.source.output = 0");
                this.Invoke(dupdataStatus);
                //
                //
                this.Invoke(dNextStatus,"關閉 MPDA Bias");
                this._mpdaControl.Bias = false;//this._mpdaControl.SetMsrRange(3, 1, 1, false, false);
                this.Invoke(dupdataStatus);
                //
                this._subFormAgent.MessageBox("Bias校正完成");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                this.Invoke(dResBtn);
                this.Invoke(dNextStatus,"Bias校正終止");
            }
            
            this.SerializeXML();



        }

        private void WriteToRam() 
        {
            this._callibrationData = this.DeSerializeXML();
            //實作更新UI委派
            DNextStatus dNextStatus = new DNextStatus(nextStatus);
            DIniUI dIniPgb = new DIniUI(InitialUI);
            DupdataStatus dupdataStatus = new DupdataStatus(updataStatus);
            DResBtn dResBtn = new DResBtn(this.RestoreBtn);

            this.Invoke(dIniPgb, new object[2] { 0, 10 });
            byte address;
            byte[] valueToRam;
            try
            {
                this._mpdaControl.EnableRam();
                //輸入校正序號
                this.Invoke(dNextStatus, "校正序號寫入RAM");
                string calNumber = this._subFormAgent.SetCalNumber();
                CalDataCenter.CalInf["CalNum"] = calNumber;
                byte[] byte1 = ASCIIEncoding.ASCII.GetBytes(calNumber);
                byte[] byte2 = new byte[4];
                Array.Copy(byte1, 0, byte2, 0, 4);
                this._mpdaControl.SetToRam(0x01, byte2);
                Array.Copy(byte1, 4, byte2, 0, 4);             
                this._mpdaControl.SetToRam(0x02, byte2);
                this.Invoke(dupdataStatus);
                //輸入校正日期
                this.Invoke(dNextStatus, "輸入校正日期");
                CalDataCenter.CalInf["Time"] = DateTime.Now.ToString("yyyy-MM-dd");
                string str = CalDataCenter.CalInf["Time"].Replace("-", "");
                byte2[0] = Convert.ToByte(str[0].ToString());//年
                byte2[1] = Convert.ToByte(str[1].ToString());
                byte2[2] = Convert.ToByte(str[2].ToString());
                byte2[3] = Convert.ToByte(str[3].ToString());
                this._mpdaControl.SetToRam(0x03, byte2);
                byte2[0] = Convert.ToByte(str[4].ToString());//月日
                byte2[1] = Convert.ToByte(str[5].ToString());
                byte2[2] = Convert.ToByte(str[6].ToString());
                byte2[3] = Convert.ToByte(str[7].ToString());
                this._mpdaControl.SetToRam(0x04, byte2);
                this.Invoke(dupdataStatus);
                //輸入TCP設定
                this.Invoke(dNextStatus, "輸入TCP設定");
                byte1 = this._subFormAgent.GetTcpSet();
                Array.Copy(byte1, 0, byte2, 0, 4);
                this._mpdaControl.SetToRam(0x0A, byte2);
                Array.Copy(byte1, 4, byte2, 0, 4);
                this._mpdaControl.SetToRam(0x0B, byte2);
                Array.Copy(byte1, 8, byte2, 0, 4);
                this._mpdaControl.SetToRam(0x0C, byte2);
                byte2[0] = 0; byte2[1] = 0; byte2[2] = 0; byte2[3] = byte1[byte1.Length - 1];
                this._mpdaControl.SetToRam(0x0D, byte2);
                this.Invoke(dupdataStatus);
                //

                // 寫電流校正資料
                for (int j = 1; j < 9; j++)
                {
                    Item2[] data;
                    this.Invoke(dNextStatus,dicRange[j] + " 校正資料寫入RAM");
                    switch (j)
                    {
                        case 1:
                            data = _callibrationData.CurrentCalibrate.Range1.ToArray();
                            break;
                        case 2:
                            data = _callibrationData.CurrentCalibrate.Range2.ToArray();
                            break;
                        case 3:
                            data = _callibrationData.CurrentCalibrate.Range3.ToArray();
                            break;
                        case 4:
                            data = _callibrationData.CurrentCalibrate.Range4.ToArray();
                            break;
                        case 5:
                            data = _callibrationData.CurrentCalibrate.Range5.ToArray();
                            break;
                        case 6:
                            data = _callibrationData.CurrentCalibrate.Range6.ToArray();
                            break;
                        case 7:
                            data = _callibrationData.CurrentCalibrate.Range7.ToArray();
                            break;
                        case 8:
                            data = _callibrationData.CurrentCalibrate.Range8.ToArray();
                            break;
                        case 9:
                            data = _callibrationData.CurrentCalibrate.Range9.ToArray();
                            break;
                        default:
                            data = _callibrationData.CurrentCalibrate.Range9.ToArray();
                            break;
                    }
                    foreach (var item in data)
                    {
                        if (item == null)
                        {
                            MessageBox.Show("電流校正資料缺失，請檢察xml檔或重新校正並寫入");
                            return;
                        }
                        address = item.Address;

                        valueToRam = BitConverter.GetBytes(Convert.ToInt32(CheckINT32(item.DiffValue * Math.Pow(10, j + 9))));

                        Array.Reverse(valueToRam);

                        this._mpdaControl.SetToRam(address, valueToRam);
                    }
                    this.Invoke(dupdataStatus);
                }
                //
                this.Invoke(dNextStatus," Bias 校正資料寫入RAM");
                foreach (var item in this._callibrationData.BiasCalibrate.TestItem)
                {
                    if (item == null)
                    {
                        MessageBox.Show("Bias校正資料缺失，請檢察xml檔或重新校正並寫入");
                        return;
                    }
                    this._mpdaControl.SetToRam(item.Address, item.ByteCommand);
                }               
                this.Invoke(dupdataStatus);
                //
                this.Invoke(dNextStatus," 校正資料寫入Flash");
                this._mpdaControl.Communication.SendCommand(new byte[2] { 0x47, 0x02 });
                this.Invoke(dupdataStatus);

                using (FileStream oFileStream = new FileStream(dirPath + @"\" + "CalInf.xml", FileMode.Create))
                {
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(SerializableDic<string, string>));
                    oXmlSerializer.Serialize(oFileStream, CalDataCenter.CalInf);
                    oFileStream.Close();
                }

                this._subFormAgent.MessageBox("校正資料寫入完成");
            }
            catch (Exception ex)
            {
                this.Invoke(dResBtn);
                MessageBox.Show(ex.Message);
            }
            
        }

        private void OutputExcel() 
        {
            this._callibrationData = this.DeSerializeXML();
            StringBuilder sb = new StringBuilder();
            string[] strTitle = new string[6]
            {
                "Ram地址","檔位","SMU讀值(A)","MPA讀值(A)","差值(A)","校正值(4byte)"
                //"A","B","C","D","E","F"
            };
            List<string> strList = new List<string>();
            string newLine;

            //寫校正資訊
            #region >>>寫校正資訊<<<

            using (FileStream oFileStream = new FileStream(dirPath + @"\" + "CalInf.xml", FileMode.Open))
            {
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(SerializableDic<string, string>));
                CalDataCenter.CalInf = (SerializableDic<string, string>)oXmlSerializer.Deserialize(oFileStream);
                oFileStream.Close();
            }

            foreach (var item in CalDataCenter.CalInf)
            {
                newLine = item.Key + "," + item.Value;
                sb.AppendLine(newLine);
            }
            sb.AppendLine(string.Empty);
            //sb.AppendLine(newLine); 

            #endregion

            //寫Title
            newLine = string.Join(",", strTitle);
            sb.AppendLine(newLine);

            //寫Zero資料
            #region >>>寫Zero資料<<<

            foreach (var item in _callibrationData.ZeroCalibrate.TestItem)
            {
                if (item == null)
                {
                    MessageBox.Show("零點校正資料缺失，請檢察xml檔或重新校正並寫入");
                    return;
                }
                strList.Clear();
                strList.Add(ExcTran(Convert.ToString(item.Address, 16)));
                strList.Add(ExcTran(dicRange[3]));
                strList.Add(string.Empty);
                strList.Add(string.Empty);
                strList.Add(string.Empty);
                strList.Add(ExcTran(BitConverter.ToString(item.Value).Replace("-", "")));
                newLine = string.Join(",", strList);
                sb.AppendLine(newLine);
            } 

            #endregion

            //寫Offset資料
            #region >>>寫Offset資料<<<

            int i = 1;
            foreach (var item in _callibrationData.OffsetCalibrate.TestItem)
            {
                if (item == null)
                {
                    MessageBox.Show("Offset校正資料缺失，請檢察xml檔或重新校正並寫入");
                    return;
                }
                strList.Clear();
                strList.Add(ExcTran(Convert.ToString(item.Address, 16)));
                strList.Add(ExcTran(dicRange[i]));
                strList.Add(string.Empty);
                strList.Add(string.Empty);
                strList.Add(string.Empty);
                strList.Add(ExcTran(BitConverter.ToString(item.Value).Replace("-", "")));
                newLine = string.Join(",", strList);
                sb.AppendLine(newLine);
                i++;
            } 

            #endregion

            //寫Current資料
            #region >>>寫Current資料<<<

            for (int j = 1; j <= 9; j++)
            {
                Item2[] data;
                switch (j)
                {
                    case 1:
                        data = _callibrationData.CurrentCalibrate.Range1.ToArray();
                        break;
                    case 2:
                        data = _callibrationData.CurrentCalibrate.Range2.ToArray();
                        break;
                    case 3:
                        data = _callibrationData.CurrentCalibrate.Range3.ToArray();
                        break;
                    case 4:
                        data = _callibrationData.CurrentCalibrate.Range4.ToArray();
                        break;
                    case 5:
                        data = _callibrationData.CurrentCalibrate.Range5.ToArray();
                        break;
                    case 6:
                        data = _callibrationData.CurrentCalibrate.Range6.ToArray();
                        break;
                    case 7:
                        data = _callibrationData.CurrentCalibrate.Range7.ToArray();
                        break;
                    case 8:
                        data = _callibrationData.CurrentCalibrate.Range8.ToArray();
                        break;
                    case 9:
                        data = _callibrationData.CurrentCalibrate.Range9.ToArray();
                        break;
                    default:
                        data = _callibrationData.CurrentCalibrate.Range9.ToArray();
                        break;
                }
                foreach (var item in data)
                {
                    if (item == null)
                    {
                        MessageBox.Show("電流校正資料缺失，請檢察xml檔或重新校正並寫入");
                        return;
                    }
                    strList.Clear();
                    strList.Add(ExcTran(Convert.ToString(item.Address, 16)));
                    strList.Add(ExcTran(dicRange[j]));
                    strList.Add(item.SMUMsrtValue.ToString());
                    strList.Add(item.MPDAMsrtValue.ToString());
                    strList.Add(item.DiffValue.ToString());
                    byte[] temp = BitConverter.GetBytes(Convert.ToInt32(CheckINT32(item.DiffValue * Math.Pow(10, j + 9))));
                    Array.Reverse(temp);
                    strList.Add(ExcTran(BitConverter.ToString(temp).Replace("-", "")));
                    newLine = string.Join(",", strList);
                    sb.AppendLine(newLine);
                }
            }

            #endregion

            //寫Bias資料
            #region >>>寫Bias資料<<<

            foreach (var item in _callibrationData.BiasCalibrate.TestItem)
            {
                if (item == null)
                {
                    MessageBox.Show("Bias校正資料缺失，請檢察xml檔或重新校正並寫入");
                    return;
                }
                strList.Clear();
                strList.Add(ExcTran(Convert.ToString(item.Address, 16)));
                strList.Add(string.Empty);
                strList.Add(item.SMUMsrtValue.ToString());
                strList.Add(string.Empty);
                strList.Add(string.Empty);
                strList.Add(ExcTran(BitConverter.ToString(item.ByteCommand).Replace("-", "")));
                newLine = string.Join(",", strList);
                sb.AppendLine(newLine);
            } 

            #endregion


            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            try
            {
                string filepath = saveFileDialog1.FileName;
                File.WriteAllText(filepath, sb.ToString(), Encoding.UTF8);
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private string ExcTran(string str) 
        {
            return "=" + "\"" + str + "\"";
        }

        private double CheckINT32(double d) 
        {
            if ((d>Int32.MaxValue) || (d<Int32.MinValue))
            {
                return 0;
            }
            else
            {
                return d;
            }
        }

        private bool CheckDevice(bool B) 
        {
            if (B)
            {
                if ((this._mpdaControl == null) || (this._k2601Control == null))
                {
                    MessageBox.Show("MPDA/SMU未連線");
                    return false;
                }
            }
            else
            {
                if (this._mpdaControl == null)
                {
                    MessageBox.Show("MPDA未連線");
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region >>>Event<<<

        private void tsmZero_Click(object sender, EventArgs e)
        {
            if (!this.CheckDevice(false))
            {
                return;
            }           
            this._calState = ECalState.ZeroCal;
            newThread = new Thread(this.ThreadWork);
            newThread.Start();
            
        }

        private void tsmOffset_Click(object sender, EventArgs e)
        {
            if (!this.CheckDevice(false))
            {
                return;
            }
            this._calState = ECalState.OffsetCal;
            newThread = new Thread(this.ThreadWork);
            newThread.Start();
        }

        private void tsmCurrent_Click(object sender, EventArgs e)
        {
            if (!this.CheckDevice(true))
            {
                return;
            }
            this._calState = ECalState.CurrentCal;
            newThread = new Thread(this.ThreadWork);
            newThread.Start();
        }

        private void tsmBias_Click(object sender, EventArgs e)
        {
            //this._mpdaControl.ClearBuffer();
            //this._mpdaControl.InternalTrigger();
            //double[] a = this._mpdaControl.ReadRawData(1);

            if (!this.CheckDevice(true))
            {
                return;
            }

            this._calState = ECalState.BiasCal;
            newThread = new Thread(this.ThreadWork);
            newThread.Start();
        }

        private void tsmWrite_Click(object sender, EventArgs e)
        {
            if (!this.CheckDevice(false))
            {
                return;
            }


            this._calState = ECalState.WriteToRam;
            newThread = new Thread(this.ThreadWork);
            newThread.Start();

        }

        private void tsmOutputExcel_Click(object sender, EventArgs e)
        {
            this.OutputExcel();
        }

        private void tsmSMUConnect_Click(object sender, EventArgs e)
        {
            this.SMUconnect();
        }

        private void tsmMPDAConnect_Click(object sender, EventArgs e)
        {
            this.MPDAconnect();
        }

        private void tsmPause_Click(object sender, EventArgs e)
        {
            this.newThread.Abort();        
        }

        private void tsmDisconnect_Click(object sender, EventArgs e)
        {
            this.CloseDevice();
        }

        private void tsmiPIV_Click(object sender, EventArgs e)
        {
            if (!this.CheckDevice(true))
            {
                return;
            }
            this._subFormAgent.PIVrun(this._mpdaControl, this._k2601Control);
        }

        #endregion


    }
}
