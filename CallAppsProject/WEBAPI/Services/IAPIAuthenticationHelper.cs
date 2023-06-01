namespace WEBAPI.Services
{
    public interface IAPIAuthenticationHelper
    {
        string GenerateJWT(string userName);
        string HashPassword(string password);
    }
}
