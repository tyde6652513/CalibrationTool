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
    public partial class CaliDataStatus : UserControl
    {
        //委派 -> UI更新元件
        public delegate void DRefreshChblst(bool b1,bool b2, bool b3, bool b4, bool b5);

        public CaliDataStatus()
        {
            InitializeComponent();
        }

        public void RefreshByCaliData(TestSeting.CalibrationData data, bool isCallFromMainThread = false) 
        {
            DRefreshChblst dRefreshChblst = new DRefreshChblst(RefreshChbLst);

            bool isZeroDone = true;
            bool isOffsetDone = true;
            bool isCurDone = true;
            bool isBiasDone = true;
            bool isQcDone = true;

            if (data.ZeroCalibrate.TestItem.Count == 0)
            {
                isZeroDone = false;
            }

            if (data.OffsetCalibrate.TestItem.Count == 0)
            {
                isOffsetDone = false;
            }

            for (int i = 1; i <= 9; i++)
            {
                if (data.CurrentCalibrate[i].Count == 0)
                {
                    isCurDone = false;
                    break;
                }
            }            

            if (data.BiasCalibrate.TestItem.Count == 0)
            {
                isBiasDone = false;
            }

            for (int i = 1; i <= 9; i++)
            {
                if (data.QCTest[i].Count == 0)
                {
                    isQcDone = false;
                    break;
                }
            }

            if (isCallFromMainThread)
            {
                IntPtr intPtr = this.Handle;//防止控制代碼尚未跑完 下面就Invoke出去
            }

            this.Invoke(dRefreshChblst, new object[5] { isZeroDone, isOffsetDone, isCurDone, isBiasDone, isQcDone });
        }

        private void RefreshChbLst(bool isZeroDone, bool isOffsetDone, bool isCurDone, bool isBiasDone, bool isQcDone) 
        {           
            this.chkListStatus.SetItemChecked(0, isZeroDone);
            this.chkListStatus.SetItemChecked(1, isOffsetDone);
            this.chkListStatus.SetItemChecked(2, isCurDone);
            this.chkListStatus.SetItemChecked(3, isBiasDone);
            this.chkListStatus.SetItemChecked(4, isQcDone);
        }
    }
}
