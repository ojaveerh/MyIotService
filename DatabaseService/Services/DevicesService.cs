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

        /// <summary>
        /// Registers device to user
        /// </summary>
        /// <param name="deviceNr">Device number</param>
        /// <param name="name">Device name</param>
        /// <param name="description">Device description</param>
        /// <param name="userId">Device owner</param>
        /// <param name="dataId">Device data id</param>
        /// <param name="dataName">Device data name</param>
        /// <param name="minRange">Device data range min</param>
        /// <param name="maxRange">Device data range max</param>
        /// <param name="value">Device data value</param>
        /// <returns></returns>
        public Task<bool> RegisterDeviceAsync(int deviceNr, string name, string? description,  string userName, int dataId, string dataName, int minRange, int maxRange, int value );
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

        public async Task<bool> RegisterDeviceAsync(int deviceNr, string name, string? description, string userName, int dataId, string dataName, int minRange, int maxRange, int value)
        {
            var isDeviceRegistred = await _context.Devices.AnyAsync(device => device.DeviceNr == deviceNr && device.Name == name);

            if (isDeviceRegistred)
                return false;

            var user = await _context.UserAccounts.FirstOrDefaultAsync(user => user.UserName == userName);

            if (user == null) { return false; }

            var newDevice = new Device
            {
                UserAccountId = user.Id,
                DeviceNr = deviceNr,
                Name = name,
                Description = description,              
            };

            await _context.Devices.AddAsync(newDevice);
            await _context.SaveChangesAsync();

            var createdDevice = await _context.Devices.FirstOrDefaultAsync(device => device.DeviceNr == deviceNr && device.Name == name);

            if (createdDevice != null)
            {
                var newDeviceData = new DeviceData
                {
                    DeviceId = createdDevice.Id,
                    DataId = dataId,
                    Name = dataName,
                    MinRange = minRange,
                    MaxRange = maxRange,
                    Value = value
                };
                await _context.DeviceDatas.AddAsync(newDeviceData);
                await _context.SaveChangesAsync();
            }

            return true;
        }
 
    }
}