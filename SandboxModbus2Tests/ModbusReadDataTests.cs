using Moq;
using NModbus;
using NUnit.Framework;
using SandboxModbus2.Modbus;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SandboxModbus2Tests
{
    public class Tests
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
            var expected = new ushort[1]
            {
                1
            };
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(1, 0, 1))
                .ReturnsAsync(expected);
            var expectedMessage = "System status - normal\r\n";
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //act
            await _modbusReadData.SystemStatusRead(_masterMock.Object);

            //assert
            var actualMessage = stringWriter.ToString();
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public async Task DeviceNameReadTest()
        {
            //arrange
            var expected = new ushort[31]
            {
                69, 76, 65, 67, 79, 77, 80, 73, 76, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(1, 1, 32))
                .ReturnsAsync(expected);
            var expectedMessage = "Device Name: ELACOMPIL\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\r\n";
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //act
            await _modbusReadData.DeviceNameRead(_masterMock.Object);

            //assert
            var actualMessage = stringWriter.ToString();
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public async Task SensorsReadTest()
        {
            //arrange
            var sensorsNumber = 2;
            var expected1 = new ushort[4]
            {
                1, 10, 5, 20
            };
            var expected2 = new ushort[4]
            {
                2, 0, 0, 0
            };
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(1, 100, 4))
                .ReturnsAsync(expected1);
            _masterMock.Setup(x => x.ReadHoldingRegistersAsync(1, 200, 4))
                .ReturnsAsync(expected2);
            var expectedMessage = "--------------------------\r\nSensor number - 1\r\n--------------------------\r\nSensor status - Online\r\nCurrent temperature: 10\r\nLower limit: 5\r\nHigher limit: 20\r\n" +
                                  "--------------------------\r\nSensor number - 2\r\n--------------------------\r\nSensor status - Alarm\r\nCurrent temperature: 0\r\nLower limit: 0\r\nHigher limit: 0\r\n";
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //act
            await _modbusReadData.SensorsRead(_masterMock.Object, sensorsNumber);

            //assert
            var actualMessage = stringWriter.ToString();
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}