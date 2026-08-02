using Krisma.Domain.Entities;
namespace Krisma.Application.Developers;
public interface IDeveloperRepository
{
    Task<bool> ExistsByGitHubUserIdAsync(Guid organizationId, long gitHubUserId, CancellationToken cancellationToken);
    Task AddAsync(Developer developer, CancellationToken cancellationToken);
}
