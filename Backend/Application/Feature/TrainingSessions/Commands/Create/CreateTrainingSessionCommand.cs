using Domain.Enums;
using MediatR;
using System;
namespace Application.Feature.TrainingSessions.Commands.Create
{
    public record CreateTrainingSessionCommand(
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
