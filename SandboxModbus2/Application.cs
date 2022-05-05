using SandboxModbus2.Modbus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SandboxModbus2
{
    public class Application : IApplication
    {
        IConsolePrintData _consolePrintData;
 
        public Application(IConsolePrintData consolePrintData)
        {
            _consolePrintData = consolePrintData;
        }

        public void Run()
        {
            Task.Run(() => _consolePrintData.PrintDataAsync());
        }
    }
}
