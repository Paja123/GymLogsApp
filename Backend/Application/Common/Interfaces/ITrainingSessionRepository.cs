using Application.Feature.TrainingSessions.Queries.GetAll;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ITrainingSessionRepository
    {
        public Task<IReadOnlyList<TrainingSession>> GetAllAsync(string userId);
        public Task<Guid> AddAsync(TrainingSession session);
        public Task<bool> OverlapingSessionExists(string userId, DateTime date, int duration);
        public Task<bool> DeleteAsync(Guid id);
        public Task<List<TrainingSession>> GetByMonthAsync(int month, int year, string userId);
    }
}
