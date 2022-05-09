﻿namespace SandboxModbus2.Enums
{
    public enum SensorStatusEnum: ushort
    {
        ConnectionProblem,
        Online = 1,
        Alarm = 2,
        Fault = 4,
        Disabled = 8
    }
}
