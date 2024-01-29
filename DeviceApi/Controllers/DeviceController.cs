using DatabaseService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    [Route("[controller]")]
    public class DeviceController : Controller
    {
        private readonly IDevicesService _devicesService;
        public DeviceController(IDevicesService devicesService)
        {
            _devicesService = devicesService;
        }

        [HttpGet]
        [Route("{deviceNr}/GetDeviceInsideTemperature")]
        public async Task<IActionResult> GetDeviceInsideTemperature([FromRoute]int deviceNr)
        {
            return Ok(await _devicesService.GetDeviceInsideTemperatureAsync(deviceNr));
        }

        [HttpPost]
        [Route("{deviceNr}/SetDeviceInsideTemperature")]
        public async Task<IActionResult> SetDeviceInsideTemperature([FromRoute] int deviceNr, int temperature)
        {
            var result = await _devicesService.SetDeviceInsideTemperatureAsync(deviceNr, temperature);

            if(result == false)
            {
                return BadRequest($"Setting temperature for device {deviceNr} was unsuccessful!");
            }

            return Ok("Temperature set!");
        }

        [HttpPost]
        [Route("/RegisterDevice")]
        public async Task<IActionResult> RegisterDeviceAsync(int deviceNr, string name, string? description, string username, int dataId, string dataName, int minRange, int maxRange, int value)
        {
            var result = await _devicesService.RegisterDeviceAsync(deviceNr, name, description, username, dataId, dataName, minRange, maxRange, value);

            if (result == false)
            {
                return BadRequest($"Something went wrong!");
            }

            return Ok("Device is registred");
        }

        //TODO - võtab vastu
        // new Device { UserAccount = null, DeviceNr = 1234, Name = "Factory device", Description = "Suitable for factories" });
        //new DeviceData { DeviceId = deviceId, DataId = 1000, Name = "Inside temperature", MinRange = -150, MaxRange = 150, Value = 22 });

    }
}