using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEBAPI.Services;

namespace WEBAPI.Helpers
{
    public class APIAuthenticationHelper : IAPIAuthenticationHelper
    {
        private readonly IConfiguration _configuration;


        public APIAuthenticationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateJWT(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.Name, userName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecurityKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddYears(1), signingCredentials: creds);

            var JWT = new JwtSecurityTokenHandler().WriteToken(token);
            return JWT;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }
}
