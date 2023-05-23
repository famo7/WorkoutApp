using GymApp.Models;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Repository.Interfaces;

namespace WorkoutApp.Repository
{
    public class WorkoutRepository : IWorkoutRepository

    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GymContext _db;
        private string? UserName;
        public WorkoutRepository(GymContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            UserName = _httpContextAccessor.HttpContext.User.Identity.Name;
        }
        public async Task CreateAsync(Workout workout)
        {
            var user = _db.Users.Include(b => b.Workouts).Where(i => i.UserName == UserName).Single();
            user.Workouts.Add(workout);

        }

        public async Task<ICollection<Workout>> GetAllAsync()
        {
            return await _db.Workouts.Include(i => i.Exercises!).ThenInclude(i => i.Sets).Where(i => i.AppUser!.UserName == UserName).ToListAsync();
        }

        public async Task<Workout> GetAsync(int id)
        {
            return await _db.Workouts.Include(i => i.Exercises!).ThenInclude(i => i.Sets!).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task RemoveAsync(Workout workout)
        {
            _db.Workouts.Remove(workout);
        }


        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Workout workout)
        {
            _db.Workouts.Update(workout);
        }
    }
}
