using Application.TrainingSessions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    public class TrainingSessionController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainingSessionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
