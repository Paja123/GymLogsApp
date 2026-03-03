using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.TrainingSessions.Queries.GetMonthlyReport
{
    public record WeeklyReportDto(
        int WeekNumber,
        int TotalDuration,
        int TrainingSessionsCount,
        double AverageIntensity,
        double AverageTiredness)
    {
    }
}
