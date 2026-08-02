using Krisma.Application.Contracts;
using Krisma.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Krisma.Infraestructure.Persistence.Repositories;

public class DevelopersRepository(ApplicationDbContext context) : IDevelopersRepository
{
    public async Task ActivateDeveloper(Guid id)
    {
        var developer = await context.Developers.FindAsync(id);
        if (developer is not null)
        {
            developer.Activate();
            await context.SaveChangesAsync();
        }
    }

    public async Task Add(Developer developer)
    {
        context.Add(developer);
        await context.SaveChangesAsync();
    }

    public async Task DeactivateDeveloper(Guid id)
    {
        var developer = await context.Developers.FindAsync(id);
        if (developer is not null)
        {
            developer.Deactivate();
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByGitHubUserId(Guid organizationId, long gitHubUserId)
    {
        return await context.Developers.AnyAsync(d =>
            d.OrganizationId == organizationId && d.GitHubUserId == gitHubUserId);
    }

    public async Task<List<Developer>> GetAll()
    {
        return await context.Developers.ToListAsync();
    }

    public async Task<Developer?> GetById(Guid id)
    {
        var developer = await context.Developers
            .Include(d => d.Organization)
            .Include(d => d.Technologies)
            .FirstOrDefaultAsync(d => d.Id == id);

        return developer;
    }

    public async Task<List<Developer>> GetByOrganizationId(Guid organizationId)
    {
        var developers = await context.Developers
            .Include(d => d.Organization)
            .Include(d => d.Technologies)
            .Where(d => d.OrganizationId == organizationId)
            .ToListAsync();

        return developers;
    }
}