using NModbus;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SandboxModbus2.Modbus
{
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
                Client.Connect(ModbusSettings.hostname, ModbusSettings.port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
