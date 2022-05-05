using NModbus;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public class ModbusReadData : IModbusReadData
    {
        public async Task ReadData()
        {
            try
            {
                var factory = new ModbusFactory();

                using (var client = new TcpClient())
                using (var master = factory.CreateMaster(client))
                {
                    await client.ConnectAsync(ModbusSettings.hostname, ModbusSettings.port);

                    await SystemStatusRead(master);

                    await DeviceNameRead(master);

                    await SensorsRead(master, ModbusSettings.sensorsNumber);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SystemStatusRead(IModbusMaster master)
        {
            try
            {
                var systemStatus = (await master.ReadHoldingRegistersAsync(1, 0, 1));

                switch (systemStatus[0])
                {
                    case 1:
                        Console.WriteLine("System status - normal");
                        break;
                    case 2:
                        Console.WriteLine("System status - fault");
                        break;
                    default:
                        throw new Exception("Connection Problem");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeviceNameRead(IModbusMaster master)
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

                Console.WriteLine($"Device Name: {decodedString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SensorsRead(IModbusMaster master, int sensorsNumber)
        {
            try
            {
                for (int i = 1; i < sensorsNumber + 1; i++)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine($"Sensor number - {i}");
                    Console.WriteLine("--------------------------");
                    int startAdress = i * 100;
                    var sensorData = await master.ReadHoldingRegistersAsync(1, (ushort)startAdress, 4);

                    switch (sensorData[0])
                    {
                        case 1:
                            Console.WriteLine("Sensor status - Online");
                            break;
                        case 2:
                            Console.WriteLine("Sensor status - Alarm");
                            break;
                        case 3:
                            Console.WriteLine("Sensor status - Fault");
                            break;
                        case 4:
                            Console.WriteLine("Sensor status - Disabled");
                            break;
                        default:
                            throw new Exception("Connection Problem");
                    }
                    Console.WriteLine($"Current temperature: {sensorData[1]}");
                    Console.WriteLine($"Lower limit: {sensorData[2]}");
                    Console.WriteLine($"Higher limit: {sensorData[3]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
