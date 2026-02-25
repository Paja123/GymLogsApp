using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistance.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
    }
}
