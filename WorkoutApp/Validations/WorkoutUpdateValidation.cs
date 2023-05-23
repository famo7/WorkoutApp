using FluentValidation;
using WorkoutApp.Models.DTO;

namespace WorkoutApp.Validations
{
    public class WorkoutUpdateValidation : AbstractValidator<WorkoutUpdateDTO>
    {
        public WorkoutUpdateValidation()
        {
            RuleFor(model => model.Name).NotEmpty().NotNull();

        }
    }
}
