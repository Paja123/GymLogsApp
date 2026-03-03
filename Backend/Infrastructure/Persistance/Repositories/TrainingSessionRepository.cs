using Application.Common.Interfaces;
using Application.Feature.TrainingSessions.Queries.GetAll;
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
            return await _context.TrainingSessions.AnyAsync(s =>
                s.UserId == userId &&
                s.Date < date.AddMinutes(duration) &&
                s.Date.AddMinutes(s.Duration) > date);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _context.TrainingSessions.Where(ts => ts.Id == id).ExecuteDeleteAsync() > 0;
        }

        public async Task<List<TrainingSession>> GetByMonthAsync(int month, int year, string userId)
        {
            return await _context.TrainingSessions
                .Where(ts => ts.UserId == userId && ts.Date.Month == month && ts.Date.Year == year)
                .ToListAsync();
        }
    }
}
