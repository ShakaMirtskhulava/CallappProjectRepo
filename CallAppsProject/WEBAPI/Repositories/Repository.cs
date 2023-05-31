using WEBAPI.Data;
using WEBAPI.Services;

namespace WEBAPI.Repositories
{
    public class Repository
    {

        protected readonly AppDbContext _dbContext;


        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
