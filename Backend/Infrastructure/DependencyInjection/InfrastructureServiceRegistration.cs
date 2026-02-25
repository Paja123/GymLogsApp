using Application.Common.Interfaces;
using Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();
            return services;
        }
    }
}
