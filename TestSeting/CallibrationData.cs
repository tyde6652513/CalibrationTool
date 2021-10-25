using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TestSeting
{
    [Serializable]
    public class CallibrationData
    {
        
        public ZeroCalibrate ZeroCalibrate = new ZeroCalibrate();
        public OffsetCalibrate OffsetCalibrate = new OffsetCalibrate();
        public CurrentCalibrate CurrentCalibrate = new CurrentCalibrate();
        public BiasCalibrate BiasCalibrate = new BiasCalibrate();
    }

    [Serializable]
    public static class CalDataCenter
    {
        //public static CallibrationData CalData;

        public static SerializableDic<string, string> CalInf = new SerializableDic<string, string>
        {
            {"Time",""},
            {"CalNum",""},
            {"IP","" },
            {"SubMask","" },
            {"DefaultGateWay","" },
            {"Port","" }
        };      
    }


    [Serializable]
    public class ZeroCalibrate
    {
        public List<Item> TestItem = new List<Item>();
        //public Item[] TestItem = new Item[2];
    }

    [Serializable]
    public class OffsetCalibrate
    {
        public List<Item> TestItem = new List<Item>();
    }

    [Serializable]
    public class CurrentCalibrate 
    {
        public List<Item2> Range1 = new List<Item2>();
        public List<Item2> Range2 = new List<Item2>();
        public List<Item2> Range3 = new List<Item2>();
        public List<Item2> Range4 = new List<Item2>();
        public List<Item2> Range5 = new List<Item2>();
        public List<Item2> Range6 = new List<Item2>();
        public List<Item2> Range7 = new List<Item2>();
        public List<Item2> Range8 = new List<Item2>();
        public List<Item2> Range9 = new List<Item2>();
        //public Item2[] Range1 = new Item2[13];
        //public Item2[] Range2 = new Item2[21];
        //public Item2[] Range3 = new Item2[21];
        //public Item2[] Range4 = new Item2[21];
        //public Item2[] Range5 = new Item2[21];
        //public Item2[] Range6 = new Item2[21];
        //public Item2[] Range7 = new Item2[21];
        //public Item2[] Range8 = new Item2[21];
        //public Item2[] Range9 = new Item2[21];
    }

    [Serializable]
    public class BiasCalibrate 
    {
        public List<Item1> TestItem = new List<Item1>();
        //public Item1[] TestItem = new Item1[21];
    }

    [Serializable]
    public class Item 
    {
        public byte Address;
        public string Value;

        //一定要返回 長度4 
        public byte[] GetCmd() 
        {
            //bool flag = true;
            if (Value==null)
            {
                throw new Exception("指令無讀入");
            }

            string[] strArr = Value.Split('-');
            List<byte> returnByte = new List<byte>();

            if (strArr.Length != 4)
            {
                throw new Exception("指令長度不符");
            }

            foreach (var str in strArr)
            {
                byte result;
                if (byte.TryParse(str, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out result))
                {
                    returnByte.Add(result);
                }
                else
                {
                    throw new Exception("指令格式不符");
                }
               // returnByte.Add(Convert.ToByte(str, 16));
            }
            return returnByte.ToArray();
        }
    }

    [Serializable]
    public class Item1 : Item
    {
        //public byte Address;
        public double SMUMsrtValue;
        //public string Value;
        
    }

    [Serializable]
    public class Item2
    {
        public byte Address;
        public double SMUMsrtValue;
        public double MPDAMsrtValue;
        public double DiffValue;
        public double MPDARpt;
    }
}
