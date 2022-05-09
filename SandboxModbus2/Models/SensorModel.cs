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
    }
}
