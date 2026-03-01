using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using MediatR;


namespace Application.TrainingSessions.Commands.Create
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
            if(await OverlapingSessionExists(request.UserId, request.Date, request.Duration))
            {
                throw new TrainingSessionOverlapException();
               
            }

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

        public async Task<bool> OverlapingSessionExists(string userId, DateTime date, int duration)
        {
            return await _trainingSessionsRepository.OverlapingSessionExists(userId, date, duration);
        }
    }
}
