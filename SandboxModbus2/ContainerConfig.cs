﻿using Autofac;
using SandboxModbus2.Modbus;
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
            builder.RegisterType<ModbusReadData>().As<IModbusReadData>();

            return builder.Build();
        }

    }
}