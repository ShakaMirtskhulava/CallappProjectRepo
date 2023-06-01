using WEBAPI.Models;

namespace WEBAPI.Services
{
    public interface IAuthenticationRepository : IRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile);
        Task<UserProfile?> GetFullUserProfileAsync(string personalNumber);
        Task<User> UpdateUserAsync(User user);
        Task<UserProfile> UpdateUserProfileAsync(UserProfile userProfile);
        bool DeleteUserProfile(UserProfile userProfile);
        bool DeleteUser(User user);
        Task<UserProfile?> GetFullUserProfileAsync(string userName, string password);
        Task<List<UserProfile>> GetAllUserProfileAsync();
    }
}
