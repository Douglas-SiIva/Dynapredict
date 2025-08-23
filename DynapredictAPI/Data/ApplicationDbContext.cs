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
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Machine>()
                .HasIndex(m => m.SerialNumber)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@dynapredict.com",
                    Name = "Administrador",
                    PasswordHash = "$2a$11$XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", // hash fixo
                    Role = "admin",
                    CreatedAt = new DateTime(2025, 8, 22, 0, 0, 0, DateTimeKind.Utc) // valor fixo
                }
            );

        }
    }
}