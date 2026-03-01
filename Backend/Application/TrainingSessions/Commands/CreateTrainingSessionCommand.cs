using Domain.Enums;
using MediatR;
using System;
namespace Application.TrainingSessions.Commands
{
    public record CreateTrainingSessionCommand(
        string UserId,
        TrainingType TrainingType,
        int Duration,
        int? CaloriesBurned,
        int IntensityLevel,
        int TirednessLevel,
        DateTime Date,
        string? Notes) : IRequest<Guid>
    {
    }
}
