using Application.TrainingSessions.Queries.GetWeeklyReport;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TrainingSessions.Queries.GetMonthlyReport
{
    public record GetMonthlyReportQuery(int Month, int Year): IRequest<List<WeeklyReportDto>>
    {
    }
}
