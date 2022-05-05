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
        public async Task<DeviceModel> ReadData()
        {
            try
            {
                var factory = new ModbusFactory();

                using (var client = new TcpClient())
                using (var master = factory.CreateMaster(client))
                {
                    await client.ConnectAsync(ModbusSettings.hostname, ModbusSettings.port);

                    var systemStatus = await SystemStatusRead(master);

                    var deviceName = await DeviceNameRead(master);

                    var sensors = await SensorsRead(master, ModbusSettings.sensorsNumber);

                    DeviceModel deviceModel = new DeviceModel()
                    {
                        SystemStatus = systemStatus,
                        DeviceName = deviceName,
                        Sensors = sensors
                    };

                    return deviceModel;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DeviceModel();
            }
        }

        public async Task<ushort> SystemStatusRead(IModbusMaster master)
        {
            try
            {
                var systemStatus = (await master.ReadHoldingRegistersAsync(1, 0, 1));
                return systemStatus[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<string> DeviceNameRead(IModbusMaster master)
        {
            try
            {
                var deviceName = await master.ReadHoldingRegistersAsync(1, 1, 32);
                Encoding ascii = Encoding.ASCII;

                var bytes = new byte[31];

                for (int i = 0; i < 31; i++)
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

        public async Task<List<SensorModel>> SensorsRead(IModbusMaster master, int sensorsNumber)
        {
            try
            {
                List<SensorModel> sensors = new List<SensorModel>();

                for (int i = 1; i < sensorsNumber + 1; i++)
                {
                    int startAdress = i * 100;
                    var sensorData = await master.ReadHoldingRegistersAsync(1, (ushort)startAdress, 4);
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
