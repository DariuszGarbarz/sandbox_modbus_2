using Autofac;
using SandboxModbus2.Comparers;
using SandboxModbus2.Modbus;
using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxModbus2
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<ModbusDataReader>().As<IModbusDataReader>();
            builder.RegisterType<ModbusManager>().As<IModbusManager>();
            builder.RegisterType<TcpClientFactory>().As<ITcpClientFactory>().InstancePerLifetimeScope();
            builder.RegisterType<DeviceEqualityComparer>().As<IEqualityComparer<DeviceModel>>().InstancePerLifetimeScope();
            builder.RegisterType<SensorEqualityComparer>().As<IEqualityComparer<SensorModel>>().InstancePerLifetimeScope();

            return builder.Build();
        }

    }
}
