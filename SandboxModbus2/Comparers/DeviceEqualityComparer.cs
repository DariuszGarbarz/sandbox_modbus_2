using SandboxModbus2.Models;
using System.Collections.Generic;

namespace SandboxModbus2.Comparers
{
    public sealed class DeviceEqualityComparer: IEqualityComparer<DeviceModel>
    {
        public bool Equals(DeviceModel x, DeviceModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.Sensors.Count != y.Sensors.Count) return false;
            for (int sensorNumber = 0; sensorNumber < x.Sensors.Count; sensorNumber++)
            {
                bool HasSensorDataChanged = x.Sensors[sensorNumber].SensorStatus != y.Sensors[sensorNumber].SensorStatus
                    || x.Sensors[sensorNumber].CurrentTemperature != y.Sensors[sensorNumber].CurrentTemperature
                    || x.Sensors[sensorNumber].LowerLimit != y.Sensors[sensorNumber].LowerLimit
                    || x.Sensors[sensorNumber].HigherLimit != y.Sensors[sensorNumber].HigherLimit;

                if (x.SystemStatus != y.SystemStatus
                    || x.DeviceName != y.DeviceName
                    || HasSensorDataChanged)
                    return false;
            }
            return true;
        }

        public int GetHashCode(DeviceModel obj)
        {
            // system.hashcode.combine
            return obj.GetHashCode();
        }
    }
}
