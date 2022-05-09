using Moq;
using NModbus;
using NUnit.Framework;
using SandboxModbus2.Modbus;
using SandboxModbus2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandboxModbus2Tests
{
    public class ModbusDataReaderTests
    {
        private readonly Mock<IModbusMaster> _masterMock = new Mock<IModbusMaster>();
        [SetUp]
        public void Setup()
        {
        }

        readonly ModbusDataReader _modbusReadData = new ModbusDataReader();

        [Test]
        public async Task SystemStatusReadTest()
        {
            //arrange
            ushort systemStatusStartAdress = 0;
            ushort systemStatusNumberOfPoints = 1;
            byte slaveNumber = 1;
            ushort expectedValue = 1;
            var valueReturnedFromRegister = new ushort[1]
            {
                1
            };

            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, systemStatusStartAdress,
            systemStatusNumberOfPoints))
                .ReturnsAsync(valueReturnedFromRegister);

            //act
            var actualValue = await _modbusReadData
                .SystemStatusRead(_masterMock.Object, slaveNumber, systemStatusStartAdress, systemStatusNumberOfPoints);

            //assert
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Test]
        public async Task DeviceNameReadTest()
        {
            //arrange
            ushort deviceNameStartAdress = 1;
            ushort deviceNameNumberOfPoints = 5;
            byte slaveNumber = 1;
            var valueReturnedFromRegister = new ushort[5]
            {
                17740, 16707, 20301, 20553, 19456
            };

            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, deviceNameStartAdress,
            deviceNameNumberOfPoints))
                .ReturnsAsync(valueReturnedFromRegister);

            var expectedString = "ELACOMPIL\0";           

            //act
            var actualString = await _modbusReadData
                .DeviceNameRead(_masterMock.Object, slaveNumber, deviceNameStartAdress, deviceNameNumberOfPoints);

            //assert
            Assert.That(actualString, Is.EqualTo(expectedString));
        }

        [Test]
        public async Task SensorsReadTest()
        {
            //arrange
            ushort sensorNumberStartAdress = 100;
            ushort sensorNumberNumberOfPoints = 4;
            byte slaveNumber = 1;
            var sensorsCount = 2;
            List<SensorModel> expectedSensors = new List<SensorModel>
            {
                new SensorModel
                {
                    SensorNumber = 1,
                    SensorStatus = 1,
                    CurrentTemperature = 1,
                    LowerLimit = 0,
                    HigherLimit = 2
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

            var firstValueReturnedFromRegister = new ushort[4]
            {
                1, 10, 5, 20
            };

            var secondValueReturnedFromRegister = new ushort[4]
            {
                2, 0, 0, 0
            };

            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, 100, sensorNumberNumberOfPoints))
                .ReturnsAsync(firstValueReturnedFromRegister);
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, 200, sensorNumberNumberOfPoints))
                .ReturnsAsync(secondValueReturnedFromRegister);

            //act
            var actualSensors = await _modbusReadData
                .SensorsRead(_masterMock.Object, sensorsCount, slaveNumber, sensorNumberStartAdress, sensorNumberNumberOfPoints);

            //assert
            for (int sensorNumber = 0; sensorNumber < sensorsCount; sensorNumber++)
            {
                Assert.That(actualSensors[sensorNumber].SensorNumber,
                    Is.EqualTo(expectedSensors[sensorNumber].SensorNumber));
                Assert.That(actualSensors[sensorNumber].SensorStatus,
                    Is.EqualTo(expectedSensors[sensorNumber].SensorStatus));
                Assert.That(actualSensors[sensorNumber].CurrentTemperature,
                    Is.EqualTo(expectedSensors[sensorNumber].CurrentTemperature));
                Assert.That(actualSensors[sensorNumber].LowerLimit,
                    Is.EqualTo(expectedSensors[sensorNumber].LowerLimit));
                Assert.That(actualSensors[sensorNumber].HigherLimit,
                    Is.EqualTo(expectedSensors[sensorNumber].HigherLimit));
            }            
        }

        [Test]
        public async Task ReadDataTest()
        {
            //arrange
            ushort systemStatusStartAdress = 0;
            ushort systemStatusNumberOfPoints = 1;
            ushort deviceNameStartAdress = 1;
            ushort deviceNameNumberOfPoints = 5;
            ushort sensorNumberStartAdress = 100;
            ushort sensorNumberOfPoints = 4;
            byte slaveNumber = 1;
            var sensorsCount = 2;
            List<SensorModel> expectedSensors = new List<SensorModel>
            {
                new SensorModel
                {
                    SensorNumber = 1,
                    SensorStatus = 1,
                    CurrentTemperature = 1,
                    LowerLimit = 0,
                    HigherLimit = 2
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
            var expectedDevice = new DeviceModel
            {
                SystemStatus = 1,
                DeviceName = "ELACOMPIL\0",
                Sensors = expectedSensors
            };

            var deviceNameReturnedFromRegister = new ushort[5]
            {
                17740, 16707, 20301, 20553, 19456
            };

            var deviceStatusReturnedFromRegister = new ushort[1]
            {
                1
            };

            var firstSensorReturnedFromRegister = new ushort[4]
            {
                1, 10, 5, 20
            };

            var secondSensorReturnedFromRegister = new ushort[4]
            {
                2, 0, 0, 0
            };

            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, deviceNameStartAdress,
            deviceNameNumberOfPoints))
                .ReturnsAsync(deviceNameReturnedFromRegister);

            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, systemStatusStartAdress,
            systemStatusNumberOfPoints))
                .ReturnsAsync(deviceStatusReturnedFromRegister);

            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, 100, sensorNumberOfPoints))
                .ReturnsAsync(firstSensorReturnedFromRegister);
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync
            (slaveNumber, 200, sensorNumberOfPoints))
                .ReturnsAsync(secondSensorReturnedFromRegister);

            //act
            var actualDevice = await _modbusReadData
                .ReadData(_masterMock.Object, slaveNumber, sensorsCount,
                systemStatusStartAdress, systemStatusNumberOfPoints,
                deviceNameStartAdress, deviceNameNumberOfPoints, 
                sensorNumberStartAdress, sensorNumberOfPoints);

            //assert
            Assert.That(actualDevice.SystemStatus, Is.EqualTo(expectedDevice.SystemStatus));
            Assert.That(actualDevice.DeviceName, Is.EqualTo(expectedDevice.DeviceName));
            for (int sensorNumber = 0; sensorNumber < sensorsCount; sensorNumber++)
            {
                Assert.That(actualDevice.Sensors[sensorNumber].SensorNumber,
                    Is.EqualTo(expectedDevice.Sensors[sensorNumber].SensorNumber));
                Assert.That(actualDevice.Sensors[sensorNumber].SensorStatus,
                    Is.EqualTo(expectedDevice.Sensors[sensorNumber].SensorStatus));
                Assert.That(actualDevice.Sensors[sensorNumber].CurrentTemperature,
                    Is.EqualTo(expectedDevice.Sensors[sensorNumber].CurrentTemperature));
                Assert.That(actualDevice.Sensors[sensorNumber].LowerLimit,
                    Is.EqualTo(expectedDevice.Sensors[sensorNumber].LowerLimit));
                Assert.That(actualDevice.Sensors[sensorNumber].HigherLimit,
                    Is.EqualTo(expectedDevice.Sensors[sensorNumber].HigherLimit));
            }
        }
    }
}