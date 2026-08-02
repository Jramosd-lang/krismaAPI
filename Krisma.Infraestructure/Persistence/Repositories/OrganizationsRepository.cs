using Krisma.Application.Contracts;
using Krisma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Infraestructure.Persistence.Repositories;

public class OrganizationsRepository(ApplicationDbContext context) : IOrganizationsRepository
{
    public async Task Add(Organization organization)
    {
        context.Add(organization);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Exists(string name, long organizationId, string githubNodeId)
    {
        return await context.Organizations.AnyAsync(o =>
            o.Name == name || o.GitHubOrganizationId == organizationId || o.GitHubNodeId == githubNodeId);
    }

    public async Task<List<Organization>> GetAll()
    {
        return await context.Organizations.ToListAsync();
    }
}

