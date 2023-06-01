using Microsoft.EntityFrameworkCore;
using WEBAPI.Data;
using WEBAPI.Models;
using WEBAPI.Services;

namespace WEBAPI.Repositories
{
    public class APIAuthenticationRepository : Repository, IAPIAuthenticationRepository
    {



        public APIAuthenticationRepository(AppDbContext dbContext) : base(dbContext)
        {}

        public async Task<bool> CreateAPIUserAsync(string userName,string passwordHash,string jwt)
        {
            var targetUser = await GetAPIUserAsync(userName, passwordHash);
            if (targetUser != null)
                return false;

            var user = new APIUser
            {
                UserName = userName,
                PasswordHash = passwordHash,
                JWT = jwt
            };

            await _dbContext.APIUsers.AddAsync(user);
            
            return true;
        }

        public async Task<APIUser?> GetAPIUserAsync(string userName, string passwordHash)
        {
            var user = await _dbContext.APIUsers.FirstOrDefaultAsync(u => u.UserName == userName && u.PasswordHash == passwordHash);
            
            return user;
        }


    }
}
