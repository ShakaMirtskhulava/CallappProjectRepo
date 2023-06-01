using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using MVC.ViewModels;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs;
using System.Net.Http.Headers;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IConfiguration _configuration;
        private readonly string _apiRootUrl;
        private readonly string _jwt;


        public HomeController(IAuthenticationHelper authenticationHelper, IConfiguration configuration)
        {
            _authenticationHelper = authenticationHelper;
            _configuration = configuration;
            _apiRootUrl = _configuration["API:APIRootUrl"]!;
            _jwt = _configuration["API:JWT"]!;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                //Check if the user is authenticated
                var currentUser = await _authenticationHelper.GetCurrentUserProfileDTOAsyn(Request);
                if (currentUser == null)
                    return RedirectToAction("Login", "Authentication");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

                    var url = _apiRootUrl + @$"Authentication/GetAllUserProfile";

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return NotFound("Users aren't found");

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JArray.Parse(responseContent);
                    var userProfiles = responseObject.ToObject<List<GetAllUserProfileDTO>>();
                    if (userProfiles == null)
                        return NotFound("Users can't be found");

                    List<IndexUserProfileVM>? indexUserProfileVMs = null;
                    if (userProfiles != null && userProfiles.Count > 0)
                    {
                        indexUserProfileVMs = new List<IndexUserProfileVM>();
                        foreach (var up in userProfiles)
                        {
                            var indexUserProfileVM = new IndexUserProfileVM
                            {
                                FirstName = up.firstName,
                                LastName = up.lastName,
                                PersonalNumber = up.personalNumber
                            };
                            indexUserProfileVMs.Add(indexUserProfileVM);
                        }
                    }

                    var vm = new IndexVM
                    {
                        IndexUserProfileVMs = indexUserProfileVMs,
                        CurrentUserPersonalNumber = currentUser.PersonalNumber
                    };

                    return View(vm);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}