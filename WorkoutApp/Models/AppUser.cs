using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{

    public class AppUser
    {
        public int Id { get; set; }


        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }
        public List<Workout> Workouts { get; set; } = new();

    }
}