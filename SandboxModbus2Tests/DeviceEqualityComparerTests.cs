using NUnit.Framework;
using SandboxModbus2.Comparers;
using SandboxModbus2.Models;
using System.Collections.Generic;

namespace SandboxModbus2Tests
{
    public class DeviceEqualityComparerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        DeviceEqualityComparer deviceEqualityComparer = new DeviceEqualityComparer();

        [Test]
        public void DeviceEqualsTest()
        {
            //arrange
            List<SensorModel> Sensors = new List<SensorModel>
            {
                new SensorModel
                {
                    SensorNumber = 1,
                    SensorStatus = 1,
                    CurrentTemperature = 10,
                    LowerLimit = 5,
                    HigherLimit = 20
                },

                new SensorModel
                {
                    SensorNumber = 2,
                    SensorStatus = 2,
                    CurrentTemperature = 0,
                    LowerLimit = 0,
                    HigherLimit = 0
                }
            };

            var firstDeviceData = new DeviceModel
            {
                SystemStatus = 1,
                DeviceName = "ELACOMPIL\0",
                Sensors = Sensors
            };

            var sameDeviceData = new DeviceModel
            {
                SystemStatus = 1,
                DeviceName = "ELACOMPIL\0",
                Sensors = Sensors
            };

            var differentDeviceData = new DeviceModel
            {
                SystemStatus = 1,
                DeviceName = "ELA",
                Sensors = Sensors
            };

            var expectedTrue = true;
            var expectedFalse = false;

            //act
            var actual1 = deviceEqualityComparer.Equals(firstDeviceData, sameDeviceData);
            var actual2 = deviceEqualityComparer.Equals(firstDeviceData, differentDeviceData);

            //assert
            Assert.That(actual1, Is.EqualTo(expectedTrue));
            Assert.That(actual2, Is.EqualTo(expectedFalse));
        }
    }
}
