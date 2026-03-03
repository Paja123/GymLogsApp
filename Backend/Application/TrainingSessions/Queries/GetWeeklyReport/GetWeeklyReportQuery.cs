using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TrainingSessions.Queries.GetWeeklyReport
{
    public record GetWeeklyReportQuery(int Month, int Year): IRequest<List<WeeklyReportDto>>
    {
    }
}
