using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxModbus2.Models
{
    public class DeviceModel
    {
        public ushort SystemStatus { get; set; }
        public string DeviceName { get; set; }
        public byte SlaveNumber { get; set; }
        public List<SensorModel> Sensors { get; set; }
    }
}
