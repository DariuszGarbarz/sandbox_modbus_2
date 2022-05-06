﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxModbus2.Enums
{
    public enum SystemStatusEnum : ushort
    {
        ConnectionProblem,
        Normal = 1,
        Fault
    }
}
