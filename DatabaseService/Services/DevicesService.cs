using Database.Context;
using Database.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public interface IDevicesService
    {
        public Task<List<Device>> GetDevicesAsync();

        public Task<List<DeviceData>> GetDeviceDataAsync(int deviceId);

        /// <summary>
        /// Sets the desired inside temperature
        /// </summary>
        /// <param name="deviceNr"></param>
        /// <param name="temperature">your desired temperature in Celsius</param>
        /// <returns>true if temperature was successfully set, otherwise false</returns>
        public Task<bool> SetDeviceInsideTemperatureAsync(int deviceNr, int temperature);

        /// <summary>
        /// Gets the current inside temperature from device
        /// </summary>
        /// <param name="deviceNr"></param>
        /// <returns>Returns temperature in Celsius. Null, if device or device data was not found.</returns>
        public Task<int?> GetDeviceInsideTemperatureAsync(int deviceNr);
    }

    public class DevicesService : IDevicesService
    {
        private readonly DatabaseContext _context;
        public DevicesService(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task<List<DeviceData>> GetDeviceDataAsync(int deviceId)
        {
            return await _context.DeviceDatas.Where(deviceData => deviceData.DeviceId == deviceId).ToListAsync();
        }

        public async Task<int?> GetDeviceInsideTemperatureAsync(int deviceNr)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(device => device.DeviceNr == deviceNr);

            if (device == null)
            {
                return null;
            }

            return await _context.DeviceDatas
                .Where(deviceData => deviceData.DeviceId == device.Id && deviceData.Name == "Inside temperature")
                .Select(deviceData => deviceData.Value)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SetDeviceInsideTemperatureAsync(int deviceNr, int temperature)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(device => device.DeviceNr == deviceNr);

            if (device == null)
            {
                return false;
            }

            var deviceInsideTemperatureData = await _context.DeviceDatas.FirstOrDefaultAsync(deviceData => deviceData.DeviceId == device.Id && deviceData.Name == "Inside temperature");

            if (deviceInsideTemperatureData == null)
            {
                deviceInsideTemperatureData = new DeviceData
                {
                    DeviceId = device.Id,
                    DataId = 1000,
                    Name = "Inside temperature",
                    MinRange = -150,
                    MaxRange = 150
                };

                await _context.DeviceDatas.AddAsync(deviceInsideTemperatureData);
            }

            deviceInsideTemperatureData.Value = temperature;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}