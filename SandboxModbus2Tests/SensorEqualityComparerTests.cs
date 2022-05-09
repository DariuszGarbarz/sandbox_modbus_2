using NUnit.Framework;
using SandboxModbus2.Comparers;
using SandboxModbus2.Models;

namespace SandboxModbus2Tests
{
    public class SensorEqualityComparerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        SensorEqualityComparer sensorEqualityComparer = new SensorEqualityComparer();

        [Test]
        public void SensorEqualsTest()
        {
            //arrange
            var firstSensorData = new SensorModel
            {
                SensorNumber = 1,
                SensorStatus = 1,
                CurrentTemperature = 10,
                LowerLimit = 5,
                HigherLimit = 20
            };

            var sameSensorData = new SensorModel
            {
                SensorNumber = 1,
                SensorStatus = 1,
                CurrentTemperature = 10,
                LowerLimit = 5,
                HigherLimit = 20
            };

            var differentSensorData = new SensorModel
            {
                SensorNumber = 1,
                SensorStatus = 4,
                CurrentTemperature = 20,
                LowerLimit = 3,
                HigherLimit = 33
            };

            var expectedTrue = true;
            var expectedFalse = false;

            //act
            var actual1 = sensorEqualityComparer.Equals(firstSensorData, sameSensorData);
            var actual2 = sensorEqualityComparer.Equals(firstSensorData, differentSensorData);

            //assert
            Assert.That(actual1, Is.EqualTo(expectedTrue));
            Assert.That(actual2, Is.EqualTo(expectedFalse));
        }
    }
}
