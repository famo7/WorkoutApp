namespace WorkoutApp.Models.DTO
{
    public class WorkoutReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;
        public List<ExerciseReadDTO>? Exercises { get; set; }
    }
}
