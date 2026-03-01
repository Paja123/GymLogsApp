using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance.Context;
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
        public async Task<Guid> AddAsync(TrainingSession session)
        {
            _context.TrainingSessions.Add(session);
            await _context.SaveChangesAsync();

            return session.Id;
        }
    }
}
