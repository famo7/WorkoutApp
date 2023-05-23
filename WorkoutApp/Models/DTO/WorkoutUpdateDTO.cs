namespace WorkoutApp.Models.DTO
{
    public class WorkoutUpdateDTO
    {
        public string Name { get; set; } = String.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
