using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Device;
using Device.Communication;

namespace GUI
{
    public partial class frmSMUConnect : Form
    {
        //委派給主視窗
        //public delegate void DSetTcp(TcpSetting tcpSet);
        //public DSetTcp SetTcp;
        public TcpSetting TcpSet = new TcpSetting();

        //

        public frmSMUConnect()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            TcpSet.IpAddress = txtIP.Text;
            TcpSet.Port = Convert.ToInt32(txtPort.Text);
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void frmMPDAConnect_Load(object sender, EventArgs e)
        {
            txtIP.Text = GUI.Properties.Settings.Default.SMUIP;
            txtPort.Text = GUI.Properties.Settings.Default.SMUPort;
        }

        private void frmMPDAConnect_FormClosed(object sender, FormClosedEventArgs e)
        {
            GUI.Properties.Settings.Default.SMUIP = txtIP.Text;
            GUI.Properties.Settings.Default.SMUPort = txtPort.Text;
            GUI.Properties.Settings.Default.Save();
        }



    }
}
