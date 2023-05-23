using GymApp.Models;

namespace WorkoutApp.Repository.Interfaces
{
    public interface IExerciseRepository
    {
        Task<ICollection<Exercise>> GetAllAsync();
        Task<Exercise> GetOneAsync(int id);
        Task CreateAsync(Exercise exercise);
        Task UpdateAsync(Exercise exercise);
        Task RemoveAsync(Exercise exercise);
        Task SaveAsync();
    }
}
