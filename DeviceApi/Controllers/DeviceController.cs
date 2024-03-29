﻿using DatabaseService.Services;
using DeviceApi.Models;
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
        public async Task<IActionResult> RegisterDeviceAsync([FromBody] RegisterDeviceRequest registerDeviceRequest)
        {
            var deviceExists = await _devicesService.DeviceExists(registerDeviceRequest.DeviceNr);
            if(deviceExists == true) 
                return BadRequest($"Device already exists");
            
            var result = await _devicesService.RegisterDeviceAsync(
                registerDeviceRequest.DeviceNr,
                registerDeviceRequest.Name,
                registerDeviceRequest.Description,
                registerDeviceRequest.Username,
                registerDeviceRequest.DataId,
                registerDeviceRequest.DataName,
                registerDeviceRequest.MinRange,
                registerDeviceRequest.MaxRange,
                registerDeviceRequest.Value);

            if (result == false)
            {
                return BadRequest($"Something went wrong!");
            }

            return Ok("Device is registred");
        }



    }

}