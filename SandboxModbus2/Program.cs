using Autofac;
using System;

namespace SandboxModbus2
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IInitialization>();
                app.Run();
            }

            Console.WriteLine("Modbus Test App");
            Console.ReadLine();
        }
    }
}
