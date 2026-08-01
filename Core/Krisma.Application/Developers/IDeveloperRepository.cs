using Krisma.Domain.Entities;
namespace Krisma.Application.Developers;
public interface IDeveloperRepository
{
    Task<bool> ExistsByGitHubLoginAsync(string gitHubLogin, CancellationToken cancellationToken);
    Task AddAsync(Developer developer, CancellationToken cancellationToken);
}
