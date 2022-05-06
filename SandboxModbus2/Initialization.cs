using SandboxModbus2.Modbus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SandboxModbus2
{
    public class Initialization : IInitialization
    {
        IConsolePrintData _consolePrintData;
 
        public Initialization(IConsolePrintData consolePrintData)
        {
            _consolePrintData = consolePrintData;
        }

        public void Run()
        {
            Task.Run(() => _consolePrintData.PrintDataAsync());
        }
    }
}
