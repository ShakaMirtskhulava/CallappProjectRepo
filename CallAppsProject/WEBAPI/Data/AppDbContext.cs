using Microsoft.EntityFrameworkCore;
using WEBAPI.Models;

namespace WEBAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<APIUser> APIUsers { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
        
    }
}
