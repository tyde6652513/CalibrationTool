using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TestSeting
{
    public class PulseParameter
    {
        #region>>>Field<<<

        private double _pulseWidth;
        private double _dutyCycle;
        private decimal _startValue;
        private decimal _endValue;
        private decimal _stepValue;
        private int _clampVoltage;
        
        //private double _count;
        
        #endregion
       
        #region>>>Property<<<

        public double DutyCycle
        {
            get { return _dutyCycle; }
            set { _dutyCycle = value; }
        }

        public double PulseWidth
        {
            get { return _pulseWidth; }
            set { _pulseWidth = value; }
        }

        public decimal StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }

        public decimal EndValue
        {
            get { return _endValue; }
            set { _endValue = value; }
        }

        public decimal StepValue
        {
            get { return _stepValue; }
            set { _stepValue = value; }
        }

        public int Count
        {
            get { return (int) ((_endValue - _startValue) / _stepValue); }
            //get { return (int) Math.Round(((_endValue - _startValue) / _stepValue)); }//((_endValue - _startValue) / _stepValue)
        }

        public int ClampVoltage
        {
            get { return _clampVoltage; }
            set { _clampVoltage = value; }
        }

        #endregion     
    
    }

    public class SwParameter 
    {
        #region>>>Property<<<

        public double Nplc;
        public int MsrRange;
        public int LimitV;
        public double SrcRange;
        public int SwCount;
        public double StartValue;
        public double EndValue;
        public double ForceTime;

        #endregion
        public SwParameter() 
        {
            Nplc = 0.01;
            MsrRange = 10;
            LimitV = 10;
            SrcRange = 0.012;
            StartValue = 10e-3;
            EndValue = 30e-3;
            SwCount = 5;
            ForceTime = 0.3e-3;
        }
    }

    public static class DMMParameter 
    {
        public static double MsrtDelay;
        public static int MsrtCount;
    }

    public class MPDAParameter 
    {
        public bool BiasEnable;
        public int IntegrationTimes;
        public int IntervalTimes;
        public int DelayTimeSet;
        public int TimeBase;
        public decimal BiasVoltage;
        public byte MeasureRange;
        public int TriggerMode;

        public MPDAParameter() 
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
