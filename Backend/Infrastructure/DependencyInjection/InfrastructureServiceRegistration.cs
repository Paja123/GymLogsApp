using Application.Common.Interfaces;
using Infrastructure.Idenitity;
using Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();
            services.AddHttpContextAccessor();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
