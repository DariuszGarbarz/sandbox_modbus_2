using System;

namespace SandboxModbus2.Modbus
{
    public static class ModbusSettings
    {
        public static string Hostname = "localhost";
        public static int Port = 502;

        public static int SensorsCount = 10;
        public static int SlavesCount = 2;
        public static ushort SystemStatusStartAdress = 0;
        public static ushort SystemStatusNumberOfPoints = 1;
        public static ushort DeviceNameStartAdress = 1;
        public static ushort DeviceNameNumberOfPoints = 32;
        public static ushort SensorStartAdress = 100;
        public static ushort SensorNumberOfPoints = 4;

        public static TimeSpan RefreshDataMs = new TimeSpan(0, 0, 20);
        public static string PrintDecor = "-------------------------";
    }
}
