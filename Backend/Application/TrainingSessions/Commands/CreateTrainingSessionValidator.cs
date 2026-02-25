using FluentValidation;

namespace Application.TrainingSessions.Commands
{
    public class CreateTrainingSessionValidator : AbstractValidator<CreateTrainingSessionCommand>
    {
        public CreateTrainingSessionValidator()
        {
            RuleFor(x => x.TrainingType).IsInEnum();
            RuleFor(x => x.Duration).GreaterThan(0).WithMessage("Duration should be greater than zero");
            RuleFor(x => x.IntensityLevel).InclusiveBetween(1, 10).WithMessage("Intesity Level should be in range 1-10");
            RuleFor(x => x.TirednessLevel).InclusiveBetween(1, 10).WithMessage("Tiredness Level should be in range 1-10"); ;
        }
    }
}
