using Domain.Enums;
using MediatR;
using System;
namespace Application.TrainingSessions.Commands
{
    public record CreateTrainingSessionCommand(
        TrainingType TrainingType,
        int Duration,
        int? CalloriesBurned,
        int IntensityLevel,
        int TirednessLevel,
        DateTime Date,
        string? Notes) : IRequest<Guid>
    {
    }
}
