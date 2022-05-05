using NModbus;
using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public class ModbusReadData : IModbusReadData
    {
        public async Task<DeviceModel> ReadData(ITcpClientFactory tcpClientFactory, byte slaveNumber)
        {
            try
            {
                var systemStatus = await SystemStatusRead(tcpClientFactory.Master, slaveNumber);

                var deviceName = await DeviceNameRead(tcpClientFactory.Master, slaveNumber);

                var sensors = await SensorsRead(tcpClientFactory.Master, ModbusSettings.sensorsNumber, slaveNumber);

                DeviceModel deviceModel = new DeviceModel()
                {
                    SystemStatus = systemStatus,
                    DeviceName = deviceName,
                    Sensors = sensors,
                    SlaveNumber = slaveNumber
                };

                return deviceModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DeviceModel();
            }
        }

        public async Task<ushort> SystemStatusRead(IModbusMaster master, byte slaveNumber)
        {
            try
            {
                var systemStatus = (await master.ReadHoldingRegistersAsync(slaveNumber, ModbusSettings.systemStatusStartAdress, ModbusSettings.systemStatusNumberOfPoints));
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
                var deviceName = await master.ReadHoldingRegistersAsync(slaveNumber, ModbusSettings.deviceNameStartAdress, ModbusSettings.deviceNameNumberOfPoints);
                Encoding ascii = Encoding.ASCII;

                var bytes = new byte[ModbusSettings.deviceNameNumberOfPoints-1];

                for (int i = 0; i < ModbusSettings.deviceNameNumberOfPoints - 1; i++)
                {
                    bytes[i] = (byte)(deviceName[i]);
                }

                String decodedString = ascii.GetString(bytes);

                return decodedString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task<List<SensorModel>> SensorsRead(IModbusMaster master, int sensorsNumber, byte slaveNumber)
        {
            try
            {
                List<SensorModel> sensors = new List<SensorModel>();

                for (int i = 1; i < sensorsNumber + 1; i++)
                {
                    int startAdress = i * ModbusSettings.sensorStartAdress;
                    var sensorData = await master.ReadHoldingRegistersAsync(slaveNumber, (ushort)startAdress, ModbusSettings.sensorNumberOfPoints);
                    SensorModel sensorModel = new SensorModel() 
                    { 
                        SensorNumber = i,
                        SensorStatus = sensorData[0],
                        CurrentTemperature = sensorData[1],
                        LowerLimit = sensorData[2],
                        HigherLimit = sensorData[3],
                    };
                    sensors.Add(sensorModel);
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
