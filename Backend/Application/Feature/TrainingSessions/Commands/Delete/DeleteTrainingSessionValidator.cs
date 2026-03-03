using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.TrainingSessions.Commands.Delete
{
    internal class DeleteTrainingSessionValidator : AbstractValidator<DeleteTrainingSessionCommand>
    {
        public DeleteTrainingSessionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
