using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;
using WEBAPI.Models;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IAuthenticationRepository _authenticationRepository;


        public AuthenticationController(IAuthenticationHelper authenticationHelper, IAuthenticationRepository authenticationRepository)
        {
            _authenticationHelper = authenticationHelper;
            _authenticationRepository = authenticationRepository;
        }


        [HttpPost("Register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] APIUserDTO dto)
        {
            try
            {
                var passwordHash = _authenticationHelper.HashPassword(dto.Password);
                var jwt = _authenticationHelper.GenerateJWT(dto.UserName);
                var result = await _authenticationRepository.CreateAPIUserAsync(dto.UserName, passwordHash, jwt);
                await _authenticationRepository.SaveChangesAsync();
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
                var passwordHash = _authenticationHelper.HashPassword(dto.Password);
                var user = await _authenticationRepository.GetAPIUserAsync(dto.UserName, passwordHash);
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
