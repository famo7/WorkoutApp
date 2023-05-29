using Microsoft.EntityFrameworkCore;

namespace GymApp.Models
{
    public class GymContext : DbContext
    {
        public GymContext(DbContextOptions<GymContext> options)
        : base(options) { }

        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<Set> Sets => Set<Set>();
        public DbSet<Workout> Workouts => Set<Workout>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user = new AppUser { Id = 1, Email = "test@gmail.com", UserName = "test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123") };

            modelBuilder.Entity<AppUser>().HasData(user);

        }
    }
}