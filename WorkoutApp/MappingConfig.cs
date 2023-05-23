using AutoMapper;
using GymApp.DTO;
using GymApp.Models;
using WorkoutApp.Models.DTO;

namespace WorkoutApp
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<WorkoutCreateDTO, Workout>().ReverseMap();
            CreateMap<WorkoutReadDTO, Workout>().ReverseMap();
            CreateMap<WorkoutUpdateDTO, Workout>().ReverseMap();
            CreateMap<ExerciseCreateDTO, Exercise>().ReverseMap();
            CreateMap<ExerciseReadDTO, Exercise>().ReverseMap();

            CreateMap<Set, SetCreateUpdateDTO>().ReverseMap();
            CreateMap<Set, SetReadDTO>().ReverseMap();



        }
    }
}
