using SandboxModbus2.Modbus;
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
        IModbusManager _modbusManager;
 
        public Application(IModbusManager modbusManager)
        {
            _modbusManager= modbusManager;
        }

        public void Run(CancellationToken cancellationToken)
        {
            Task.Run(() => _modbusManager
            .PrintDataAsync(cancellationToken), cancellationToken);
        }
    }
}
