using Krisma.Application.Contracts;
using Krisma.Domain.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Krisma.Infraestructure.Persistence.Repositories;

public class RepositoryDevelopers(ApplicationDbContext context) : IRepositoryDevelopers
{
    public async Task Add(Developer developer)
    {
        context.Add(developer);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByGitHubLogin(string gitHubLogin)
    {
        return await context.Developers.AnyAsync(d => d.GitHubLogin == gitHubLogin);
    }

    public async Task<List<Developer>> GetAllDevelopers()
    {
        return await context.Developers.ToListAsync();
    }
}
