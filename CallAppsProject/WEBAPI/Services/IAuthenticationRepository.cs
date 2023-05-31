using WEBAPI.Models;

namespace WEBAPI.Services
{
    public interface IAuthenticationRepository : IRepository
    {
        Task<bool> CreateAPIUserAsync(string userName, string passwordHash, string jwt);
        Task<APIUser?> GetAPIUserAsync(string userName, string passwordHash);
    }
}
