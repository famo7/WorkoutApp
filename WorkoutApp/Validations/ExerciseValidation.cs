using FluentValidation;
using WorkoutApp.Models.DTO;

namespace WorkoutApp.Validations
{
    public class ExerciseValidation : AbstractValidator<ExerciseCreateDTO>
    {
        public ExerciseValidation()
        {
            RuleFor(model => model.Name).NotEmpty().NotNull();
        }
    }
}
