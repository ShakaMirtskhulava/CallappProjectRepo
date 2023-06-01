using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using MVC.ViewModels;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IConfiguration _configuration;
        private readonly string _apiRootUrl;
        private readonly string _jwt;


        public AuthenticationController(IAuthenticationHelper authenticationHelper, IConfiguration configuration)
        {
            _authenticationHelper = authenticationHelper;
            _configuration = configuration;
            _apiRootUrl = _configuration["API:APIRootUrl"]!;
            _jwt = _configuration["API:JWT"]!;
        }


        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM VM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(VM);

                var url = _apiRootUrl + @"Authentication/CreateUserProfile";

                var userProfileDTO = new UserProfileDTO
                {
                    UserName = VM.UserName,
                    Password = VM.Password,
                    Email = VM.Email,
                    IsActive = VM.IsActive,
                    FirstName = VM.FirstName,
                    LastName = VM.Lastname,
                    PersonalNumber = VM.PersonalNumber
                };
            
                using(var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                    StringContent content = new StringContent(JsonSerializer.Serialize(userProfileDTO), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                
                    if(response.IsSuccessStatusCode)
                    {
                        var url2 = _apiRootUrl + @$"Authentication/GetUserProfile?personalNumber={VM.PersonalNumber}";

                        var response2 = await client.GetAsync(url2);
                        var responseContent = await response2.Content.ReadAsStringAsync();
                        var responseObject = JObject.Parse(responseContent);
                        var userProfile = responseObject.ToObject<UserProfileDTO>();
                        if (userProfile == null)
                            return BadRequest("User can't be found");

                        var userJWT = _authenticationHelper.GenerateJWT(userProfile.PersonalNumber);
                        Response.Cookies.Append("userJWT", userJWT, new CookieOptions { HttpOnly = true,Expires = DateTime.Now.AddMinutes(10)});

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User already exists");
                        return View(VM);
                    }

                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var userProfileDto = await _authenticationHelper.GetCurrentUserProfileDTOAsyn(Request);
            if (userProfileDto != null)
                return RedirectToAction("ProfilePage", "Profile", userProfileDto);
            


            return View();
        }
    }
}
