using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace TestSeting
{
    /// <summary>
    /// 定義校正 與 QC 各Range所測試的電流
    /// 以List型態
    /// </summary>
    public static class CurrentCalSet
    {
        private static List<double> _calCur1;
        private static List<double> _calCur2;
        private static List<double> _calCur3;
        private static List<double> _calCur4;
        private static List<double> _calCur5;
        private static List<double> _calCur6;
        private static List<double> _calCur7;
        private static List<double> _calCur8;
        private static List<double> _calCur9;

        private static List<double> _qCCur1;
        private static List<double> _qCCur2;
        private static List<double> _qCCur3;
        private static List<double> _qCCur4;
        private static List<double> _qCCur5;
        private static List<double> _qCCur6;
        private static List<double> _qCCur7;
        private static List<double> _qCCur8;
        private static List<double> _qCCur9;

        private static List<double> _qCThreshold1;
        private static List<double> _qCThreshold2;
        private static List<double> _qCThreshold3;
        private static List<double> _qCThreshold4;
        private static List<double> _qCThreshold5;
        private static List<double> _qCThreshold6;
        private static List<double> _qCThreshold7;
        private static List<double> _qCThreshold8;
        private static List<double> _qCThreshold9;

        public static void Init()
        {
            _calCur1 = new List<double>();
            _calCur2 = new List<double>();
            _calCur3 = new List<double>();
            _calCur4 = new List<double>();
            _calCur5 = new List<double>();
            _calCur6 = new List<double>();
            _calCur7 = new List<double>();
            _calCur8 = new List<double>();
            _calCur9 = new List<double>();

            #region >>>電流校正各Range電流設定<<<

            // -100 ~ 100 (mA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur1.Add((i / 100));
            }

            //-10 ~ 10 (mA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur2.Add((i / 1000));
            }

            // -1000 ~ 1000 (uA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur3.Add((i / 10000));
            }

            //-100 ~ 100 (uA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur4.Add((i / 100000));
            }

            //-10 ~ 10 (uA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur5.Add((i / 1000000));
            }

            //-1000 ~ 1000 (nA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur6.Add((i / 10000000));
            }

            //-100 ~ 100 (nA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur7.Add((i / 100000000));
            }

            //-10 ~ 10 (nA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur8.Add((i / 1000000000));
            }

            //-1000 ~1000(pA)
            for (double i = -10; i <= 10; i++)
            {
                _calCur9.Add((i / 10000000000));
            }

            #endregion

            _qCCur1 = new List<double>();
            _qCCur2 = new List<double>();
            _qCCur3 = new List<double>();
            _qCCur4 = new List<double>();
            _qCCur5 = new List<double>();
            _qCCur6 = new List<double>();
            _qCCur7 = new List<double>();
            _qCCur8 = new List<double>();
            _qCCur9 = new List<double>();
           
            _qCThreshold1 = new List<double>();
            _qCThreshold2 = new List<double>();
            _qCThreshold3 = new List<double>();
            _qCThreshold4 = new List<double>();
            _qCThreshold5 = new List<double>();
            _qCThreshold6 = new List<double>();
            _qCThreshold7 = new List<double>();
            _qCThreshold8 = new List<double>();
            _qCThreshold9 = new List<double>();

            string filePath = CalDataCenter.DATA_DIR + @"\" + CalDataCenter.PATH_QCSET; //QC設置檔案           

            if ((File.Exists(filePath)))
            {
                #region >>>將CSV寫入 QC設置資料結構中<<<

                var reader = new StreamReader(File.OpenRead(filePath));

                int readLine = 0;

                //只是中繼點用以指派 不用NEW
                List<double> tempCur;
                List<double> tempThreshold;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    readLine++;

                    if (readLine == 1) //檔頭跳過
                    {
                        continue;
                    }

                    var values = line.Split(',');

                    if (values.Length != 3)
                    {
                        throw new Exception("QC檔案 格式有誤");
                    }

                    int range = 0;

                    double cur;

                    double threshold;

                    foreach (KeyValuePair<int, string> kvp in Range)
                    {
                        if (kvp.Value.Equals(values[0]))
                        {
                            range = kvp.Key;
                            break;
                        }
                    }

                    if (range == 0) //表示 -> Range的值寫錯了 導致找不到
                    {
                        throw new Exception("QC檔案 檔位定義有誤");
                    }

                    tempCur = QCListI(range);

                    tempThreshold = QCThreshold(range);

                    Double.TryParse(values[1], out cur);

                    Double.TryParse(values[2], out threshold);

                    tempCur.Add(cur);

                    tempThreshold.Add(threshold);
                } 

                #endregion
            }
            else
            {
                #region >>>QC 各Range設置 使用For迴圈<<<

                // -100 ~ 100 (mA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur1.Add((i / 100));
                    _qCThreshold1.Add(0);
                }

                //-10 ~ 10 (mA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur2.Add((i / 1000));
                    _qCThreshold2.Add(0);
                }

                // -1000 ~ 1000 (uA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur3.Add((i / 10000));
                    _qCThreshold3.Add(0);
                }

                //-100 ~ 100 (uA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur4.Add((i / 100000));
                    _qCThreshold4.Add(0);
                }

                //-10 ~ 10 (uA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur5.Add((i / 1000000));
                    _qCThreshold5.Add(0);
                }

                //-1000 ~ 1000 (nA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur6.Add((i / 10000000));
                    _qCThreshold6.Add(0);
                }

                //-100 ~ 100 (nA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur7.Add((i / 100000000));
                    _qCThreshold7.Add(0);
                }

                //-10 ~ 10 (nA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur8.Add((i / 1000000000));
                    _qCThreshold8.Add(0);
                }

                //-1000 ~1000(pA)
                for (double i = -10; i <= 10; i = i + 3)
                {
                    _qCCur9.Add((i / 10000000000));
                    _qCThreshold9.Add(0);
                }

                #endregion

                saveQCSetting();
            }
        }

        private static void saveQCSetting() 
        {
            StringBuilder sb = new StringBuilder();
            List<double> lstQCI = new List<double>();
            List<double> lstQCThreshold = new List<double>();

            string strLine = string.Empty;

            strLine = "檔位,測試電流(A),閾值(A)";

            sb.AppendLine(strLine);

            for (int r = 1; r <= 9; r++)
            {
                lstQCI = QCListI(r);

                lstQCThreshold = QCThreshold(r);

                for (int i = 0; i < lstQCI.Count; i++)
                {
                    strLine = Range[r] + ",";

                    strLine += lstQCI[i] + ",";

                    strLine += lstQCThreshold[i];

                    sb.AppendLine(strLine);
                }
            }

            //寫檔案
            File.WriteAllText(CalDataCenter.DATA_DIR + @"\" + CalDataCenter.PATH_QCSET, sb.ToString(), Encoding.UTF8);

        }

        public static List<double> CalListI(int range)
        {
            switch (range)
            {
                case 1:
                    return _calCur1;
                case 2:
                    return _calCur2;
                case 3:
                    return _calCur3;
                case 4:
                    return _calCur4;
                case 5:
                    return _calCur5;
                case 6:
                    return _calCur6;
                case 7:
                    return _calCur7;
                case 8:
                    return _calCur8;
                case 9:
                    return _calCur9;
                default:
                    return _calCur1;
            }
        }

        public static List<double> QCListI(int range)
        {
            switch (range)
            {
                case 1:
                    return _qCCur1;
                case 2:
                    return _qCCur2;
                case 3:
                    return _qCCur3;
                case 4:
                    return _qCCur4;
                case 5:
                    return _qCCur5;
                case 6:
                    return _qCCur6;
                case 7:
                    return _qCCur7;
                case 8:
                    return _qCCur8;
                case 9:
                    return _qCCur9;
                default:
                    return _qCCur1;
            }
        }

        public static List<double> QCThreshold(int range) 
        {
            switch (range)
            {
                case 1:
                    return _qCThreshold1;
                case 2:
                    return _qCThreshold2;
                case 3:
                    return _qCThreshold3;
                case 4:
                    return _qCThreshold4;
                case 5:
                    return _qCThreshold5;
                case 6:
                    return _qCThreshold6;
                case 7:
                    return _qCThreshold7;
                case 8:
                    return _qCThreshold8;
                case 9:
                    return _qCThreshold9;
                default:
                    return _qCThreshold1;
            }
        }

        /// <summary>
        /// key : 檔位
        /// value : 該黨位起始記憶體位址
        /// </summary>
        public static Dictionary<int, byte> Address =
        new Dictionary<int, byte>()
        {
            /// 1:100mA; 2:10mA; 3:1mA; 4:100uA; 5:10uA;
            /// 6:1uA; 7:100nA; 8:10nA; 9:1nA
            {1, 0x1f}, {2, 0x34},
            {3, 0x49}, {4, 0x5e},
            {5, 0x73}, {6, 0x88},
            {7, 0x9d}, {8, 0xb2},
            {9, 0xC7}
        };

        /// <summary>
        /// key : 檔位
        /// value : 該檔位電流(文字表示)
        /// </summary>
        public static Dictionary<int, string> Range =
        new Dictionary<int, string>()
        {
            /// 1:100mA; 2:10mA; 3:1mA; 4:100uA; 5:10uA;
            /// 6:1uA; 7:100nA; 8:10nA; 9:1nA
            {1, "100mA"}, {2, "10mA"},
            {3, "1mA"}, {4, "100uA"},
            {5, "10uA"}, {6, "1uA"},
            {7, "100nA"}, {8, "10nA"},
            {9, "1nA"}
        };

        public static int stepCnt 
        {
            get 
            {
                int re = 0;

                re += _qCCur1.Count + _qCCur2.Count + _qCCur3.Count + _qCCur4.Count + _qCCur5.Count + _qCCur6.Count + _qCCur7.Count + _qCCur8.Count + _qCCur9.Count;

                re += 27; //3*9

                return re;
            }
        }
    }
}
