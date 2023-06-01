using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class EditUserVM
    {
        public required string PersonalNumber { get; set; }
        [Required]
        [MaxLength(30)]
        [PasswordPropertyText]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "New Email Address")]
        public string? NewEmail { get; set; }
        [Display(Name = "New Activity Status")]
        public bool IsActive { get; set; } = false;
        [Required]
        [MaxLength(30)]
        [Display(Name = "New Firstname")]
        public string? NewFirstName { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "New LastName")]
        public string? NewLastName { get; set; }
    }
}
