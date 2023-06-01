using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Models
{
    public class User
    {
        [Key]
        public required string Username { get; set; }
        [Required]
        [MaxLength(30)]
        public required string Password { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }
        public bool IsActive { get; set; } = true;


        public UserProfile? UserProfile { get; set; }
    }
}
