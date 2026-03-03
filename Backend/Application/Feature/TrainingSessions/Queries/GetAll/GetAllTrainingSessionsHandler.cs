using Application.Common.Base;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.TrainingSessions.Queries.GetAll
{
    public class GetAllTrainingSessionsHandler : AuthorizedHandler<GetAllTrainingSessionsQuery, List<TrainingSessionResponseDto>>
    {
        private readonly ITrainingSessionRepository _trainingSessionRepository;
        public GetAllTrainingSessionsHandler(ITrainingSessionRepository trainingSessionRepository, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _trainingSessionRepository = trainingSessionRepository;
        }
        public override async Task<List<TrainingSessionResponseDto>> Handle(GetAllTrainingSessionsQuery request, CancellationToken cancellationToken)
        {
            var sessions = await _trainingSessionRepository.GetAllAsync(getUserId());
            var dtos = sessions.Select(ts => new TrainingSessionResponseDto(
                ts.Id.ToString(),
                ts.TrainingType.ToString(), 
                ts.Duration,
                ts.CaloriesBurned,
                ts.IntensityLevel,
                ts.TirednessLevel,
                ts.Date,
                ts.Notes
            )).ToList();

            return dtos;
        }

    }
}
