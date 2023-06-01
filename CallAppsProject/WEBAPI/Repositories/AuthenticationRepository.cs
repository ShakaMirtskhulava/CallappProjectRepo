using Microsoft.EntityFrameworkCore;
using WEBAPI.Data;
using WEBAPI.Models;
using WEBAPI.Services;

namespace WEBAPI.Repositories
{
    public class AuthenticationRepository : Repository, IAuthenticationRepository
    {
        public AuthenticationRepository(AppDbContext dbContext) : base(dbContext)
        {}


        public async Task<User> CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile)
        {
            await _dbContext.UserProfiles.AddAsync(userProfile);
            await _dbContext.SaveChangesAsync();
            return userProfile;
        }

        public async Task<UserProfile?> GetFullUserProfileAsync(string personalNumber){
            
            return await _dbContext.UserProfiles
                        .Include(up => up.User)
                        .FirstOrDefaultAsync(up => up.PersonalNumber == personalNumber);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserProfile> UpdateUserProfileAsync(UserProfile userProfile)
        {
            _dbContext.UserProfiles.Update(userProfile);
            await _dbContext.SaveChangesAsync();
            return userProfile;
        }

        public bool DeleteUserProfile(UserProfile userProfile)
        {
            _dbContext.UserProfiles.Remove(userProfile);
            return true;
        }

        public bool DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
            return true;
        }


    }
}
