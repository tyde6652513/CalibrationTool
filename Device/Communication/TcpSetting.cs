using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device.Communication 
{
    /// <summary>
    /// Base class for TCP Connect.
    /// </summary>
    public class TcpSetting
    {
        private string _ipAddress;
        private int _port;

        #region >>>Constructor<<<
        public TcpSetting() 
        {
            this._ipAddress = "192.168.50.2";
            this._port = 8;
        }
        public TcpSetting(string ipAddress, int port) 
        {
            this._ipAddress = ipAddress;
            this._port = port;
        }
        #endregion

        #region >>> Public Property <<<
        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        #endregion
    }
}
