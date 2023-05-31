namespace WEBAPI.Services
{
    public interface IAuthenticationHelper
    {
        string GenerateJWT(string userName);
        string HashPassword(string password);
    }
}
