using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device.MPDA
{
    public class MPDAMeasureParameter
    {
        public int IntegrationTimes;
        public int IntervalTimes;
        public int DelayTimeSet;
        public int TimeBase;
        public decimal BiasVoltage;
        public byte MeasureRange;
        public int TriggerMode;

        public MPDAMeasureParameter() 
        {
            // Default value
            IntegrationTimes = 10;
            IntervalTimes = 1;
            DelayTimeSet = 10;
            TimeBase = 10;
            BiasVoltage = 0;
            MeasureRange = 9;
            TriggerMode = 1;
        }

    }
}
