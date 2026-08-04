using Krisma.Domain.Common;
using Krisma.Domain.Enums;

namespace Krisma.Domain.Entities;

public class PullRequest
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid RepositoryId { get; private set; }
    public Repository? Repository { get; private set; }
    public Guid? AuthorDeveloperId { get; private set; }
    public Developer? AuthorDeveloper { get; private set; }
    public Guid? MergedByDeveloperId { get; private set; }
    public Developer? MergedByDeveloper { get; private set; }
    public int Number { get; private set; }
    public string GitHubNodeId { get; private set; } = null!;
    public PullRequestState State { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? FirstReviewAt { get; private set; }
    public DateTimeOffset? MergedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }

    // Propiedad de navegación hacia las revisiones del PR
    private readonly List<PullRequestReview> _reviews = [];
    public IReadOnlyCollection<PullRequestReview> Reviews => _reviews.AsReadOnly();

    private PullRequest() { }

    private PullRequest(
        Guid repositoryId,
        int number,
        string gitHubNodeId,
        DateTimeOffset createdAt,
        Guid? authorDeveloperId,
        Guid? mergedByDeveloperId,
        PullRequestState state,
        DateTimeOffset? firstReviewAt,
        DateTimeOffset? mergedAt,
        DateTimeOffset? closedAt)
    {
        RepositoryId = repositoryId;
        Number = number;
        GitHubNodeId = gitHubNodeId;
        CreatedAt = createdAt;
        AuthorDeveloperId = authorDeveloperId;
        MergedByDeveloperId = mergedByDeveloperId;
        State = state;
        FirstReviewAt = firstReviewAt;
        MergedAt = mergedAt;
        ClosedAt = closedAt;
    }

    public static Result<PullRequest> Create(
        Guid repositoryId,
        int number,
        string gitHubNodeId,
        DateTimeOffset createdAt,
        Guid? authorDeveloperId = null,
        Guid? mergedByDeveloperId = null,
        PullRequestState state = PullRequestState.Open,
        DateTimeOffset? firstReviewAt = null,
        DateTimeOffset? mergedAt = null,
        DateTimeOffset? closedAt = null)
    {
        if (repositoryId == Guid.Empty)
            return Result.Failure<PullRequest>(new Error("pullRequest.repositoryId.required", "Repository ID is required.", ErrorType.Validation));

        if (number <= 0)
            return Result.Failure<PullRequest>(new Error("pullRequest.number.invalid", "Pull request number must be greater than zero.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(gitHubNodeId))
            return Result.Failure<PullRequest>(new Error("pullRequest.nodeId.required", "GitHub Node ID is required.", ErrorType.Validation));

        if (mergedAt.HasValue && mergedAt.Value < createdAt)
            return Result.Failure<PullRequest>(new Error("pullRequest.dates.mergedInvalid", "Merged date cannot be earlier than creation date.", ErrorType.Validation));

        if (closedAt.HasValue && closedAt.Value < createdAt)
            return Result.Failure<PullRequest>(new Error("pullRequest.dates.closedInvalid", "Closed date cannot be earlier than creation date.", ErrorType.Validation));

        return Result.Success(new PullRequest(
            repositoryId,
            number,
            gitHubNodeId.Trim(),
            createdAt,
            authorDeveloperId,
            mergedByDeveloperId,
            state,
            firstReviewAt,
            mergedAt,
            closedAt));
    }
}