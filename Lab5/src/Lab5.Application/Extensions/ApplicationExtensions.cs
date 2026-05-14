using Lab5.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string adminPassword)
    {
        services.AddScoped<AccountService>();
        services.AddScoped<SessionService>(sp => new SessionService(
            sp.GetRequiredService<Abstractions.ISessionRepository>(),
            sp.GetRequiredService<Abstractions.IAccountRepository>(),
            sp.GetRequiredService<AccountService>(),
            adminPassword));

        return services;
    }
}
