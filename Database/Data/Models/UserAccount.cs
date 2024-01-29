using System.ComponentModel.DataAnnotations;

namespace Database.Data.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public string Role { get; set; }

        
        public ICollection<Device>? Devices { get; set; }
    }
}