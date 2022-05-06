using SandboxModbus2.Modbus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxModbus2
{
    public interface IApplication
    {
        void Run(CancellationToken tokenSource);
    }

    public class Application : IApplication
    {
        IModbusManager _consolePrintData;
 
        public Application(IModbusManager consolePrintData)
        {
            _consolePrintData = consolePrintData;
        }

        public void Run(CancellationToken cancellationToken)
        {
            Task.Run(() => _consolePrintData.PrintDataAsync(cancellationToken), cancellationToken);
        }
    }
}
