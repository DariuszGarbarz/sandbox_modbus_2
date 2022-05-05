using NModbus;
using System.Threading.Tasks;

namespace SandboxModbus2.Modbus
{
    public interface IModbusReadData
    {
        Task DeviceNameRead(IModbusMaster master);
        Task ReadData();
        Task SensorsRead(IModbusMaster master, int sensorsNumber);
        Task SystemStatusRead(IModbusMaster master);
    }
}