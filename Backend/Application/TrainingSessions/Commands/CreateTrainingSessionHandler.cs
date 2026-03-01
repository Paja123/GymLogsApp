using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;


namespace Application.TrainingSessions.Commands
{
    public class CreateTrainingSessionHandler : IRequestHandler<CreateTrainingSessionCommand, Guid>
    {
        private readonly ITrainingSessionRepository _trainingSessionsRepository;
        public CreateTrainingSessionHandler(ITrainingSessionRepository trainingSessionsRepository)
        {
            _trainingSessionsRepository = trainingSessionsRepository;
        }
        public async Task<Guid> Handle(CreateTrainingSessionCommand request, CancellationToken cancellationToken)
        {
            var session = new TrainingSession
            {
                TrainingType = request.TrainingType,
                Duration = request.Duration,
                CaloriesBurned = request.CaloriesBurned,
                IntensityLevel = request.IntensityLevel,
                TirednessLevel = request.TirednessLevel,
                Date = request.Date,
                Notes = request.Notes
            };
            return await _trainingSessionsRepository.AddAsync(session);
        }
    }
}
