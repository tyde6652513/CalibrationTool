using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device.Communication;
using System.Windows.Forms;
using Device;

namespace GUI
{
    public class SubFormAgent
    {

        public DialogResult Pause() 
        {
            frmPause frmPause = new frmPause();
            frmPause.ShowDialog();
            return frmPause.DialogResult;
        }

        public void MessageBox(string message) 
        {
            frmMessegeBox frmMes = new frmMessegeBox(message);
            frmMes.ShowDialog();
        }

        public TcpSetting MPDAConnect() 
        {
            frmMPDAConnect frmMPDA = new frmMPDAConnect();
            frmMPDA.ShowDialog();
            if (frmMPDA.DialogResult == DialogResult.Yes)
            {
                return frmMPDA.TcpSet;
            }
            else
            {
                return null;
            }
            
        }

        public TcpSetting SMUConnect()
        {
            frmSMUConnect frmSMU = new frmSMUConnect();
            frmSMU.ShowDialog();
            if (frmSMU.DialogResult == DialogResult.Yes)
            {
                return frmSMU.TcpSet;
            }
            else
            {
                return null;
            }
            
        }

        public String SetCalNumber() 
        {
            frmCalNumberSet frm = new frmCalNumberSet();
            frm.ShowDialog();
            return frm.CalNumber;
        }

        public byte[] GetTcpSet() 
        {
            frmTcpSet frm = new frmTcpSet();
            frm.ShowDialog();
            return frm.TcpSet;
        }

        public void PIVrun(Device.MPDA.MPDAControl m, Device.K2601.K2601Control k) 
        {
            frmMsrtSetMPDA frm = new frmMsrtSetMPDA(m, k);
            frm.ShowDialog();
            return;
        } 

    }
}
