using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ITrainingSessionRepository
    {
        public Task<Guid> AddAsync(TrainingSession session);
        public Task<bool> OverlapingSessionExists(string userId, DateTime date, int duration);
    }
}
