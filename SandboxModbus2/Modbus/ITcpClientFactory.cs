using NModbus;
using System.Net.Sockets;

namespace SandboxModbus2.Modbus
{
    public interface ITcpClientFactory
    {
        IModbusMaster Master { get; set; }
        TcpClient Client { get; set; }
    }
}