using Krisma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Application.Contracts;

public interface IDevelopersRepository
{
    Task Add (Developer developer);
    Task<bool> ExistsByGitHubUserId(Guid organizationId, long gitHubUserId);
    Task<List<Developer>> GetAll();
    Task<Developer?> GetById(Guid id);
    Task DeactivateDeveloper(Guid id);
    Task ActivateDeveloper(Guid id);
    Task<List<Developer>> GetByOrganizationId(Guid organizationId);

}
