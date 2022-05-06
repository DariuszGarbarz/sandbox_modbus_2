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
        Task<string> DeviceNameRead(IModbusMaster master, byte slaveNumber);
        Task<DeviceModel> ReadData(ITcpClientFactory tcpClientFactory, byte slaveNumber);
        Task<List<SensorModel>> SensorsRead(IModbusMaster master, int sensorsNumber, byte slaveNumber);
        Task<ushort> SystemStatusRead(IModbusMaster master, byte slaveNumber);
    }

    public class ModbusDataReader : IModbusDataReader
    {
        public async Task<DeviceModel> ReadData(ITcpClientFactory tcpClientFactory, byte slaveNumber)
        {
                var systemStatus = await SystemStatusRead(tcpClientFactory.Master, slaveNumber);

                var deviceName = await DeviceNameRead(tcpClientFactory.Master, slaveNumber);

                var sensors = await SensorsRead
                    (tcpClientFactory.Master, ModbusSettings.SensorsCount, slaveNumber);

                var device = new DeviceModel()
                {
                    SystemStatus = systemStatus,
                    DeviceName = deviceName,
                    Sensors = sensors,
                    SlaveNumber = slaveNumber
                };

                return device;            
        }

        public async Task<ushort> SystemStatusRead(IModbusMaster master, byte slaveNumber)
        {
            try
            {
                var systemStatus = await master.ReadHoldingRegistersAsync
                    (slaveNumber, ModbusSettings.SystemStatusStartAdress, ModbusSettings.SystemStatusNumberOfPoints);
                return systemStatus[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<string> DeviceNameRead(IModbusMaster master, byte slaveNumber)
        {
            try
            {
                var deviceName = await master.ReadHoldingRegistersAsync
                    (slaveNumber, ModbusSettings.DeviceNameStartAdress, ModbusSettings.DeviceNameNumberOfPoints);

                var decodedString = Encoding.ASCII.
                    GetString(deviceName.SelectMany(x => BitConverter.GetBytes(x)).ToArray());

                return decodedString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task<List<SensorModel>> SensorsRead(IModbusMaster master, int sensorsCount, byte slaveNumber)
        {
            try
            {
                var sensors = new List<SensorModel>();

                for (var sensorNumber = 1; sensorNumber <= sensorsCount; sensorNumber++)
                {
                    var startAdress = sensorNumber * ModbusSettings.SensorStartAdress;
                    var sensorData = await master.ReadHoldingRegistersAsync
                        (slaveNumber, (ushort)startAdress, ModbusSettings.SensorNumberOfPoints);
                    var sensor = new SensorModel() 
                    { 
                        SensorNumber = sensorNumber,
                        SensorStatus = sensorData[0],
                        CurrentTemperature = sensorData[1],
                        LowerLimit = sensorData[2],
                        HigherLimit = sensorData[3],
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
