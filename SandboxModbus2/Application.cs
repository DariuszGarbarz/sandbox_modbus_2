using SandboxModbus2.Modbus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SandboxModbus2
{
    public class Application : IApplication
    {
        IModbusReadData _modbusReadData;
        public Application(IModbusReadData modbusReadData)
        {
            _modbusReadData = modbusReadData;
        }

        public void Run()
        {
            Task.Run(() => _modbusReadData.ReadData());
        }
    }
}
