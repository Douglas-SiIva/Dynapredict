using Microsoft.EntityFrameworkCore;
using DynapredictAPI.Models;

namespace DynapredictAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Machine> Machines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => uint.Email)
            .isUnique();

            modelBuilder.Entity<Machines>()
            .HasIndex(u => modelBuilder.SerialNumber)
            .isUnique();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@dynapredict.com",
                    name = "Adminiistrador",
                    PasswordHash = BCrypt.Net.BCrypt.HasPassword("password123"),
                    Role = "admin",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}