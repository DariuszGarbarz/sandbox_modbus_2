using Moq;
using NModbus;
using NUnit.Framework;
using SandboxModbus2.Modbus;
using SandboxModbus2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SandboxModbus2Tests
{
    public class ModbusReadDataTests
    {
        private readonly Mock<IModbusMaster> _masterMock = new Mock<IModbusMaster>();
        [SetUp]
        public void Setup()
        {
        }

        readonly ModbusReadData _modbusReadData = new ModbusReadData();

        [Test]
        public async Task SystemStatusReadTest()
        {
            //arrange
            byte slaveNumber = 1;
            ushort expectedValue = 1;
            var expected = new ushort[1]
            {
                1
            };
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(slaveNumber, ModbusSettings.systemStatusStartAdress, ModbusSettings.systemStatusNumberOfPoints))
                .ReturnsAsync(expected);

            //act
            var actualValue = await _modbusReadData.SystemStatusRead(_masterMock.Object, slaveNumber);

            //assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public async Task DeviceNameReadTest()
        {
            //arrange
            byte slaveNumber = 1;
            var expected = new ushort[31]
            {
                69, 76, 65, 67, 79, 77, 80, 73, 76, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(slaveNumber, ModbusSettings.deviceNameStartAdress, ModbusSettings.deviceNameNumberOfPoints))
                .ReturnsAsync(expected);
            var expectedString = "ELACOMPIL\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";           

            //act
            var actualString = await _modbusReadData.DeviceNameRead(_masterMock.Object, slaveNumber);

            //assert
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public async Task SensorsReadTest()
        {
            //arrange
            byte slaveNumber = 1;
            var sensorsNumber = 2;
            List<SensorModel> expectedSensors = new List<SensorModel>();
            expectedSensors.Add(new SensorModel
            {
                SensorNumber = 1,
                SensorStatus = 1,
                CurrentTemperature = 10,
                LowerLimit = 5,
                HigherLimit = 20
            });

            expectedSensors.Add(new SensorModel
            {
                SensorNumber = 2,
                SensorStatus = 2,
                CurrentTemperature = 0,
                LowerLimit = 0,
                HigherLimit = 0
            });

            var expected1 = new ushort[4]
            {
                1, 10, 5, 20
            };
            var expected2 = new ushort[4]
            {
                2, 0, 0, 0
            };

            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(slaveNumber, 100, ModbusSettings.sensorNumberOfPoints))
                .ReturnsAsync(expected1);
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(slaveNumber, 200, ModbusSettings.sensorNumberOfPoints))
                .ReturnsAsync(expected2);            

            //act
            var actualSensors = await _modbusReadData.SensorsRead(_masterMock.Object, sensorsNumber, slaveNumber);

            //assert
            for (int i = 0; i < sensorsNumber; i++)
            {
                Assert.AreEqual(expectedSensors[i].SensorNumber, actualSensors[i].SensorNumber);
                Assert.AreEqual(expectedSensors[i].SensorStatus, actualSensors[i].SensorStatus);
                Assert.AreEqual(expectedSensors[i].CurrentTemperature, actualSensors[i].CurrentTemperature);
                Assert.AreEqual(expectedSensors[i].LowerLimit, actualSensors[i].LowerLimit);
                Assert.AreEqual(expectedSensors[i].HigherLimit, actualSensors[i].HigherLimit);
            }            
        }
    }
}