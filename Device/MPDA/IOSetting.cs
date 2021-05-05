using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device.Communication;

namespace Device.MPDA
{
    public class IOSetting
    {
        private byte[] _byteCmd = new byte[2];
        private byte _byteReceive;
        private CommunicationBase _communication;

        public IOSetting(CommunicationBase com) 
        {
            this._communication = com;
            this._byteCmd[0] = 0x51;
            this._byteCmd[1] = 0x00;
        }
       
        #region >>>Public Property<<<
        public bool D1Enabled 
        {
            get 
            {
                this._communication.SendCmd("52");
                _byteReceive = this._communication.ReturnBytes[1];
                return (_byteReceive & 0x01) != 0;
            }
        }
        public bool D2Enabled
        {
            get
            {
                this._communication.SendCmd("52");
                byte _byteReceive = this._communication.ReturnBytes[1];
                return (_byteReceive & 0x02) != 0;
            }
        }
        public bool D3Enabled
        {
            get
            {
                this._communication.SendCmd("52");
                byte _byteReceive = this._communication.ReturnBytes[1];
                return (_byteReceive & 0x04) != 0;
            }
        }
        public bool D4Enabled
        {
            get
            {
                this._communication.SendCmd("52");
                byte _byteReceive = this._communication.ReturnBytes[1];
                return (_byteReceive & 0x08) != 0;
            }
        }
        public bool D5Enabled
        {
            get
            {
                this._communication.SendCmd("52");
                byte _byteReceive = this._communication.ReturnBytes[1];
                return (_byteReceive & 0x10) != 0;
            }
        }
        public bool D6Enabled
        {
            get
            {
                this._communication.SendCmd("52");
                byte _byteReceive = this._communication.ReturnBytes[1];
                return (_byteReceive & 0x20) != 0;
            }
        }
        #endregion
        
        #region >>>Public Method<<<       
        /// <summary>
        /// Enable IO (1 ~ 6)
        /// </summary>
        /// <param name="IOnumber">
        /// 1~6
        /// </param>
        public void Enable(int IOnumber)
        {
            int temp = 0x01 << IOnumber - 1;            
            this._byteCmd[1] = (byte)(this._byteCmd[1] | temp);
            this._communication.SendCmd(this._byteCmd);
        }
        /// <summary>
        /// Disable IO (1 ~ 6)
        /// </summary>
        /// <param name="IOnumber">
        /// 1~6
        /// </param>
        public void Disable(int IOnumber)
        {
            int temp = ~(0x01 << IOnumber - 1);
            this._byteCmd[1] = (byte)(this._byteCmd[1] & temp);
            this._communication.SendCmd(this._byteCmd);
        }
        #endregion
    }
}
