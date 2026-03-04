using Application.Common.Base;
using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using MediatR;


namespace Application.Feature.TrainingSessions.Commands.Create
{
    public class CreateTrainingSessionHandler : AuthorizedHandler<CreateTrainingSessionCommand, Guid>
    {
        private readonly ITrainingSessionRepository _trainingSessionsRepository;
        public CreateTrainingSessionHandler(ITrainingSessionRepository trainingSessionsRepository, ICurrentUserService currentUserService) : base(currentUserService)                        
        {
            _trainingSessionsRepository = trainingSessionsRepository;
        }
        public override async Task<Guid> Handle(CreateTrainingSessionCommand request, CancellationToken cancellationToken)
        {   
            string userId = getUserId();
            if (await OverlapingSessionExists(userId, request.Date, request.Duration))
            {
                throw new TrainingSessionOverlapException();
            }

            var session = new TrainingSession
            {
                UserId = userId,
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
