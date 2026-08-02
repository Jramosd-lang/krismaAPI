using System.Collections.Concurrent;
using Krisma.Application.Developers;
using Krisma.Domain.Entities;
namespace Krisma.Infrastructure.Developers;
public sealed class InMemoryDeveloperRepository : IDeveloperRepository
{
    private readonly ConcurrentDictionary<Guid, Developer> _developers = new();
    public Task<bool> ExistsByGitHubUserIdAsync(Guid organizationId, long gitHubUserId, CancellationToken cancellationToken) => Task.FromResult(_developers.Values.Any(x => x.OrganizationId == organizationId && x.GitHubUserId == gitHubUserId));
    public Task AddAsync(Developer developer, CancellationToken cancellationToken) { _developers.TryAdd(developer.Id, developer); return Task.CompletedTask; }
}
