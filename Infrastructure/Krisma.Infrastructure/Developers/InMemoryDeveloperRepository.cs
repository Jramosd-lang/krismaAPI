using System.Collections.Concurrent;
using Krisma.Application.Developers;
using Krisma.Domain.Entities;
namespace Krisma.Infrastructure.Developers;
public sealed class InMemoryDeveloperRepository : IDeveloperRepository
{
    private readonly ConcurrentDictionary<Guid, Developer> _developers = new();
    public Task<bool> ExistsByGitHubLoginAsync(string gitHubLogin, CancellationToken cancellationToken) => Task.FromResult(_developers.Values.Any(x => string.Equals(x.GitHubLogin, gitHubLogin, StringComparison.OrdinalIgnoreCase)));
    public Task AddAsync(Developer developer, CancellationToken cancellationToken) { _developers.TryAdd(developer.Id, developer); return Task.CompletedTask; }
}
