using Database.Context;
using Database.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Tests
{
    public class TestData
    {
        private static DatabaseContext? _databaseContextInMemory;

        public static DatabaseContext DatabaseContextInMemory
        {
            get
            {
                if (_databaseContextInMemory != null)
                    return _databaseContextInMemory;

                var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase("IoTServiceTestDb", builder => builder.EnableNullChecks(false))
                    .Options;

                var databaseContextInMemory = new DatabaseContext(dbContextOptions);

                databaseContextInMemory.Devices.Add(new Device { UserAccount = null, DeviceNr = 123, Name = "Factory device", Description = "Suitable for factories" });
                databaseContextInMemory.Devices.Add(new Device { UserAccount = null, DeviceNr = 321, Name = "Home device", Description = "Suitable for homes" });

                databaseContextInMemory.SaveChanges();

                var deviceId = databaseContextInMemory.Devices.Where(x => x.DeviceNr == 123).Select(x => x.Id).FirstOrDefault();

                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1000, Name = "Inside temperature", MinRange = -150, MaxRange = 150, Value = 22 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1010, Name = "Has outside temperature sensor", MinRange = 0, MaxRange = 1, Value = 1 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1011, Name = "Outside temperature", MinRange = -100, MaxRange = 100, Value = 32 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1020, Name = "Water temperature", MinRange = -100, MaxRange = 100, Value = 50 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 2000, Name = "OperationTimeInSec", MinRange = 0, MaxRange = int.MaxValue, Value = 0 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 2010, Name = "WorkingHour", MinRange = 0, MaxRange = int.MaxValue, Value = 0 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 4040, Name = "IsOperational", MinRange = 0, MaxRange = 1, Value = 1 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 5050, Name = "SilentMode", MinRange = 0, MaxRange = 1, Value = 1 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 8000, Name = "Machine is broken", MinRange = 0, MaxRange = 1, Value = 0 });
                databaseContextInMemory.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 9000, Name = "SerialNumber", MinRange = 0, MaxRange = 999999, Value = 12345 });

                databaseContextInMemory.SaveChanges();

                _databaseContextInMemory = databaseContextInMemory;

                return _databaseContextInMemory;
            }
        }
    }
}