using AutoMapper;
using FluentValidation;
using GymApp.DTO;
using GymApp.Models;
using WorkoutApp.Models.DTO;
using WorkoutApp.Repository.Interfaces;

namespace WorkoutApp.Endpoints
{
    public static class SetEndpoint
    {
        public static void MapSetEndpoints(this WebApplication app)
        {
            var sets = app.MapGroup("/sets");


            sets.MapPost("/exercise/{id}", async (IExerciseRepository repo, IMapper _mapper, IValidator<SetCreateUpdateDTO> _validation, SetCreateUpdateDTO setToCreate, int id) =>
            {
                var validationResult = await _validation.ValidateAsync(setToCreate);
                if (!validationResult.IsValid)
                {
                    var response = validationResult.Errors.FirstOrDefault().ToString();
                    return Results.BadRequest(response);

                }

                var exercise = await repo.GetOneAsync(id);

                if (exercise == null) return Results.NotFound();
                var set = _mapper.Map<Set>(setToCreate);
                exercise.Sets.Add(set);
                await repo.SaveAsync();
                return Results.Ok(set);

            });

            sets.MapGet("/exercise/{id}", async (IExerciseRepository repo, IMapper _mapper, int id) =>
            {


                var exercise = await repo.GetOneAsync(id);
                if (exercise != null)
                {
                    return Results.Ok(_mapper.Map<IEnumerable<SetReadDTO>>(exercise.Sets));

                }
                return Results.NotFound();
            });


            sets.MapGet("/{id}", async (GymContext _db, IMapper _mapper, int id) =>
            {
                var set = await _db.Sets.FindAsync(id);
                if (set == null) return Results.NotFound();
                return Results.Ok(_mapper.Map<SetReadDTO>(set));

            });

            sets.MapPut("/{id}", async (GymContext _db, IMapper _mapper, IValidator<SetCreateUpdateDTO> _validation, SetCreateUpdateDTO setToUpdate, int id) =>
            {
                var validationResult = await _validation.ValidateAsync(setToUpdate);
                if (!validationResult.IsValid)
                {
                    var response = validationResult.Errors.FirstOrDefault().ToString();
                    return Results.BadRequest(response);

                }

                var set = await _db.Sets.FindAsync(id);
                if (set == null) return Results.NotFound();

                set.Reps = setToUpdate.Reps;
                set.Weight = setToUpdate.Weight;
                _db.Update(set);
                await _db.SaveChangesAsync();
                return Results.Ok();

            });

            sets.MapDelete("/{id}", async (GymContext _db, int id) =>
            {
                var set = await _db.Sets.FindAsync(id);
                if (set == null) return Results.NotFound();

                _db.Remove(set);
                await _db.SaveChangesAsync();
                return Results.NoContent();

            });

        }
    }
}
