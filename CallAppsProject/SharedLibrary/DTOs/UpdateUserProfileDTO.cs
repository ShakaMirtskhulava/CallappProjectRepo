using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class UpdateUserProfileDTO
    {
        public required string Password { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
