using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.TrainingSessions.Queries.GetAll
{
    public class GetAllTrainingSessionsHandler : IRequestHandler<GetAllTrainingSessionsQuery, List<TrainingSessionResponseDto>>
    {
        private readonly ITrainingSessionRepository _trainingSessionRepository;

        public GetAllTrainingSessionsHandler(ITrainingSessionRepository trainingSessionRepository)
        {
            _trainingSessionRepository = trainingSessionRepository;
        }

        public async Task<List<TrainingSessionResponseDto>> Handle(GetAllTrainingSessionsQuery request, CancellationToken cancellationToken)
        {
            string userId = "11111111-1111-1111-1111-111111111111"; // TODO: Get user ID from the context
                            

            var sessions = await _trainingSessionRepository.GetAllAsync(userId);
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
