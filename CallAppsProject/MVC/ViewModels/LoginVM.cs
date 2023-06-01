using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MaxLength(30)]
        public required string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        [PasswordPropertyText]
        public required string Password { get; set; }
    }
}
