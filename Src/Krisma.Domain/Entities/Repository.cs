using Krisma.Domain.Common;

namespace Krisma.Domain.Entities;

public class Repository
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid ProjectId { get; private set; }
    public Project? Project { get; private set; }
    public long GitHubRepositoryId { get; private set; }
    public string GitHubNodeId { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public DateTimeOffset? LastSynchronizedAt { get; private set; }

    private readonly List<Commit> _commits = [];
    public IReadOnlyCollection<Commit> Commits => _commits.AsReadOnly();

    private readonly List<PullRequest> _pullRequests = [];
    public IReadOnlyCollection<PullRequest> PullRequests => _pullRequests.AsReadOnly();

    private Repository() { }

    private Repository(
        Guid projectId,
        long gitHubRepositoryId,
        string gitHubNodeId,
        string fullName,
        DateTimeOffset? lastSynchronizedAt)
    {
        ProjectId = projectId;
        GitHubRepositoryId = gitHubRepositoryId;
        GitHubNodeId = gitHubNodeId;
        FullName = fullName;
        LastSynchronizedAt = lastSynchronizedAt;
    }

    public static Result<Repository> Create(
        Guid projectId,
        long gitHubRepositoryId,
        string gitHubNodeId,
        string fullName,
        DateTimeOffset? lastSynchronizedAt = null)
    {
        if (projectId == Guid.Empty)
            return Result.Failure<Repository>(new Error("repository.projectId.required", "Project ID is required.", ErrorType.Validation));

        if (gitHubRepositoryId <= 0)
            return Result.Failure<Repository>(new Error("repository.githubId.invalid", "GitHub Repository ID must be greater than zero.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(gitHubNodeId))
            return Result.Failure<Repository>(new Error("repository.nodeId.required", "GitHub Node ID is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(fullName))
            return Result.Failure<Repository>(new Error("repository.fullName.required", "Full name is required.", ErrorType.Validation));

        return Result.Success(new Repository(
            projectId,
            gitHubRepositoryId,
            gitHubNodeId.Trim(),
            fullName.Trim(),
            lastSynchronizedAt));
    }
}