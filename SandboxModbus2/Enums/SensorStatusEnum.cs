namespace SandboxModbus2.Enums
{
    public enum SensorStatusEnum: ushort
    {
        Unknown,
        Online = 1,
        Alarm = 2,
        Fault = 4,
        Disabled = 8
    }
}
