using GymApp.Models;

namespace WorkoutApp.Repository.Interfaces
{
    public interface IWorkoutRepository
    {
        Task<ICollection<Workout>> GetAllAsync();
        Task<Workout> GetAsync(int id);
        Task CreateAsync(Workout coupon);
        Task UpdateAsync(Workout coupon);
        Task RemoveAsync(Workout coupon);
        Task SaveAsync();
    }
}
