using System.ComponentModel.DataAnnotations;

namespace Database.Data.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public int? UserAccountId { get; set; }
        public UserAccount? UserAccount {  get; set; }
        public int DeviceNr { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<DeviceData>? DeviceData { get; set; }

    }
}