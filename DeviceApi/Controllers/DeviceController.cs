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
    }
}