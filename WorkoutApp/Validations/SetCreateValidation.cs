using FluentValidation;
using WorkoutApp.Models.DTO;

namespace WorkoutApp.Validations
{
    public class SetCreateValidation : AbstractValidator<SetCreateUpdateDTO>
    {
        public SetCreateValidation()
        {
            RuleFor(model => model.Reps).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(model => model.Weight).NotEmpty().NotNull().GreaterThan(0);

        }
    }
}
