using DatabaseService.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseService.Tests.Services
{
    [TestClass]
    public class DevicesServiceTests
    {
        [TestMethod]
        public async Task GetDeviceInsideTemperatureAsync_ReturnsNull()
        {
            //Arrange:
            var devicesService = new DevicesService(TestData.DatabaseContextInMemory);

            //Act:
            var result = await devicesService.GetDeviceInsideTemperatureAsync(999);

            //Assert:
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetDeviceInsideTemperatureAsync_ReturnsCorrectValue()
        {
            //Arrange:
            var devicesService = new DevicesService(TestData.DatabaseContextInMemory);

            //Act:
            var result = await devicesService.GetDeviceInsideTemperatureAsync(123);

            //Assert:
            Assert.AreEqual(22, result);
        }

        [TestMethod]
        public async Task SetDeviceInsideTemperatureAsync_ReturnsNull()
        {
            //Arrange:
            var devicesService = new DevicesService(TestData.DatabaseContextInMemory);

            //Act:
            var result = await devicesService.SetDeviceInsideTemperatureAsync(999, 1);

            //Assert:
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task SetDeviceInsideTemperatureAsync_ReturnsTrue()
        {
            //Arrange:
            var devicesService = new DevicesService(TestData.DatabaseContextInMemory);

            //Act:
            var result = await devicesService.SetDeviceInsideTemperatureAsync(123, 1);

            //Assert:
            Assert.IsTrue(result);
        }
    }
}