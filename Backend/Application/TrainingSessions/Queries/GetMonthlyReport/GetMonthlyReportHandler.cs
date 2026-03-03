using Application.Common.Interfaces;
using Application.TrainingSessions.Queries.GetWeeklyReport;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.TrainingSessions.Queries.GetMonthlyReport
{
    public class GetMonthlyReportHandler : IRequestHandler<GetMonthlyReportQuery, List<WeeklyReportDto>>
    {
        private readonly ITrainingSessionRepository _trainingSessionRepository;
        public GetMonthlyReportHandler(ITrainingSessionRepository trainingSessionRepository)
        {
            _trainingSessionRepository = trainingSessionRepository;
        }
        public async Task<List<WeeklyReportDto>> Handle(GetMonthlyReportQuery request, CancellationToken cancellationToken)
        {
            var monthSessions = await _trainingSessionRepository.GetByMonthAsync(request.Month, request.Year);

            //var firstDayOfMonth = new DateTime(year, month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var result = monthSessions
                .GroupBy(ts => ((ts.Date.Day - 1) / 7) + 1)
                .Select(g => new WeeklyReportDto(
                
                    g.Key,
                    g.Sum(ts => ts.Duration),
                    g.Count(),
                    g.Average(ts => ts.IntensityLevel),
                    g.Average(ts => ts.TirednessLevel)
                ))
                .OrderBy(x => x.WeekNumber)
                .ToList();

            return result;
        }
    }
}
