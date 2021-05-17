using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmCalNumberSet : Form
    {
        private string _calNumber;

        public string CalNumber { get => _calNumber; }

        public frmCalNumberSet()
        {
            InitializeComponent();
        }

        private bool CheckFormat() 
        {
            if (this._calNumber.Length != 8)
            {
                return false;
            }
            foreach (var item in this._calNumber)
            {
                if ( (((int) item) > 122)&& (((int)item) < 48) )
                {
                    return false;
                }
            }
            return true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this._calNumber = textBox1.Text;
            if (!CheckFormat())
            {
                MessageBox.Show("請輸入8位英數字");
                return;
            }           
            this.Close();
        }

        private void frmCalNumberSet_Load(object sender, EventArgs e)
        {
            textBox1.Text = GUI.Properties.Settings.Default.CalNumber;
        }

        private void frmCalNumberSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            GUI.Properties.Settings.Default.CalNumber = textBox1.Text;
            GUI.Properties.Settings.Default.Save();
        }
    }
}
