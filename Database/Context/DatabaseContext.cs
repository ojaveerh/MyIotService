using Database.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceData> DeviceDatas { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

    }
}