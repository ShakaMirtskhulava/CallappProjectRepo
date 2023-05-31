using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Models
{
    public class APIUser
    {
        [Key]
        public int APIUserId { get; set; }
        [Required]
        [MaxLength(100)]
        public required string UserName { get; set; }
        [Required]
        [MaxLength(200)]
        public required string PasswordHash { get; set; }
        [Required]
        public required string JWT { get; set; }
    }
}
