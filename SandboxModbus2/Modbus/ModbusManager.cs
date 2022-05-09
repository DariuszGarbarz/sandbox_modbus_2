using NModbus;
using SandboxModbus2.Enums;
using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
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
        private IModbusMaster _modbusMaster;
        private IEqualityComparer<DeviceModel> _deviceComparer;
        private IEqualityComparer<SensorModel> _sensorComparer;

        public ModbusManager(IModbusDataReader modbusReadData, ITcpClientFactory tcpClientFactory,
            IEqualityComparer<DeviceModel> deviceComparer, IEqualityComparer<SensorModel> sensorComparer)
        {
            _modbusReadData = modbusReadData;
            _modbusMaster = tcpClientFactory.Master;
            _deviceComparer = deviceComparer;
            _sensorComparer = sensorComparer;
        }

        public async Task PrintDataAsync(CancellationToken cancellationToken)
        {
            try
            {
                List<DeviceModel> deviceList = new List<DeviceModel>();
                for (byte slaveNumber = 1; slaveNumber <= ModbusSettings.SlavesCount; slaveNumber++)
                {
                    var deviceModel = await _modbusReadData
                        .ReadData(_modbusMaster, slaveNumber, ModbusSettings.SensorsCount,
                        ModbusSettings.SystemStatusStartAdress, ModbusSettings.SystemStatusNumberOfPoints,
                        ModbusSettings.DeviceNameStartAdress, ModbusSettings.DeviceNameNumberOfPoints,
                        ModbusSettings.SensorStartAdress, ModbusSettings.SensorNumberOfPoints);

                    PrintDeviceData(deviceModel);

                    for (var sensorNumber = 0; sensorNumber < deviceModel.Sensors.Count; sensorNumber++)
                        PrintSensorsData(deviceModel.Sensors[sensorNumber]); 

                        deviceList.Add(deviceModel);
                }

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    for (byte slaveNumber = 1; slaveNumber <= ModbusSettings.SlavesCount; slaveNumber++)
                    {
                        var deviceModel = await _modbusReadData
                        .ReadData(_modbusMaster, slaveNumber, ModbusSettings.SensorsCount,
                        ModbusSettings.SystemStatusStartAdress, ModbusSettings.SystemStatusNumberOfPoints,
                        ModbusSettings.DeviceNameStartAdress, ModbusSettings.DeviceNameNumberOfPoints,
                        ModbusSettings.SensorStartAdress, ModbusSettings.SensorNumberOfPoints);

                        if (!_deviceComparer.Equals(deviceModel, deviceList[slaveNumber-1]))
                        {
                            PrintDeviceData(deviceModel);

                            for (int sensorNumber = 0; sensorNumber < deviceModel.Sensors.Count; sensorNumber++)
                            {
                                if (!_sensorComparer.Equals(deviceModel.Sensors[sensorNumber],
                                    deviceList[slaveNumber - 1].Sensors[sensorNumber]))
                                    PrintSensorsData(deviceModel.Sensors[sensorNumber]);
                            }
                            deviceList[slaveNumber - 1] = deviceModel;
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
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(ModbusSettings.PrintDecor);
            Console.WriteLine($"Slave number: {deviceModel.SlaveNumber}");
            Console.WriteLine($"System Status: {(SystemStatusEnum)deviceModel.SystemStatus}");
            Console.WriteLine($"Device Name:{deviceModel.DeviceName}");
        }

        public void PrintSensorsData(SensorModel sensor)
        {
            Console.WriteLine(ModbusSettings.PrintDecor);
            Console.WriteLine($"Sensor number - {sensor.SensorNumber}");
            Console.WriteLine(ModbusSettings.PrintDecor);
            Console.WriteLine($"Sensor Status: {(SensorStatusEnum)sensor.SensorStatus}");
            Console.WriteLine($"Current temperature: {sensor.CurrentTemperature}");
            Console.WriteLine($"Lower limit: {sensor.LowerLimit}");
            Console.WriteLine($"Higher limit: {sensor.HigherLimit}");
        }
    }
}
