using NModbus;
using SandboxModbus2.Enums;
using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public interface IModbusManager
    {
        Task PrintDataAsync(CancellationToken cancellationToken);
    }

    public class ModbusManager : IModbusManager
    {
        private IModbusDataReader _modbusReadData;
        private ITcpClientFactory _tcpClientFactory;
        public ModbusManager(IModbusDataReader modbusReadData, ITcpClientFactory tcpClientFactory)
        {
            _modbusReadData = modbusReadData;
            _tcpClientFactory = tcpClientFactory;
        }

        public async Task PrintDataAsync(CancellationToken cancellationToken)
        {
            try
            {
                List<DeviceModel> deviceList = new List<DeviceModel>();
                for (byte a = 1; a < ModbusSettings.SlavesCount + 1; a++)
                {
                    var deviceModel = await _modbusReadData.ReadData(_tcpClientFactory, a);

                    PrintDeviceData(deviceModel);

                    for (int i = 0; i < deviceModel.Sensors.Count; i++)
                    {
                        PrintSensorsData(deviceModel, i);
                    }   

                        deviceList.Add(deviceModel);
                }

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    for (byte a = 1; a < ModbusSettings.SlavesCount+1; a++)
                    {
                        var deviceModel = await _modbusReadData.ReadData(_tcpClientFactory, a);
                        if (deviceModel != deviceList[a-1])
                        {
                            PrintDeviceData(deviceModel);

                            for (int i = 0; i < deviceModel.Sensors.Count; i++)
                            {
                                if (deviceModel.Sensors[i] != deviceList[a - 1].Sensors[i])
                                {
                                    PrintSensorsData(deviceModel, i);
                                }
                            }
                            deviceList[a - 1] = deviceModel;
                        }
                    }
                    await Task.Delay(ModbusSettings.RefreshDataMs, cancellationToken);
                }
            }
            catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
        }

        public void PrintDeviceData(DeviceModel deviceModel)
        {
            Console.WriteLine(ModbusSettings.PrintDecor);
            Console.WriteLine(DateTime.Now.ToString());
            Console.WriteLine(ModbusSettings.PrintDecor);
            Console.WriteLine($"Slave number: {deviceModel.SlaveNumber}");
            Console.WriteLine($"System Status: {(SystemStatusEnum)deviceModel.SystemStatus}");
            Console.WriteLine($"Device Name:{deviceModel.DeviceName}");
        }

        public void PrintSensorsData(DeviceModel deviceModel, int i)
        {
            Console.WriteLine(ModbusSettings.PrintDecor);
            Console.WriteLine($"Sensor number - {deviceModel.Sensors[i].SensorNumber}");
            Console.WriteLine(ModbusSettings.PrintDecor);
            Console.WriteLine($"Sensor Status: {(SensorStatusEnum)deviceModel.Sensors[i].SensorStatus}");
            Console.WriteLine($"Current temperature: {deviceModel.Sensors[i].CurrentTemperature}");
            Console.WriteLine($"Lower limit: {deviceModel.Sensors[i].LowerLimit}");
            Console.WriteLine($"Higher limit: {deviceModel.Sensors[i].HigherLimit}");
        }
    }
}
