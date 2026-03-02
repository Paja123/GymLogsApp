using Application.TrainingSessions.Commands.Create;
using Application.TrainingSessions.Commands.Delete;
using Application.TrainingSessions.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    public class TrainingSessionController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IReadOnlyList<TrainingSessionResponseDto>> GetAll()
        {
            return await Mediator.Send(new GetAllTrainingSessionsQuery());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainingSessionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteTrainingSessionCommand(id));
        }
    }
}
