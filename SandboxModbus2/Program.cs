using Autofac;
using System;
using System.Threading;

namespace SandboxModbus2
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var container = ContainerConfig.Configure();
            using var tokenSource = new CancellationTokenSource();

            var app = container.Resolve<IApplication>();
            app.Run(tokenSource.Token);

            Console.WriteLine("Modbus Test App");
            Console.ReadLine();
            tokenSource.Cancel();
        }
    }
}
