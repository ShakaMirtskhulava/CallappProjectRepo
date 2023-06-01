using Microsoft.EntityFrameworkCore;
using WEBAPI.Models;

namespace WEBAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<APIUser> APIUsers { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<User> Users { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne(up => up.UserProfile)
                .HasForeignKey<UserProfile>(up => up.Username)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProfile>()
                .Property(x => x.PersonalNumber)
                .HasColumnType("char(11)")
                .HasMaxLength(11);

        }

    }
}
