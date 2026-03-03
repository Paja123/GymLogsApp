using Application.Feature.TrainingSessions.Commands.Create;
using Application.Feature.TrainingSessions.Queries.GetAll;
using Application.Feature.TrainingSessions.Queries.GetMonthlyReport;
using Application.Feature.TrainingSessions.Commands.Delete;
using Application.Feature.TrainingSessions.Queries.GetMonthlyReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Web_API.Controllers
{
    public class TrainingSessionController : ApiControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IReadOnlyList<TrainingSessionResponseDto>> GetAll()
        {
            return await Mediator.Send(new GetAllTrainingSessionsQuery());
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainingSessionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteTrainingSessionCommand(id));
        }
        [Authorize]
        [HttpGet("{year}/{month}")]
        public async Task<List<WeeklyReportDto>> GetMonthlyReport(int year, int month)
        {
            return await Mediator.Send(new GetMonthlyReportQuery(month, year));
        }
    }
}
