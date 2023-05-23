using GymApp.Models;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Repository.Interfaces;

namespace WorkoutApp.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {

        private readonly GymContext _db;

        public ExerciseRepository(GymContext db)
        {
            _db = db;
        }
        public Task CreateAsync(Exercise exercise)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Exercise>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise> GetOneAsync(int id)
        {
            return await _db.Exercises.Include(i => i.Sets!).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task RemoveAsync(Exercise ex)
        {
            _db.Exercises.Remove(ex);

        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exercise exercise)
        {
            _db.Exercises.Update(exercise);
        }
    }
}
