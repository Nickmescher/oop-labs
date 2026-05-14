using Lab5.Application.Abstractions;
using Lab5.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddSingleton<IOperationRepository, InMemoryOperationRepository>();
        services.AddSingleton<ISessionRepository, InMemorySessionRepository>();
        return services;
    }
}
