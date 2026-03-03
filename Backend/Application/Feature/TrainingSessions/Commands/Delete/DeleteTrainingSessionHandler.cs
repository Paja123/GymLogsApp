using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.TrainingSessions.Commands.Delete
{
    public class DeleteTrainingSessionHandler : IRequestHandler<DeleteTrainingSessionCommand, bool>
    {
        private readonly ITrainingSessionRepository _trainingSessionRepository;
        public DeleteTrainingSessionHandler(ITrainingSessionRepository trainingSessionRepository)
        {
            _trainingSessionRepository = trainingSessionRepository;
        }
        public async Task<bool> Handle(DeleteTrainingSessionCommand request, CancellationToken cancellationToken)
        {
            return await _trainingSessionRepository.DeleteAsync(request.Id);
        }
    }
}
