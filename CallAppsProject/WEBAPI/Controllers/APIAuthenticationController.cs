using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;
using WEBAPI.Repositories;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIAuthenticationController : Controller
    {
        private readonly IAPIAuthenticationHelper _apiAuthenticationHelper;
        private readonly IAPIAuthenticationRepository _apiAuthenticationRepository;


        public APIAuthenticationController(IAPIAuthenticationHelper authenticationHelper, IAPIAuthenticationRepository authenticationRepository)
        {
            _apiAuthenticationHelper = authenticationHelper;
            _apiAuthenticationRepository = authenticationRepository;
        }


        [HttpPost("Register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromQuery] APIUserDTO dto)
        {
            try
            {
                var passwordHash = _apiAuthenticationHelper.HashPassword(dto.Password);
                var jwt = _apiAuthenticationHelper.GenerateJWT(dto.UserName);
                var result = await _apiAuthenticationRepository.CreateAPIUserAsync(dto.UserName, passwordHash, jwt);
                await _apiAuthenticationRepository.SaveChangesAsync();
                if(!result)
                    return BadRequest("User already exists");

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromQuery] APIUserDTO dto)
        {
            try
            {
                var passwordHash = _apiAuthenticationHelper.HashPassword(dto.Password);
                var user = await _apiAuthenticationRepository.GetAPIUserAsync(dto.UserName, passwordHash);
                if (user == null)
                    return BadRequest("User can't be found");



                return Ok(new {UserName = user.UserName, JWT = user.JWT});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
