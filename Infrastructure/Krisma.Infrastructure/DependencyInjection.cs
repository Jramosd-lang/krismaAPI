using Krisma.Application.Developers;
using Krisma.Infrastructure.Developers;
using Microsoft.Extensions.DependencyInjection;
namespace Krisma.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDeveloperRepository, InMemoryDeveloperRepository>();
        return services;
    }
}
