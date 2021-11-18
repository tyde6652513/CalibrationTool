using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSeting
{
    public enum ECalState
    {
        ZeroCal,
        OffsetCal,
        CurrentCal,
        BiasCal,
        QCTest,
        WriteToRam,
        OutputExcel
    };

    public enum ETestType
    {
        Calibrate = 0,
        QCTest = 1
    }
}
