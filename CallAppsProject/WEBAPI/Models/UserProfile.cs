using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Models
{
    public class UserProfile
    {
        [Key]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Password must be exactly 11 digits.")]
        public required string PersonalNumber { get; set; }
        [Required]
        [MaxLength(30)]
        public required string Firstname { get; set; }
        [Required]
        [MaxLength(30)]
        public required string Lastname { get; set; }
        

        public required string? Username { get; set; }
        public User? User { get; set; }

    }
}
