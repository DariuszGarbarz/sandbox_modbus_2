using System;
using System.Configuration;

namespace SandboxModbus2.Modbus
{
    public static class ModbusSettings
    {
        public static string Hostname = ConfigurationManager.AppSettings["Hostname"];
        public static int Port = int.Parse(ConfigurationManager.AppSettings["Port"]);

        public static int SensorsCount = int.Parse(ConfigurationManager.AppSettings["SensorsCount"]);
        public static int SlavesCount = int.Parse(ConfigurationManager.AppSettings["SlavesCount"]);
        public static ushort SystemStatusStartAdress = ushort.Parse(ConfigurationManager.AppSettings["SystemStatusStartAdress"]);
        public static ushort SystemStatusNumberOfPoints = ushort.Parse(ConfigurationManager.AppSettings["SystemStatusNumberOfPoints"]);
        public static ushort DeviceNameStartAdress = ushort.Parse(ConfigurationManager.AppSettings["DeviceNameStartAdress"]);
        public static ushort DeviceNameNumberOfPoints = ushort.Parse(ConfigurationManager.AppSettings["DeviceNameNumberOfPoints"]);
        public static ushort SensorStartAdress = ushort.Parse(ConfigurationManager.AppSettings["SensorStartAdress"]);
        public static ushort SensorNumberOfPoints = ushort.Parse(ConfigurationManager.AppSettings["SensorNumberOfPoints"]);

        public static TimeSpan RefreshDataMs = TimeSpan.Parse(ConfigurationManager.AppSettings["RefreshDataMs"]);
        public static string PrintDecor = ConfigurationManager.AppSettings["PrintDecor"];
    }
}
