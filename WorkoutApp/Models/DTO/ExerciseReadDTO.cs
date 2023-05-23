using GymApp.DTO;

namespace WorkoutApp.Models.DTO
{
    public class ExerciseReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public List<SetReadDTO>? Sets { get; set; }
    }
}
