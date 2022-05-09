using NModbus;
using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public interface IModbusDataReader
    {
        Task<string> DeviceNameRead(IModbusMaster master, byte slaveNumber,
            ushort deviceNameStartAdress, ushort deviceNameNumberOfPoints);
        Task<DeviceModel> ReadData(IModbusMaster modbusMaster, byte slaveNumber, int sensorsCount,
            ushort systemStatusStartAdress, ushort systemStatusNumberOfPoints,
            ushort deviceNameStartAdress, ushort deviceNameNumberOfPoints,
            ushort sensorStartAdress, ushort sensorNumberOfPoints);
        Task<List<SensorModel>> SensorsRead(IModbusMaster master, int sensorsNumber, byte slaveNumber,
            ushort sensorStartAdress, ushort sensorNumberOfPoints);
        Task<ushort> SystemStatusRead(IModbusMaster master, byte slaveNumber,
            ushort systemStatusStartAdress, ushort systemStatusNumberOfPoints);
    }

    public class ModbusDataReader : IModbusDataReader
    {
        public async Task<DeviceModel> ReadData(IModbusMaster modbusMaster, byte slaveNumber, int sensorsCount,
            ushort systemStatusStartAdress, ushort systemStatusNumberOfPoints,
            ushort deviceNameStartAdress, ushort deviceNameNumberOfPoints,
            ushort sensorStartAdress, ushort sensorNumberOfPoints)
        {
                var systemStatus = await SystemStatusRead(modbusMaster, slaveNumber,
                    systemStatusStartAdress, systemStatusNumberOfPoints);

                var deviceName = await DeviceNameRead(modbusMaster, slaveNumber,
                    deviceNameStartAdress, deviceNameNumberOfPoints);

                var sensors = await SensorsRead
                    (modbusMaster, sensorsCount, slaveNumber,
                    sensorStartAdress, sensorNumberOfPoints);

                var device = new DeviceModel()
                {
                    SystemStatus = systemStatus,
                    DeviceName = deviceName,
                    Sensors = sensors,
                    SlaveNumber = slaveNumber
                };

                return device;            
        }

        public async Task<ushort> SystemStatusRead(IModbusMaster master, byte slaveNumber,
            ushort systemStatusStartAdress, ushort systemStatusNumberOfPoints)
        {
            try
            {
                var systemStatus = await master.ReadHoldingRegistersAsync
                    (slaveNumber, systemStatusStartAdress,
                    systemStatusNumberOfPoints);
                return systemStatus[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<string> DeviceNameRead(IModbusMaster master, byte slaveNumber,
            ushort deviceNameStartAdress, ushort deviceNameNumberOfPoints)
        {
            try
            {
                var deviceName = await master.ReadHoldingRegistersAsync
                    (slaveNumber, deviceNameStartAdress,
                    deviceNameNumberOfPoints);

                var decodedString = Encoding.ASCII.
                    GetString(deviceName.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray());

                return decodedString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task<List<SensorModel>> SensorsRead
            (IModbusMaster master, int sensorsCount, byte slaveNumber,
            ushort sensorStartAdress, ushort sensorNumberOfPoints)
        {
            try
            {
                var sensors = new List<SensorModel>();
                //Incoming data is i.e. 16,9C which is represented as 169 ushort
                //temperaturePrecision is to cut off ",9"
                var temperaturePrecision = 10;
                for (var sensorNumber = 1; sensorNumber <= sensorsCount; sensorNumber++)
                {
                    var startAdress = sensorNumber * sensorStartAdress;
                    var sensorData = await master.ReadHoldingRegistersAsync
                        (slaveNumber, (ushort)startAdress, sensorNumberOfPoints);
                    var sensor = new SensorModel() 
                    { 
                        SensorNumber = sensorNumber,
                        SensorStatus = sensorData[0],
                        CurrentTemperature = (ushort)(sensorData[1] / temperaturePrecision),
                        LowerLimit = (ushort)(sensorData[2] / temperaturePrecision),
                        HigherLimit = (ushort)(sensorData[3] / temperaturePrecision),
                    };
                    sensors.Add(sensor);
                }
                return sensors;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<SensorModel>();
            }
        }
    }
}
