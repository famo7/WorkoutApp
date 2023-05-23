namespace GymApp.Models
{

    public class Workout
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Exercise>? Exercises { get; set; }
        public AppUser? AppUser { get; set; }

    }
}