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
    public partial class frmPause : Form
    {
        public frmPause()
        {
            InitializeComponent();
            lblMessage.Text = "校正已暫停";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Yes;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.No;
        }
    }
}
