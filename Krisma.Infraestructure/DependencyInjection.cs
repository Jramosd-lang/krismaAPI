using Krisma.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Krisma.Infraestructure.Persistence;
using Krisma.Infraestructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Krisma.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(static options =>
            options.UseSqlServer("name=DefaultConnection"));


        services.AddScoped<IRepositoryDevelopers, RepositoryDevelopers>();

        return services;
    }
}
