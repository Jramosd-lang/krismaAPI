using FluentValidation;
using Krisma.Application.Mappers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Krisma.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<IDeveloperMapper, DeveloperMapper>();

        return services;
    }
}