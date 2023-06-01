using SharedLibrary.DTOs;

namespace MVC.Services
{
    public interface IAuthenticationHelper
    {
        string GenerateJWT(string personalNumber);
        Task<UserProfileDTO?> GetCurrentUserProfileDTOAsyn(HttpRequest httpRequest);
        void RemoveJWTCookies(HttpResponse httpResponse);
    }
}
