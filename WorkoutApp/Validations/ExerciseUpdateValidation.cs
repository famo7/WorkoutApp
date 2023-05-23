using FluentValidation;
using WorkoutApp.Models.DTO;

namespace WorkoutApp.Validations
{
    public class ExerciseUpdateValidation : AbstractValidator<ExerciseUpdateDTO>
    {
        public ExerciseUpdateValidation()
        {
            RuleFor(model => model.Name).NotEmpty().NotNull();

        }
    }
}
