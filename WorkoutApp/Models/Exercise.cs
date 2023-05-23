using System.Text.Json.Serialization;

namespace GymApp.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        [JsonIgnore]
        public List<Workout>? Workouts { get; set; }
        public List<Set>? Sets { get; set; }

    }
}