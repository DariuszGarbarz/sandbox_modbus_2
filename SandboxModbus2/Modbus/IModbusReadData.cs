using NModbus;
using SandboxModbus2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public interface IModbusReadData
    {
        Task<string> DeviceNameRead(IModbusMaster master);
        Task<DeviceModel> ReadData();
        Task<List<SensorModel>> SensorsRead(IModbusMaster master, int sensorsNumber);
        Task<ushort> SystemStatusRead(IModbusMaster master);
    }
}