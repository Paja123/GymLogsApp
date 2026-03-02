using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TrainingSessions.Queries.GetAll
{
    public record TrainingSessionResponseDto(
        string Id,
        string TrainingType,
        int Duration,
        int? CaloriesBurned,
        int IntensityLevel,
        int TirednessLevel,
        DateTime Date,
        string? Notes
)
    {
    }
}
