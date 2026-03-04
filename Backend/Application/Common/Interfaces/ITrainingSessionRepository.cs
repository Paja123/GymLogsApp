using Application.Feature.TrainingSessions.Queries.GetAll;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ITrainingSessionRepository
    {
        Task<IReadOnlyList<TrainingSession>> GetAllAsync(string userId);
        Task<Guid> AddAsync(TrainingSession session);
        Task<bool> OverlapingSessionExists(string userId, DateTime date, int duration);
        Task<bool> DeleteAsync(Guid id);
        Task<List<TrainingSession>> GetByMonthAsync(int month, int year, string userId);
    }
}
