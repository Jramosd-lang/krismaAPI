using Krisma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Application.Contracts;

public interface IOrganizationsRepository
{
    Task Add(Organization organization);
    Task<bool> Exists(string name, long organizationId, string githubNodeId);
    Task<List<Organization>> GetAll(); 
}
