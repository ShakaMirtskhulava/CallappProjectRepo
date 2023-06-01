using Microsoft.IdentityModel.Tokens;
using MVC.Services;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MVC.Helpers
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiRootUrl;
        private readonly string _jwt;

        public AuthenticationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiRootUrl = _configuration["API:APIRootUrl"]!;
            _jwt = _configuration["API:JWT"]!;
        }

        public async Task<UserProfileDTO?> GetCurrentUserProfileDTOAsyn(HttpRequest httpRequest)
        {
            try
            {
                string? userJWT = httpRequest.Cookies["userJWT"];
                if (userJWT == null)
                    return null;

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = tokenHandler.ReadJwtToken(userJWT);
                string? personalNumber = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (personalNumber == null)
                    return null;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                    var url = _apiRootUrl + @$"Authentication/GetUserProfile?personalNumber={personalNumber}";

                    var response = await client.GetAsync(url);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JObject.Parse(responseContent);
                    var userProfile = responseObject.ToObject<UserProfileDTO>();
                    if (userProfile == null)
                        return null;

                    return userProfile;
                }
            }
            catch
            {
                return null;
            }

        }

        public string GenerateJWT(string personalNumber)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, personalNumber),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecurityKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddYears(1), signingCredentials: creds);

            var JWT = new JwtSecurityTokenHandler().WriteToken(token);

            return JWT;
        }
    }
}
