using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device.Communication;

namespace GUI
{
    public class SubFormAgent
    {

        public void MessageBox(string message) 
        {
            frmMessegeBox frmMes = new frmMessegeBox(message);
            frmMes.ShowDialog();
        }

        public TcpSetting MPDAConnect() 
        {
            frmMPDAConnect frmMPDA = new frmMPDAConnect();
            frmMPDA.ShowDialog();           
            return frmMPDA.TcpSet;
        }

        public TcpSetting SMUConnect()
        {
            frmSMUConnect frmSMU = new frmSMUConnect();
            frmSMU.ShowDialog();
            return frmSMU.TcpSet;
        }

    }
}
