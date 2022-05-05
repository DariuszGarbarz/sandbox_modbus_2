using NModbus;
using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public class ConsolePrintData : IConsolePrintData
    {
        private IModbusReadData _modbusReadData;
        public ConsolePrintData(IModbusReadData modbusReadData)
        {
            _modbusReadData = modbusReadData;
        }

        public async Task PrintDataAsync()
        {
            try
            {
                var deviceModel = await _modbusReadData.ReadData();

                Console.WriteLine("-------------------------");
                Console.WriteLine($"Slave number: {deviceModel.SlaveNumber}");

                switch (deviceModel.SystemStatus)
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

                Console.WriteLine($"Device Name:{deviceModel.DeviceName}");

                for (int i = 0; i < deviceModel.Sensors.Count; i++)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine($"Sensor number - {deviceModel.Sensors[i].SensorNumber}");
                    Console.WriteLine("--------------------------");

                    switch (deviceModel.Sensors[i].SensorStatus)
                    {
                        case 1:
                            Console.WriteLine("Sensor status - Online");
                            break;
                        case 2:
                            Console.WriteLine("Sensor status - Alarm");
                            break;
                        case 4:
                            Console.WriteLine("Sensor status - Fault");
                            break;
                        case 8:
                            Console.WriteLine("Sensor status - Disabled");
                            break;
                        default:
                            Console.WriteLine("Sensor status - Unknown");
                            break;
                    }
                    Console.WriteLine($"Current temperature: {deviceModel.Sensors[i].CurrentTemperature}");
                    Console.WriteLine($"Lower limit: {deviceModel.Sensors[i].LowerLimit}");
                    Console.WriteLine($"Higher limit: {deviceModel.Sensors[i].HigherLimit}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
        }
    }
}
