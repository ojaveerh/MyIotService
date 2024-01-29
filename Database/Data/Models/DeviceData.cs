using System.ComponentModel.DataAnnotations;

namespace Database.Data.Models
{
    public class DeviceData
    {
        [Key]
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public Device Device { get; set; }

        [Required]
        public int DataId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int MinRange { get; set; }
        [Required]
        public int MaxRange { get; set; }

        [Required]
        public int Value { get; set; }

    }
}
