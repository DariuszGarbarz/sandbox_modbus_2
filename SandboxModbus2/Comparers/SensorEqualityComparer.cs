using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxModbus2.Comparers
{
    internal sealed class SensorEqualityComparer: IEqualityComparer<SensorModel>
    {
        public bool Equals(SensorModel x, SensorModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.SensorStatus != y.SensorStatus
                || x.CurrentTemperature != y.CurrentTemperature
                || x.LowerLimit != y.LowerLimit
                || x.HigherLimit != y.HigherLimit) return false;
            return true;
        }

        public int GetHashCode(SensorModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
