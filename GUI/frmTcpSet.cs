using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestSeting;

namespace GUI
{
    public partial class frmTcpSet : Form
    {
        private byte[] _TcpSet = new Byte[13];

        public byte[] TcpSet { get => _TcpSet;}

        public frmTcpSet()
        {
            InitializeComponent();
        }

        private bool CheckFormat() 
        {
            foreach (var item in _TcpSet)
            {
                if (item > 255 || item < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool GetTcpSet()
        {
            try
            {
                _TcpSet[0] = Convert.ToByte(textBox1.Text);
                _TcpSet[1] = Convert.ToByte(textBox2.Text);
                _TcpSet[2] = Convert.ToByte(textBox3.Text);
                _TcpSet[3] = Convert.ToByte(textBox4.Text);
                _TcpSet[4] = Convert.ToByte(txt5.Text);
                _TcpSet[5] = Convert.ToByte(txt6.Text);
                _TcpSet[6] = Convert.ToByte(txt7.Text);
                _TcpSet[7] = Convert.ToByte(txt8.Text);
                _TcpSet[8] = Convert.ToByte(txt9.Text);
                _TcpSet[9] = Convert.ToByte(txt10.Text);
                _TcpSet[10] = Convert.ToByte(txt11.Text);
                _TcpSet[11] = Convert.ToByte(txt12.Text);
                _TcpSet[12] = Convert.ToByte(txt13.Text);

                
                CalDataCenter.CalInf["IP"] = textBox1.Text + "." + textBox2.Text + "." + textBox3.Text + "." + textBox4.Text;
                CalDataCenter.CalInf["SubMask"] = txt5.Text + "." + txt6.Text + "." + txt7.Text + "." + txt8.Text;
                CalDataCenter.CalInf["DefaultGateWay"] = txt9.Text + "." + txt10.Text + "." + txt11.Text + "." + txt12.Text;
                CalDataCenter.CalInf["Port"] = txt13.Text;
                return true;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                return false;
            }
            
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!this.GetTcpSet())
            {
                return;
            }
            
            if (!CheckFormat())
            {
                MessageBox.Show("請輸入0-255數字");
                return;
            }           
            this.Close();
        }

        private void frmTcpSet_Load(object sender, EventArgs e)
        {
            textBox1.Text = GUI.Properties.Settings.Default.txt1;
            textBox2.Text = GUI.Properties.Settings.Default.txt2;
            textBox3.Text = GUI.Properties.Settings.Default.txt3;
            textBox4.Text = GUI.Properties.Settings.Default.txt4;
            txt5.Text = GUI.Properties.Settings.Default.txt5;
            txt6.Text = GUI.Properties.Settings.Default.txt6;
            txt7.Text = GUI.Properties.Settings.Default.txt7;
            txt8.Text = GUI.Properties.Settings.Default.txt8;
            txt9.Text = GUI.Properties.Settings.Default.txt9;
            txt10.Text = GUI.Properties.Settings.Default.txt10;
            txt11.Text = GUI.Properties.Settings.Default.txt11;
            txt12.Text = GUI.Properties.Settings.Default.txt12;
            txt13.Text = GUI.Properties.Settings.Default.txt13;
            
        }

        private void frmTcpSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            GUI.Properties.Settings.Default.txt1 = textBox1.Text;
            GUI.Properties.Settings.Default.txt2 = textBox2.Text;
            GUI.Properties.Settings.Default.txt3 = textBox3.Text;
            GUI.Properties.Settings.Default.txt4 = textBox4.Text;
            GUI.Properties.Settings.Default.txt5 = txt5.Text;
            GUI.Properties.Settings.Default.txt6 = txt6.Text;
            GUI.Properties.Settings.Default.txt7 = txt7.Text;
            GUI.Properties.Settings.Default.txt8 = txt8.Text;
            GUI.Properties.Settings.Default.txt9 = txt9.Text;
            GUI.Properties.Settings.Default.txt10 = txt10.Text;
            GUI.Properties.Settings.Default.txt11 = txt11.Text;
            GUI.Properties.Settings.Default.txt12 = txt12.Text;
            GUI.Properties.Settings.Default.txt13 = txt13.Text;
            GUI.Properties.Settings.Default.Save();
        }
    }
}
