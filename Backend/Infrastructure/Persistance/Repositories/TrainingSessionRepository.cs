using Application.Common.Interfaces;
using Application.TrainingSessions.Queries.GetAll;
using Domain.Entities;
using Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Persistance.Repositories
{
    public class TrainingSessionRepository : ITrainingSessionRepository
    {
        private readonly AppDbContext _context;

        public TrainingSessionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<TrainingSession>> GetAllAsync(string userId)
        {
            return await _context.TrainingSessions
                .Where(ts => ts.UserId== userId)
                .ToListAsync();
        }

        public async Task<Guid> AddAsync(TrainingSession session)
        {
            _context.TrainingSessions.Add(session);
            await _context.SaveChangesAsync();

            return session.Id;
        }

        public async Task<bool> OverlapingSessionExists(string userId, DateTime date, int duration)
        {
            userId = "11111111-1111-1111-1111-111111111111"; //TODO: Get actual userId from jwt
            return await _context.TrainingSessions.AnyAsync(s =>
                s.UserId == userId &&
                s.Date < date.AddMinutes(duration) &&
                s.Date.AddMinutes(s.Duration) > date);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _context.TrainingSessions.Where(ts => ts.Id == id).ExecuteDeleteAsync() > 0;
        }
    }
}
