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
        public List<SensorModel> Sensors { get; set;}

        public override bool Equals(object obj)
        {
            if (!(obj is DeviceModel))
                return false;
           
            var other = obj as DeviceModel;
            for (int i = 0; i < Sensors.Count; i++)
            {
                bool HasSensorDataChanged = Sensors[i].SensorStatus != other.Sensors[i].SensorStatus || Sensors[i].CurrentTemperature != other.Sensors[i].CurrentTemperature
                    || Sensors[i].LowerLimit != other.Sensors[i].LowerLimit || Sensors[i].HigherLimit != other.Sensors[i].HigherLimit;

                if (SystemStatus != other.SystemStatus || DeviceName != other.DeviceName || HasSensorDataChanged)
                    return false;
            }
            return true;
        }

        public static bool operator ==(DeviceModel x, DeviceModel y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(DeviceModel x, DeviceModel y)
        {
            return !(x == y);
        }
    }
}
