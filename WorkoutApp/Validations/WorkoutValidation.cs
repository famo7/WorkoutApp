using FluentValidation;
using WorkoutApp.Models.DTO;

namespace WorkoutApp.Validations
{
    public class WorkoutValidation : AbstractValidator<WorkoutCreateDTO>
    {
        public WorkoutValidation()
        {

            RuleFor(model => model.Name).NotEmpty().NotNull();

        }
    }
}
