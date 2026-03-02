using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TrainingSessions.Commands.Delete
{
    public record DeleteTrainingSessionCommand(Guid Id) : IRequest<bool>
    {
    }
}
