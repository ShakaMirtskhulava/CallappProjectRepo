using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using MVC.ViewModels;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IConfiguration _configuration;
        private readonly string _apiRootUrl;
        private readonly string _jwt;


        public UserController(IConfiguration configuration, IAuthenticationHelper authenticationHelper)
        {
            _authenticationHelper = authenticationHelper;
            _configuration = configuration;
            _apiRootUrl = _configuration["API:APIRootUrl"]!;
            _jwt = _configuration["API:JWT"]!;
        }


        [HttpGet]
        public async Task<IActionResult> ProfilePage(string personalNumber)
        {
            //Check if the user is authenticated
            var currentUser = await _authenticationHelper.GetCurrentUserProfileDTOAsyn(Request);
            if (currentUser == null)
                return RedirectToAction("Login", "Authentication");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                var url = _apiRootUrl + @$"Authentication/GetUserProfile?personalNumber={personalNumber}";

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return NotFound("User couldn't be found");

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseContent);
                var userProfile = responseObject.ToObject<UserProfileDTO>();
                if (userProfile == null)
                    return NotFound("User can't be found");

                return View(userProfile);
            }

        }

        [HttpGet]
        public async Task<IActionResult> DeleteUserProfile(string personalNumber)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                    var url = _apiRootUrl + @$"Authentication/DeleteUserProfile?personalNumber={personalNumber}";

                    var response = await client.DeleteAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return BadRequest("Couldn't delete the user");

                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return BadRequest("Couldn't delete the user");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string personalNumber)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                var url = _apiRootUrl + @$"Authentication/GetUserProfile?personalNumber={personalNumber}";

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return NotFound("User couldn't be found");

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseContent);
                var userProfile = responseObject.ToObject<UserProfileDTO>();
                if (userProfile == null)
                    return NotFound("User can't be found");

                var VM = new EditUserVM
                {
                    PersonalNumber = personalNumber,
                    NewEmail = userProfile.Email,
                    NewFirstName = userProfile.FirstName,
                    NewLastName = userProfile.LastName,
                    IsActive = userProfile.IsActive
                };

                return View(VM);
            }     
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM VM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(VM);

                var url = _apiRootUrl + @$"Authentication/UpdateUserProfile?personalNumber={VM.PersonalNumber}";
                var dto = new UpdateUserProfileDTO
                {
                    Password = VM.NewPassword!,
                    Email = VM.NewEmail!,
                    IsActive = VM.IsActive,
                    FirstName = VM.NewFirstName!,
                    LastName = VM.NewLastName!
                };

                using(var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                    var json = JsonSerializer.Serialize(dto);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(url, data);

                    if (!response.IsSuccessStatusCode)
                        return BadRequest("Couldn't update the user");


                    return RedirectToAction("ProfilePage", "User",new { personalNumber = VM.PersonalNumber});
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
