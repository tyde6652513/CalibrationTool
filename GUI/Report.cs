using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestSeting;
using System.Windows.Forms;
using System.IO;

namespace GUI
{
    public class Report
    {
        private StringBuilder _sb;
        private string[] _titleOfZero;
        private string[] _titleOfCur;
        private string[] _titleOfBias;
        private string[] _titleOfQC;

        public Report()
        {
            _sb = new StringBuilder();

            _titleOfZero = new string[3]
            {
                "Ram地址","檔位","校正值(4byte)"
                //"A","B","C","D","E","F"
            };

            _titleOfCur = new string[7]
            {
                "Ram地址","檔位","SMU讀值(A)","MPA讀值(A)","差值(A)","校正值(4byte)","MPDA Repeatability(重現性)"
                //"A","B","C","D","E","F"
            };

            _titleOfBias = new string[3]
            {
                "Ram地址","SMU讀值(A)","校正值(4byte)",
            };

            _titleOfQC = new string[6]
            {
                "檔位","SMU讀值(A)","MPA讀值(A)","差值(A)","閾值(A)","結果"
                //"A","B","C","D","E","F"
            };
        }

        public void OutputExcel(CalibrationData data, string filePath)
        {
            this._sb.Clear();

            //寫校正資訊
            this.WriteCalInf();           

            //寫Zero、Offset資料
            this.WriteZeroAndOffset(data);

            //寫Current資料
            this.WriteCur(data);
          
            //寫Bias資料
            this.WriteBias(data);

            //寫QC資料
            this.WriteQC(data);

            //寫檔案
            try
            {
                File.WriteAllText(filePath, this._sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteCalInf() 
        {
            foreach (var item in CalDataCenter.CalInf)
            {
                this._sb.AppendLine(item.Key + "," + item.Value);
            }
            this._sb.AppendLine(string.Empty);
        }

        private void WriteZeroAndOffset(CalibrationData data) 
        {
            List<string> strList = new List<string>();
            string newLine;

            this._sb.AppendLine("零點、Offset校正資訊");
            newLine = string.Join(",", _titleOfZero);
            this._sb.AppendLine(newLine);

            foreach (var item in data.ZeroCalibrate.TestItem)
            {
                if (item == null)
                {
                    MessageBox.Show("零點校正資料缺失，請檢察xml檔或重新校正並寫入");
                    return;
                }

                strList.Clear();

                //依序 "Ram地址","檔位","校正值(4byte)"
                strList.Add(ExcTran(Convert.ToString(item.Address, 16)));

                strList.Add(ExcTran(CurrentCalSet.Range[3]));

                strList.Add(ExcTran(item.Value));

                newLine = string.Join(",", strList);
                this._sb.AppendLine(newLine);
            }

            int i = 1;

            foreach (var item in data.OffsetCalibrate.TestItem)
            {
                if (item == null)
                {
                    MessageBox.Show("Offset校正資料缺失，請檢察xml檔或重新校正並寫入");
                    return;
                }
                strList.Clear();

                //依序 "Ram地址","檔位","校正值(4byte)"
                strList.Add(ExcTran(Convert.ToString(item.Address, 16)));

                strList.Add(ExcTran(CurrentCalSet.Range[i]));

                strList.Add(ExcTran(item.Value));

                newLine = string.Join(",", strList);

                this._sb.AppendLine(newLine);

                i++;
            }

            this._sb.AppendLine(string.Empty);
        }

        private void WriteCur(CalibrationData data) 
        {
            List<string> strList = new List<string>();
            string newLine;

            this._sb.AppendLine("電流校正資訊");
            newLine = string.Join(",", _titleOfCur);
            this._sb.AppendLine(newLine);

            for (int j = 1; j <= 9; j++)
            {
                foreach (var item in data.CurrentCalibrate[j])
                {
                    if (item == null)
                    {
                        MessageBox.Show("電流校正資料缺失，請檢察xml檔或重新校正並寫入");
                        return;
                    }
                    strList.Clear();

                    strList.Add(ExcTran(Convert.ToString(item.Address, 16)));

                    strList.Add(ExcTran(CurrentCalSet.Range[j]));

                    strList.Add(item.SMUMsrtValue.ToString());

                    strList.Add(item.MPDAMsrtValue.ToString());

                    strList.Add(item.DiffValue.ToString());

                    byte[] temp = item.GetByteArr(j);

                    Array.Reverse(temp);

                    strList.Add(ExcTran(BitConverter.ToString(temp)));

                    strList.Add(item.MPDARpt.ToString());

                    newLine = string.Join(",", strList);
                    this._sb.AppendLine(newLine);
                }
            }

            this._sb.AppendLine(string.Empty);
        }

        private void WriteBias(CalibrationData data) 
        {
            List<string> strList = new List<string>();
            string newLine;

            this._sb.AppendLine("Bias校正資訊");
            newLine = string.Join(",", _titleOfBias);
            this._sb.AppendLine(newLine);

            foreach (var item in data.BiasCalibrate.TestItem)
            {
                if (item == null)
                {
                    MessageBox.Show("Bias校正資料缺失，請檢察xml檔或重新校正並寫入");
                    return;
                }
                strList.Clear();

                strList.Add(ExcTran(Convert.ToString(item.Address, 16)));

                strList.Add(item.SMUMsrtValue.ToString());

                strList.Add(ExcTran(item.Value));

                newLine = string.Join(",", strList);

                this._sb.AppendLine(newLine);
            }

            this._sb.AppendLine(string.Empty);
        }

        private void WriteQC(CalibrationData data)
        {
            List<string> strList = new List<string>();
            string newLine;
            string isPass;
            List<Item2> resultOfQC = new List<Item2>();
            List<double> thresholdOfQC = new List<double>();

            this._sb.AppendLine("QC資訊");
            newLine = string.Join(",", _titleOfQC);
            this._sb.AppendLine(newLine);

            //"檔位","SMU讀值(A)","MPA讀值(A)","差值(A)","閾值(A)","結果"
            for (int j = 1; j <= 9; j++)
            {
                resultOfQC = data.QCTest[j];
                thresholdOfQC = CurrentCalSet.QCThreshold(j);

                if (resultOfQC.Count != thresholdOfQC.Count)
                {
                    MessageBox.Show("xml中資料點數與QC設置中不同，請再做一次QC將其複寫。");
                    break;
                }

                for (int i = 0; i < resultOfQC.Count; i++)
                {
                    if (resultOfQC[i] == null)
                    {
                        MessageBox.Show("QC資料缺失，請檢察xml檔或重新校正並寫入");
                        return;
                    }

                    if (Math.Abs(resultOfQC[i].DiffValue) >= thresholdOfQC[i])
                    {
                        isPass = "Fail";
                    }
                    else
                    {
                        isPass = "Pass";
                    }

                    strList.Clear();

                    strList.Add(ExcTran(CurrentCalSet.Range[j]));

                    strList.Add(resultOfQC[i].SMUMsrtValue.ToString());

                    strList.Add(resultOfQC[i].MPDAMsrtValue.ToString());

                    strList.Add(resultOfQC[i].DiffValue.ToString());

                    strList.Add(thresholdOfQC[i].ToString());

                    strList.Add(isPass);

                    newLine = string.Join(",", strList);
                    this._sb.AppendLine(newLine);
                }
                //foreach (var item in data.QCTest[j])
                //{
                //    if (item == null)
                //    {
                //        MessageBox.Show("QC資料缺失，請檢察xml檔或重新校正並寫入");
                //        return;
                //    }
                //    strList.Clear();

                //    strList.Add(ExcTran(Convert.ToString(item.Address, 16)));

                //    strList.Add(ExcTran(CurrentCalSet.Range[j]));

                //    strList.Add(item.SMUMsrtValue.ToString());

                //    strList.Add(item.MPDAMsrtValue.ToString());

                //    strList.Add(item.DiffValue.ToString());

                //    byte[] temp = item.GetByteArr(j);

                //    Array.Reverse(temp);

                //    strList.Add(ExcTran(BitConverter.ToString(temp)));

                //    strList.Add(item.MPDARpt.ToString());

                //    newLine = string.Join(",", strList);
                //    this._sb.AppendLine(newLine);
                //}
            }

            this._sb.AppendLine(string.Empty);
        }

        private string ExcTran(string str)
        {
            return "=" + "\"" + str + "\"";
        }
    }
}
