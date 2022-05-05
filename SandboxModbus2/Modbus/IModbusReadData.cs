using NModbus;
using SandboxModbus2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public interface IModbusReadData
    {
        Task<string> DeviceNameRead(IModbusMaster master, byte slaveNumber);
        Task<DeviceModel> ReadData(ITcpClientFactory tcpClientFactory, byte slaveNumber);
        Task<List<SensorModel>> SensorsRead(IModbusMaster master, int sensorsNumber, byte slaveNumber);
        Task<ushort> SystemStatusRead(IModbusMaster master, byte slaveNumber);
    }
}