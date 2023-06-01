using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;
using WEBAPI.Models;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository;


        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }


        [HttpPost("CreateUserProfile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserProfile([FromBody]UserProfileDTO dto)
        {
            try{

                //Validate the dto
                if (dto.PersonalNumber.Length != 11)
                    return BadRequest("Personal number length must be 11 character");

                //Firt we need to create the user
                var user = new User
                {
                    Username = dto.UserName,
                    Password = dto.Password,//Not hashing the passowrd since it was not required from the tasks description
                    Email = dto.Email,
                    IsActive = dto.IsActive
                };
                
                user = await _authenticationRepository.CreateUserAsync(user);
                //Now we can create the user profile
                var userProfile = new UserProfile
                {
                    Firstname = dto.FirstName,
                    Lastname = dto.LastName,
                    PersonalNumber = dto.PersonalNumber,
                    Username = user.Username
                };
                userProfile = await _authenticationRepository.CreateUserProfileAsync(userProfile);


                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserProfile")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(UserProfileDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserProfile(string personalNumber)
        {
            try{

                var userProfile = await _authenticationRepository.GetFullUserProfileAsync(personalNumber);
                if (userProfile == null)
                    return NotFound("User profile not found");

                //Due to app logic userProfile.User newer will be null so we can use null forgiving operator '!'
                var result = new UserProfileDTO
                {
                    UserName = userProfile.User!.Username,
                    Password = userProfile.User.Password,
                    Email = userProfile.User.Email,
                    IsActive = userProfile.User.IsActive,
                    FirstName = userProfile.Firstname,
                    LastName = userProfile.Lastname,
                    PersonalNumber = userProfile.PersonalNumber
                };

                return Ok(result);
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUserProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserProfile([FromQuery]string personalNumber,[FromBody] UpdateUserProfileDTO dto)
        {
            try
            {
                var userProfile = await _authenticationRepository.GetFullUserProfileAsync(personalNumber);
                if (userProfile == null || userProfile.User == null)
                    return BadRequest("User is not found");
                var user = userProfile.User;

                user.Password = dto.Password;
                user.Email = dto.Email;
                user.IsActive = dto.IsActive;

                await _authenticationRepository.UpdateUserAsync(user);

                userProfile.Firstname = dto.FirstName;
                userProfile.Lastname = dto.LastName;
                
                userProfile = await _authenticationRepository.UpdateUserProfileAsync(userProfile);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUserProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserProfile([FromQuery] string personalNumber)
        {
            try
            {
                var userProfile = await _authenticationRepository.GetFullUserProfileAsync(personalNumber);
                if (userProfile == null || userProfile.User == null)
                    return BadRequest("User is not found");
                var user = userProfile.User;

                _authenticationRepository.DeleteUserProfile(userProfile);
                _authenticationRepository.DeleteUser(user);
                await _authenticationRepository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
