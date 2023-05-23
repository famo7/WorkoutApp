using AutoMapper;
using FluentValidation;
using GymApp.Models;
using WorkoutApp.Models.DTO;
using WorkoutApp.Repository.Interfaces;

namespace GymApp.Endpoints
{

    public static class WorkoutEndpoints
    {

        public static void MapWorkoutEndpoints(this WebApplication app)
        {
            var workouts = app.MapGroup("/workouts");
            workouts.MapGet("/", async (IWorkoutRepository repo, IMapper _mapper) =>
            {
                return Results.Ok(_mapper.Map<IEnumerable<WorkoutReadDTO>>(await repo.GetAllAsync()));

            });

            workouts.MapGet("/{id}", async (GymContext db, IWorkoutRepository repo, IMapper _mapper, int id) =>
            {
                return Results.Ok(_mapper.Map<WorkoutReadDTO>(await repo.GetAsync(id)));
            });

            workouts.MapPost("/", async (GymContext db, IWorkoutRepository repo, IMapper _mapper, IValidator<WorkoutCreateDTO> _validation, WorkoutCreateDTO workoutToAdd) =>
            {
                var workouts = await repo.GetAllAsync();
                var w = workouts.Any(i => i.Name == workoutToAdd.Name);
                if (w) return Results.Conflict();
                var validationResult = await _validation.ValidateAsync(workoutToAdd);
                if (!validationResult.IsValid)
                {
                    var response = validationResult.Errors.FirstOrDefault().ToString();
                    return Results.BadRequest(response);

                }

                Workout workout = _mapper.Map<Workout>(workoutToAdd);

                await repo.CreateAsync(workout);
                await repo.SaveAsync();
                return Results.Ok();
            });

            workouts.MapPut("/{id}", async (IWorkoutRepository repo, IMapper _mapper, IValidator<WorkoutUpdateDTO> _validation, WorkoutUpdateDTO updateWorkoutDTO, int id) =>
            {
                var validationResult = await _validation.ValidateAsync(updateWorkoutDTO);
                if (!validationResult.IsValid)
                {
                    var response = validationResult.Errors.FirstOrDefault().ToString();
                    return Results.BadRequest(response);

                }


                if (await repo.GetAsync(id) is Workout workout)
                {

                    workout.Name = updateWorkoutDTO.Name;
                    await repo.SaveAsync();
                    return Results.Ok();
                }
                return Results.NotFound();

            });

            workouts.MapDelete("/{id}", async (GymContext db, IWorkoutRepository repo, IMapper _mapper, int id) =>
            {
                if (await repo.GetAsync(id) is Workout workout)
                {

                    await repo.RemoveAsync(workout);
                    await repo.SaveAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();

            });


        }
    }
}