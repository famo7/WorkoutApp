namespace GymApp.Models
{
    public class Set
    {
        public int Id { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }

        public Exercise? Exercise { get; set; }
    }
}