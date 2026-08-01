using Krisma.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Krisma.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<IDeveloperMapper, DeveloperMapper>();
        
        // futuros servicios, validadres, etc

        return services;
    }
}