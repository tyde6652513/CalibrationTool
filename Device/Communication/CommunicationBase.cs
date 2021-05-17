using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace Device.Communication
{
    /// <summary>
    /// Communication給客戶端和主機端共用，可傳送接收訊息
    /// </summary>
    public class CommunicationBase
    {
        private TcpSetting _tcpSetting;
        private TcpClient _client;
        private Exception _exception;
        private Stopwatch _sw = new Stopwatch();
        private int _waitResponseTime = 25000; //ms
        private int _waitDataToBuffer = 100; //ms
        private List<byte> _returnBytes = new List<byte>();
        

        

        #region >>> Constructor <<<

        public CommunicationBase(TcpSetting tcpSetting) 
        {
            this._tcpSetting = tcpSetting;
        }
        
        #endregion

        #region >>> Public Property <<<
        public delegate void UpdateStatusDel(string status);
        public UpdateStatusDel DUpdateStatus;
        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }
        /// <summary>
        /// Return a byte array,that receive from previous command
        /// </summary>
        public byte[] ReturnBytes
        {
            get { return _returnBytes.ToArray(); }
        }
        #endregion

        #region >>> Public Method <<<

        /// <summary>
        /// Start Connect
        /// </summary>     
        public bool Connect() 
        {
            try
            {
                // Create Tcp client.
                this._client = new TcpClient(this._tcpSetting.IpAddress, this._tcpSetting.Port);
                if (this._client.Connected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SocketException ex)
            {
                this._exception = ex;
                return false;
            }
        }
        
        public void Close()
        {
            this._client.Close();
        }

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="msg">The command want to send</param>
        public void SendCommand(string cmd)
        {
            //NetworkStream ns = this._client.GetStream();
            //if (ns.CanWrite)
            //{
            //    int numberOfData = cmd.Length/2;
            //    byte[] msgByte = new byte[numberOfData];
            //    for (int i = 0; i < numberOfData; i++)
            //    {
            //        msgByte[i] = Convert.ToByte(cmd.Substring(0+i*2, 2), 16);
            //    }
            //    ns.Write(msgByte, 0, msgByte.Length);
            //}
            //while (this._sw.ElapsedMilliseconds < _waitResponseTime)
            //{
            //    this._sw.Start();
            //    this._returnBytes.Clear();
            //    if (ns.CanRead && ns.DataAvailable)
            //    {
            //        byte[] receiveBytes = new byte[this._client.ReceiveBufferSize];
            //        int numberOfBytesRead = 0;
            //        numberOfBytesRead = ns.Read(receiveBytes, 0, this._client.ReceiveBufferSize);                   
            //        for (int i = 0; i < numberOfBytesRead; i++)
            //        {
            //            this._returnBytes.Add(receiveBytes[i]);
            //        }
            //        this._sw.Stop();
            //        this._sw.Reset();
            //        DUpdateStatus.Invoke("Data is response"); //UI介面觀察狀態
            //        return;
            //    }
            //}
            //this._sw.Stop();
            //this._sw.Reset();
            //DUpdateStatus.Invoke("No Response");  //UI介面觀察狀態 
            NetworkStream ns = this._client.GetStream();
            if (ns.CanWrite)
            {
                cmd = string.Format("{0}\n", cmd);
                byte[] msgByte = Encoding.ASCII.GetBytes(cmd);
                ns.Write(msgByte, 0, msgByte.Length);
            }              
        }
        
        public void SendCommand(byte[] byteCmd) 
        {
            NetworkStream ns = this._client.GetStream();
            if (ns.CanWrite)
            {
                ns.Write(byteCmd, 0, byteCmd.Length);
            }
            if (byteCmd[0] == 0x44)
            {
                Thread.Sleep(_waitDataToBuffer); //wait data push to buffer
            }
            this._returnBytes.Clear();
            while (this._sw.ElapsedMilliseconds < _waitResponseTime)
            {
                this._sw.Start();               
                if (ns.CanRead && ns.DataAvailable)
                {
                    byte[] receiveBytes = new byte[this._client.ReceiveBufferSize];
                    int numberOfBytesRead = 0;
                    numberOfBytesRead = ns.Read(receiveBytes, 0, this._client.ReceiveBufferSize);                   
                    for (int i = 0; i < numberOfBytesRead; i++)
                    {
                        this._returnBytes.Add(receiveBytes[i]);
                    }
                    this._sw.Stop();
                    this._sw.Reset();
                    //DUpdateStatus.Invoke("Data is response"); //UI介面觀察狀態
                    return;
                }
            }
            this._returnBytes.Add(0x00);
            this._returnBytes.Add(0x00);//若無回傳時 補0 方便後續判斷時無bug
            this._sw.Stop();
            this._sw.Reset();
            //DUpdateStatus.Invoke("No Response");  //UI介面觀察狀態
        }
        
        public void SendCommand(byte byteSend) 
        {
            byte[] byteCmd = new byte[1] { byteSend };
            NetworkStream ns = this._client.GetStream();
            if (ns.CanWrite)
            {
                ns.Write(byteCmd, 0, byteCmd.Length);
            }
            if (byteCmd[0] == 0x58)
            {
                Thread.Sleep(_waitDataToBuffer); //wait data push to buffer
            }
            this._returnBytes.Clear();
            while (this._sw.ElapsedMilliseconds < _waitResponseTime)
            {               
                this._sw.Start();
                if (ns.CanRead && ns.DataAvailable)
                {
                    byte[] receiveBytes = new byte[this._client.ReceiveBufferSize];
                    int numberOfBytesRead = 0;
                    numberOfBytesRead = ns.Read(receiveBytes, 0, this._client.ReceiveBufferSize);
                    for (int i = 0; i < numberOfBytesRead; i++)
                    {
                        this._returnBytes.Add(receiveBytes[i]);
                    }
                    this._sw.Stop();
                    this._sw.Reset();
                    //DUpdateStatus.Invoke("Data is response"); //UI介面觀察狀態
                    return;
                }
            }
            this._returnBytes.Add(0x00);
            this._returnBytes.Add(0x00);//若無回傳時 補0 方便後續判斷時無bug
            this._sw.Stop();
            this._sw.Reset();
            //DUpdateStatus.Invoke("No Response");  //UI介面觀察狀態
        }
        
        /// <summary>
        /// Receive responce
        /// </summary>
        /// <param name="readOffset">Read data from this position to end</param>
        /// <returns>Response of Device</returns>
        /// 
        public string Receive(int readOffset)
        {
            string receiveMsg = string.Empty;
            byte[] receiveBytes = new byte[this._client.ReceiveBufferSize];
            int numberOfBytesRead = 0;
            NetworkStream ns = this._client.GetStream();
            while (this._sw.ElapsedMilliseconds < 50)
            {
                this._sw.Start();
                if (ns.CanRead && ns.DataAvailable)
                {
                    numberOfBytesRead = ns.Read(receiveBytes, 0, this._client.ReceiveBufferSize);
                    receiveMsg = Encoding.ASCII.GetString(receiveBytes, 0, numberOfBytesRead);
                    this._sw.Stop();
                    this._sw.Reset();
                    return receiveMsg;
                }
            }
            this._sw.Stop();
            this._sw.Reset();
            return "No Response";
        }

        #endregion
    }
}