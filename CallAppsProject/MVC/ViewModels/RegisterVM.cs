using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(30)]
        public required string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        [PasswordPropertyText]
        public required string Password { get; set; }
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The Personal Number must be exactly 11 digits.")]
        public required string PersonalNumber { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MaxLength(30)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public required string Lastname { get; set; }
        public bool IsActive { get; set; } = false;  
    }
}
