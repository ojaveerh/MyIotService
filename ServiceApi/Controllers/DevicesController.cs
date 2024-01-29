using DatabaseService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    [Route("[controller]")]
    public class DevicesController : Controller
    {
        private readonly IDevicesService _devicesService;
        public DevicesController(IDevicesService devicesService)
        {
            _devicesService = devicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            return Ok(await _devicesService.GetDevicesAsync());
        }

        [HttpGet]
        [Route("{deviceId}/GetDeviceData")]
        public async Task<IActionResult> GetDeviceData(int deviceId)
        {
            return Ok(await _devicesService.GetDeviceDataAsync(deviceId));
        }

        [HttpGet]
        [Route("{deviceNr}/GetDeviceInsideTemperature")]
        public async Task<IActionResult> GetDeviceInsideTemperature([FromRoute] int deviceNr)
        {
            return Ok(await _devicesService.GetDeviceInsideTemperatureAsync(deviceNr));
        }

        [HttpPost]
        [Route("{deviceNr}/SetDeviceInsideTemperature")]
        public async Task<IActionResult> SetDeviceInsideTemperature([FromRoute] int deviceNr, int temperature)
        {
            var result = await _devicesService.SetDeviceInsideTemperatureAsync(deviceNr, temperature);

            if (result == false)
            {
                return BadRequest($"Setting temperature for device {deviceNr} was unsuccessful!");
            }

            return Ok("Temperature set!");
        }
    }
}