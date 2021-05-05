using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSeting
{
    [Serializable]
    public class CallibrationData
    {
        public ZeroCalibrate ZeroCalibrate = new ZeroCalibrate();
        public OffsetCalibrate OffsetCalibrate = new OffsetCalibrate();

    }

    [Serializable]
    public class ZeroCalibrate
    {
        public Item[] TestItem = new Item[2];
    }

    [Serializable]
    public class OffsetCalibrate
    {
        public Item[] TestItem = new Item[9];
    }

    [Serializable]
    public class Item 
    {
        public byte Address;
        public byte[] Value;
    }
}
