using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TrainingSessions.Queries.GetWeeklyReport
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
