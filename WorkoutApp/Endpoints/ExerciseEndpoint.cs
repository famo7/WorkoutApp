using AutoMapper;
using FluentValidation;
using GymApp.Models;
using WorkoutApp.Models.DTO;
using WorkoutApp.Repository.Interfaces;

namespace WorkoutApp.Endpoints
{
    public static class ExerciseEndpoint
    {
        public static void MapExerciseEndpoints(this WebApplication app)
        {
            var exercises = app.MapGroup("/exercises");


            exercises.MapPost("/workout/{id}", async (IWorkoutRepository repo, IMapper _mapper, IValidator<ExerciseCreateDTO> _validation, ExerciseCreateDTO exerciseToAdd, int id) =>
            {
                var validationResult = await _validation.ValidateAsync(exerciseToAdd);
                if (!validationResult.IsValid)
                {
                    var response = validationResult.Errors.FirstOrDefault().ToString();
                    return Results.BadRequest(response);

                }

                var workout = await repo.GetAsync(id);
                var exerciseExist = workout.Exercises.Any(i => i.Name == exerciseToAdd.Name);
                if (exerciseExist) return Results.Conflict();

                workout.Exercises.Add(_mapper.Map<Exercise>(exerciseToAdd));
                await repo.SaveAsync();
                return Results.Ok(workout);

            });

            exercises.MapGet("/workout/{id}", async (IWorkoutRepository repo, int id) =>
            {


                var workout = await repo.GetAsync(id);
                if (workout != null)
                {
                    return Results.Ok(workout.Exercises);

                }
                return Results.NotFound();
            });


            exercises.MapGet("/{id}", async (IExerciseRepository repo, int id) =>
            {
                var ex = await repo.GetOneAsync(id);
                if (ex == null) return Results.NotFound();
                return Results.Ok(ex);

            });

            exercises.MapPut("/{id}", async (IValidator<ExerciseUpdateDTO> _validation, IExerciseRepository repo, int id, ExerciseUpdateDTO exerciseToUpdate) =>
            {
                var validationResult = await _validation.ValidateAsync(exerciseToUpdate);
                if (!validationResult.IsValid)
                {
                    var response = validationResult.Errors.FirstOrDefault().ToString();
                    return Results.BadRequest(response);

                }

                var ex = await repo.GetOneAsync(id);
                if (ex == null) return Results.NotFound();
                ex.Name = exerciseToUpdate.Name;

                await repo.UpdateAsync(ex);
                await repo.SaveAsync();

                return Results.Ok(ex);

            });

            exercises.MapDelete("/{id}", async (IExerciseRepository repo, int id) =>
            {


                var ex = await repo.GetOneAsync(id);
                if (ex == null) return Results.NotFound();


                await repo.RemoveAsync(ex);
                await repo.SaveAsync();

                return Results.Ok(ex);

            });
        }
    }
}
