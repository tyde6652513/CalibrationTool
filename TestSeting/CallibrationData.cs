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
        public CurrentCalibrate CurrentCalibrate = new CurrentCalibrate();
        public BiasCalibrate BiasCalibrate = new BiasCalibrate();
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
    public class CurrentCalibrate 
    {
        public Item2[] Range1 = new Item2[13];
        public Item2[] Range2 = new Item2[21];
        public Item2[] Range3 = new Item2[21];
        public Item2[] Range4 = new Item2[21];
        public Item2[] Range5 = new Item2[21];
        public Item2[] Range6 = new Item2[21];
        public Item2[] Range7 = new Item2[21];
        public Item2[] Range8 = new Item2[21];
        public Item2[] Range9 = new Item2[21];
    }

    [Serializable]
    public class BiasCalibrate 
    {
        public Item2[] TestItem = new Item2[11];
    }

    [Serializable]
    public class Item 
    {
        public byte Address;
        public byte[] Value;
    }

    [Serializable]
    public class Item2
    {
        public byte Address;
        public double Value;
    }
}
