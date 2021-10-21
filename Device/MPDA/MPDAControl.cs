using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device.Communication;
using TestSeting;



namespace Device.MPDA
{   
    /// <summary>
    /// Base class for MPDA.Provide methods/parameter for setting/getting MPDA set value
    /// Ver 2.0;
    /// </summary>
    public class MPDAControl
    {

        #region >>>private field<<<

        private CommunicationBase _con;
        private List<byte> _byteCmd = new List<byte>();


        //    Default value    //    
        private bool _offset = false;
        private bool _bias = false;
        private byte _filterRange = 1; 
        private byte _secondRange = 1; 

        #endregion

        #region >>>Constructor<<<
        public MPDAControl(TcpSetting tcpSet) 
        {
            this._con = new CommunicationBase(tcpSet);
            if (!this._con.Connect())
            {
                throw this._con.Exception;
            }
            //this._iOSet = new IOSetting(this._con);
        }
        #endregion

        #region >>>public property<<<

        public CommunicationBase Communication
        {
            get { return _con; }
        }

        public int MsrTime
        {
            get 
            {
                this._con.SendCommand(0x38);
                int apertureTime = ( (this._con.ReturnBytes[1] << 24) | (this._con.ReturnBytes[2] << 16) | (this._con.ReturnBytes[3] << 8) | (this._con.ReturnBytes[4]) );
                //int intervalTime = this._con.ReturnBytes[3];
                return apertureTime;
            }
        }

        public int DelayTime
        {
            get
            {
                this._con.SendCommand(0x40);
                int delayTime = ( (this._con.ReturnBytes[1] << 8) | this._con.ReturnBytes[2]);
                int timeBase = ((this._con.ReturnBytes[3] << 8) | this._con.ReturnBytes[4]);
                return delayTime * timeBase;
            }
        }

        public decimal BiasVoltage
        {
            get
            {
                this._con.SendCommand(0x34);
                Int16 biasVoltageDAC = (Int16) ((this._con.ReturnBytes[2] << 8) | this._con.ReturnBytes[3]);
                //Int16 signedMask = 0x7ff; // 07ff -> Mask the signed bit
                decimal biasVoltage = Math.Abs(0x800 - biasVoltageDAC) * 0.005m;
                if ((biasVoltageDAC & 0x0800) == 0)
                {
                    biasVoltage = -biasVoltage;
                }
                return biasVoltage;
            }
        }

        public string FirstRange 
        {
            get 
            {
                this._con.SendCommand(0x32);
                switch (this._con.ReturnBytes[1])
                {
                    case 9:
                        return "1nA";
                    case 8:
                        return "10nA";
                    case 7:
                        return "100nA";
                    case 6:
                        return "1uA";
                    case 5:
                        return "10uA";
                    case 4:
                        return "100uA";
                    case 3:
                        return "1mA";
                    case 2:
                        return "10mA";
                    case 1:
                        return "100mA";
                    default:
                        return string.Empty;
                }
            }
        }

        public int FilterRange 
        {
            get 
            {
                this._con.SendCommand(0x32);
                return this._con.ReturnBytes[2] >> 4;
            }
        }

        public int SecondRange
        {
            get
            {
                this._con.SendCommand(0x32);
                return (this._con.ReturnBytes[2] & 0x0c) >> 2;
            }
        }

        public bool Offset
        {
            get
            {
                this._con.SendCommand(0x32);
                return (this._con.ReturnBytes[2] & 0x02) != 0;
            }
        }

        public bool Bias
        {
            get
            {
                return this._bias;
            }
            set
            {
                this._bias = value;
            }
        }

        public string TriggerMode
        {
            get
            {
                this._con.SendCommand(0x36);
                switch (this._con.ReturnBytes[1])
	            {
                    case (byte) 0x01:
                        return "HD Trigger";                     
                    case (byte)0x02:
                        return "Rise Trigger";
                    case (byte)0x04:
                        return "Window Capture";
                    case (byte)0x08:
                        return "Real-time Translation";
                    case (byte)0x10:
                        return "Bias Trigger";
                    default:
                        return string.Empty;
	            }
            }
        }

        public byte[] ByteCmd { get => _byteCmd.ToArray(); }

        #endregion

        #region >>>private method<<<
        #endregion

        #region >>>public method<<<

        /// <summary>
        /// Clear Buffer
        /// </summary>
        public void ClearBuffer() 
        {
            this._con.SendCommand(0x45);
            if (this._con.ReturnBytes[1] != (byte)0x01)
            {
                throw new Exception("ClearBuffer is failed");
            }
        }

        /// <summary>
        /// Use internal trigger to start measure.
        /// </summary>
        public void InternalTrigger() 
        {
            this._con.SendCommand(0x98);
            if (this._con.ReturnBytes[1] != (byte)0x01)
            {
                throw new Exception("Internal Trigger is failed");
            }
        }

        public void OffsetAdjust() 
        {
            this._con.SendCommand(0x56);
            if (this._con.ReturnBytes[1] != (byte)0x01)
            {
                throw new Exception("OffsetAdjust is failed");
            }
        }

        public byte[] ReadOffset() 
        {
            List<byte> temp = new List<byte>();
            this._con.SendCommand(0x58);
            temp.Add(0);
            temp.Add(0);
            for (int i = 1; i < 3; i++)
            {
                temp.Add(this._con.ReturnBytes[i]);
            }
            
            return temp.ToArray();
        }

        /// <summary>
        /// Set MPDA measure times = apertureTime (us)
        /// </summary>
        /// <param name="apertureTime">Limit = 32767</param>
        public void SetMsrTime(uint apertureTime) 
        {
            this._byteCmd.Clear();
            byte[] temp = BitConverter.GetBytes((uint)apertureTime);
            this._byteCmd.Add(0x37);
            this._byteCmd.Add(temp[3]);
            this._byteCmd.Add(temp[2]);
            this._byteCmd.Add(temp[1]);
            this._byteCmd.Add(temp[0]);
            this._con.SendCommand(this._byteCmd.ToArray());
            if (this._con.ReturnBytes[1] != (byte) 0x01)
            {
                throw new Exception("Measure times set is failed");
            }
        }

        /// <summary>
        /// Set Delay time = DelayTimeSet * Timebase * 100 (ns)
        /// </summary>
        /// <param name="delayTimeSet">Limit = 65535</param>
        /// <param name="timeBase">Limit = 2000</param>
        public void SetDelayTime(int delayTimeSet, int timeBase)
        {
            byte[] temp = BitConverter.GetBytes((ushort)delayTimeSet);
            this._byteCmd.Clear();
            this._byteCmd.Add(0x39);
            //this._byteCmd.Add(0x01); //預設 IO 1 
            this._byteCmd.Add(temp[1]);
            this._byteCmd.Add(temp[0]);
            temp = BitConverter.GetBytes((ushort)timeBase);
            this._byteCmd.Add(temp[1]);
            this._byteCmd.Add(temp[0]);
            this._con.SendCommand(this._byteCmd.ToArray());
            if (this._con.ReturnBytes[1] != (byte)0x01)
            {
                throw new Exception("Delay times set is failed");
            }           
        }

        /// <summary>
        /// Set Bias Voltage
        /// </summary>
        /// <param name="biasVoltage">Range = -10 to +10, resolution is 0.005 (V)</param>
        public void SetBiasVoltage(double biasVoltage)
        {
            Int16 positiveMask = 2048; // 0X0800
            double resolution = 0.005d;
            Int16 biasVoltageDAC = Convert.ToInt16(Math.Abs(biasVoltage) / resolution);
            if (biasVoltage > 0)
            {
                //biasVoltageDAC = (Int16)(biasVoltageDAC | positiveMask); //最左位元補1
                biasVoltageDAC = (Int16) (biasVoltageDAC + positiveMask);
            }
            else
            {
                //biasVoltageDAC = Math.Abs(biasVoltageDAC);
                biasVoltageDAC = (Int16)(positiveMask - biasVoltageDAC);
            }            
            byte[] temp = BitConverter.GetBytes(biasVoltageDAC);
            this._byteCmd.Clear();
            this._byteCmd.Add(0x33);
            this._byteCmd.Add(Convert.ToByte(this._bias));
            this._byteCmd.Add(temp[1]);
            this._byteCmd.Add(temp[0]);
            this._con.SendCommand(this._byteCmd.ToArray());
            if ((this._con.ReturnBytes[1] != 0x01) || (Math.Abs(biasVoltage) > 10.235d))
            {
                throw new Exception("Bias set is failed");
            }
        }

        /// <summary>
        /// Set Measure Range, Filter Range, Offset, Bias
        /// </summary>
        /// <param name="firstRange">(Set value:Range)
        /// 1:100mA; 2:10mA; 3:1mA; 4:100uA; 5:10uA;
        /// 6:1uA; 7:100nA; 8:10nA; 9:1nA
        /// </param>
        public void SetMsrRange(byte firstRange) 
        {
            this._byteCmd.Clear();
            this._byteCmd.Add(0x31);
            this._byteCmd.Add(firstRange);
            //int temp = (this._filterRange << 4) | (this._secondRange << 2) | (Convert.ToByte(this._offset) << 1) | (Convert.ToByte(this._bias));
            this._byteCmd.Add(0x00);
            this._con.SendCommand(this._byteCmd.ToArray());
            if (this._con.ReturnBytes[1] != (byte) 0x01)
            {
                throw new Exception("Measure times set is failed");
            } 
        }

        public void SetMsrRange(byte firstRange, byte filterRange, byte secondRange, bool offset, bool bias) 
        {
            this._filterRange = filterRange;
            this._secondRange = secondRange;
            this._offset = offset;
            this._bias = bias;
            this.SetMsrRange(firstRange);
        }

        /// <summary>
        /// Set Trigger Mode (第二版以後應該用不到)
        /// </summary>
        /// <param name="modeSelect">
        /// 1:硬體觸發; 2:上升觸發; 3:窗型擷取; 4:實時傳輸; 5:Bias觸發;
        /// </param>
        public void SetTriggerMode(int modeSelect) 
        {
            this._byteCmd.Clear();
            this._byteCmd.Add(0x35);
            this._byteCmd.Add( (byte) (0x01 << (modeSelect-1)) );
            this._con.SendCommand(this._byteCmd.ToArray());            
            if (this._con.ReturnBytes[1] != 0x01)
            {
                throw new Exception("Trigger mode set is failed");
            } 
        }

        //public void SetTrigIn(int io = 1, ETrigEdge trigEdge = ETrigEdge.FALLING_EDGE, ETrigType trigType = ETrigType.HW)
        //{
        //    int cmd = 0;
        //    this._byteCmd.Clear();
        //    this._byteCmd.Add(0x61);
        //    this._byteCmd.Add((byte)io);

        //    cmd += (int)trigEdge << 2;
        //    cmd += (int)trigType << 1;

        //    this._byteCmd.Add((byte)cmd);

        //    this._byteCmd.Add(0x00); //filter 先都填0 用不太到

        //    this._con.SendCommand(this._byteCmd.ToArray());
        //    if (this._con.ReturnBytes[1] != 0x01)
        //    {
        //        this._errorMsg = "TrigInIO set is failed";
        //        //throw new Exception("Trigger mode set is failed");
        //    } 
 
        //}

        ///// <summary>
        ///// 只有 io 2~4 可以用作 HW TrigOut,尚未想到做法可卡住
        ///// </summary>
        ///// <param name="io">
        ///// just for 2~4 (20211007)
        ///// </param>
        ///// <param name="delayTime">
        ///// (0.1us)  delayTime = 0~65535
        ///// </param>
        ///// <param name="pulseWidth">
        ///// (0.1us)  pulseWidth = 0~65535, default=10us
        ///// </param>
        //public void SetTrigOut(int io = 2,ETrigLevel trigLevel = ETrigLevel.High, ETrigOutForm trigOutForm = ETrigOutForm.Pulse, 
        //                        ETrigType trigType = ETrigType.HW, EStimulus stimulus = EStimulus.MsrtStart, int delayTime = 1,
        //                        int pulseWidth = 100) 
        //{
            

        //    #region >>>mode set<<<

        //    int cmd = 0;

        //    this._byteCmd.Clear();
        //    this._byteCmd.Add(0x61); //same as set TrigIn
        //    this._byteCmd.Add((byte)io);

        //    cmd += (int)trigOutForm << 4;
        //    cmd += (int)trigLevel << 2;
        //    cmd += (int)trigType << 1;
        //    cmd += 1; //for out , 1

        //    this._byteCmd.Add((byte)cmd);

        //    this._byteCmd.Add((byte)stimulus); 

        //    this._con.SendCommand(this._byteCmd.ToArray());
        //    if (this._con.ReturnBytes[1] != 0x01)
        //    {
        //        this._errorMsg = "TrigOutIO set is failed";              
        //    }  

        //    #endregion

        //    #region >>>Time set<<<

        //    this._byteCmd.Clear();
        //    this._byteCmd.Add(0x63); //same as set TrigIn
        //    this._byteCmd.Add((byte)io);

        //    byte[] temp = BitConverter.GetBytes((ushort)delayTime);
        //    this._byteCmd.Add(temp[1]);
        //    this._byteCmd.Add(temp[0]);

        //    temp = BitConverter.GetBytes((ushort)pulseWidth);
        //    this._byteCmd.Add(temp[1]);
        //    this._byteCmd.Add(temp[0]);

        //    this._byteCmd.Add(0x00);
        //    this._byteCmd.Add(0x01); //Timebase這裡先寫死為最小 100ns 日後有需要再改

        //    this._con.SendCommand(this._byteCmd.ToArray());
        //    if (this._con.ReturnBytes[1] != 0x01)
        //    {
        //        this._errorMsg = "TrigOutIO set is failed";
        //    }

        //    #endregion

        //}

        //public void AssertIO(int io) 
        //{
        //    if ( (io<1) || (io>6) )
        //    {
        //        return;
        //    }
        //    int shift = io - 1;

        //    this._byteCmd.Clear();
        //    this._byteCmd.Add(0x65);
        //    this._byteCmd.Add((byte)(0x01 << shift));
        //    this._con.SendCommand(this._byteCmd.ToArray());
        //    if (this._con.ReturnBytes[1] != 0x01)
        //    {
        //        this._errorMsg = "AssertIO is failed";
        //    } 
        //}



        public void SetToRam(byte ramAddress, byte[] setValue)
        {
            this._byteCmd.Clear();
            this._byteCmd.Add(0x49);
            this._byteCmd.Add(0x00);
            this._byteCmd.Add(ramAddress);
            this._byteCmd.Add(setValue[0]);
            this._byteCmd.Add(setValue[1]);
            this._byteCmd.Add(setValue[2]);
            this._byteCmd.Add(setValue[3]);
            this._con.SendCommand(this._byteCmd.ToArray());
            if (this._con.ReturnBytes[1] != (byte)0x01)
            {
                throw new Exception("Set to ram is failed");
            }
        }

        public void EnableRam() 
        {
            this._byteCmd.Clear();
            this._byteCmd.Add(0x49);
            this._byteCmd.Add(0x00);
            this._byteCmd.Add(0x00);
            this._byteCmd.Add(0x00);
            this._byteCmd.Add(0x00);
            this._byteCmd.Add(0x00);
            this._byteCmd.Add(0x01);
            this._con.SendCommand(this._byteCmd.ToArray());
            if (this._con.ReturnBytes[1] != (byte)0x01)
            {
                throw new Exception("Enable calibrate is failed");
            }
        }

        /// <summary>
        /// Read buffer data
        /// </summary>
        /// <param name="numOfRead">
        /// The number of data want to get
        /// </param>
        public double[] ReadRawData(int numOfRead) 
        {
            if (numOfRead > 1000)
            {
                numOfRead = 1000;
            }
            this._byteCmd.Clear();
            this._byteCmd.Add(0x44);
            byte[] temp = BitConverter.GetBytes((Int16)numOfRead);
            this._byteCmd.Add(temp[1]);
            this._byteCmd.Add(temp[0]);
            this._con.SendCommand(this._byteCmd.ToArray());

            temp = this._con.ReturnBytes;
            double[] readData = new double[numOfRead];

            if (temp.Length < 5*numOfRead) //有時會返回怪東西 待查 先保護
            {
                //this._errorMsg = "MPDA Read Data Error";
                return readData;
            }
            
            for (int i = 0; i < numOfRead; i++)
            {
                int startIndex = i * 5;
                
                int rawValue = (temp[startIndex + 0] << 24) + (temp[startIndex + 1] << 16)
                             + (temp[startIndex + 2] << 8) + (temp[startIndex + 3]);
                //double scale = (10e-9) * ( Math.Pow(10, -(this.Communication.ReturnBytes[startIndex + 4])) ); //20210413 Darcy
                double scale = Math.Pow(10, -((temp[startIndex + 4]) + 9));
                readData[i] = rawValue * scale;
            }
            return readData;
            
        }

        public byte[][] ReadData(int numOfRead) 
        {
            this._byteCmd.Clear();
            this._byteCmd.Add(0x44);
            byte[] temp = BitConverter.GetBytes((Int16)numOfRead);
            this._byteCmd.Add(temp[1]);
            this._byteCmd.Add(temp[0]);
            this._con.SendCommand(this._byteCmd.ToArray());
            temp = this._con.ReturnBytes;
            byte[][] readData = new byte[numOfRead][];

            if (temp.Length < 5 * numOfRead) //有時會返回怪東西 待查 先保護
            {
                //this._errorMsg = "MPDA Read Data Error";
                return readData;
            }

            for (int i = 0; i < numOfRead; i++)
            {
                int startIndex = i * 5;
                byte[] readTempData = new byte[5]; 
                for (int j = 0; j < 5; j++)
                {
                    readTempData[j] = temp[startIndex + j];
                }
                readData[i] = readTempData;
            }
            return readData;
        }

        public void SetAllParameter(MPDAParameter msrItem)
        {
            try
            {
                this._bias = msrItem.BiasEnable; //enable bias
                this.SetMsrRange(msrItem.MeasureRange);
                this.SetMsrTime((uint)msrItem.ApertureTimes);
                this.SetDelayTime(msrItem.DelayTimeSet, msrItem.TimeBase);
                this.SetBiasVoltage(msrItem.BiasVoltage);
                this.SetTriggerMode(msrItem.TriggerMode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion
    }
}
