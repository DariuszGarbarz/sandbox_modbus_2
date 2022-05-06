using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxModbus2.Models
{
    public class SensorModel
    {
        public int SensorNumber { get; set; }  
        public ushort SensorStatus { get; set; }
        public ushort CurrentTemperature { get; set; }
        public ushort LowerLimit { get; set; }
        public ushort HigherLimit { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is SensorModel))
                return false;

            var other = obj as SensorModel;

            if (SensorStatus != other.SensorStatus || CurrentTemperature != other.CurrentTemperature || LowerLimit != other.LowerLimit || HigherLimit != other.HigherLimit)
                return false;

            return true;
        }

        public static bool operator ==(SensorModel x, SensorModel y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(SensorModel x, SensorModel y)
        {
            return !(x == y);
        }
    }
}
