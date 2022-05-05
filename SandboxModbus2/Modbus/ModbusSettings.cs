using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxModbus2.Modbus
{
    public static class ModbusSettings
    {
        public static string hostname = "localhost";
        public static int port = 502;

        public static int sensorsNumber = 10;
        public static int slavesNumber = 2;
        public static ushort systemStatusStartAdress = 0;
        public static ushort systemStatusNumberOfPoints = 1;
        public static ushort deviceNameStartAdress = 1;
        public static ushort deviceNameNumberOfPoints = 32;
        public static ushort sensorStartAdress = 100;
        public static ushort sensorNumberOfPoints = 4;

        public static int refreshDataMs = 20000;
        public static string printDecor = "-------------------------";
    }
}
