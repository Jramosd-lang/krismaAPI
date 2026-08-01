using Krisma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Application.Contracts;

public interface IRepositoryDevelopers
{
    Task Add (Developer developer);
    Task<bool> ExistsByGitHubLogin(string gitHubLogin);
    Task<List<Developer>> GetAllDevelopers();
}
