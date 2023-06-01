using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using MVC.ViewModels;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

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
                        Response.Cookies.Append("userJWT", userJWT, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddDays(10) });

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
            //Check if the user is authenticated
            var currentUser = await _authenticationHelper.GetCurrentUserProfileDTOAsyn(Request);
            if (currentUser != null)
                return RedirectToAction("ProfilePage", "User", new { personalNumber = currentUser.PersonalNumber });

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM VM)
        {
            try
            {
                if(!ModelState.IsValid)
                    return View(VM);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                    var url = _apiRootUrl + @$"Authentication/GetUserProfileByUserNameAndPassword?userName={VM.UserName}&password={VM.Password}";

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("UserName", "User can't be found");
                        return View(VM);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JObject.Parse(responseContent);
                    var userProfile = responseObject.ToObject<UserProfileDTO>();
                    if (userProfile == null)
                        return NotFound("User can't be found");

                    var userJWT = _authenticationHelper.GenerateJWT(userProfile.PersonalNumber);
                    Response.Cookies.Append("userJWT", userJWT, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddDays(10) });

                    return RedirectToAction("ProfilePage", "User", new { personalNumber = userProfile.PersonalNumber });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                _authenticationHelper.RemoveJWTCookies(Response);

                return RedirectToAction("Login");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
