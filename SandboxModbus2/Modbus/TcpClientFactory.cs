using NModbus;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SandboxModbus2.Modbus
{
    public interface ITcpClientFactory
    {
        IModbusMaster Master { get; set; }
        TcpClient Client { get; set; }
    }

    public class TcpClientFactory : ITcpClientFactory
    {
        public IModbusMaster Master { get; set; }
        public TcpClient Client { get; set; }

        public TcpClientFactory()
        {
            try
            {
                var factory = new ModbusFactory();
                Client = new TcpClient();
                Master = factory.CreateMaster(Client);
                Client.Connect(ModbusSettings.Hostname, ModbusSettings.Port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
