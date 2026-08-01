using Krisma.Domain.Common;
using Krisma.Domain.Enums;

namespace Krisma.Domain.Entities;

public class PullRequestReview
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid PullRequestId { get; private set; }
    public PullRequest? PullRequest { get; private set; }
    public Guid? ReviewerDeveloperId { get; private set; }
    public Developer? ReviewerDeveloper { get; private set; }
    public long GitHubReviewId { get; private set; }
    public PullRequestReviewState State { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; }

    private PullRequestReview() { }

    private PullRequestReview(
        Guid pullRequestId,
        long gitHubReviewId,
        PullRequestReviewState state,
        DateTimeOffset submittedAt,
        Guid? reviewerDeveloperId)
    {
        PullRequestId = pullRequestId;
        GitHubReviewId = gitHubReviewId;
        State = state;
        SubmittedAt = submittedAt;
        ReviewerDeveloperId = reviewerDeveloperId;
    }

    public static Result<PullRequestReview> Create(
        Guid pullRequestId,
        long gitHubReviewId,
        PullRequestReviewState state,
        DateTimeOffset submittedAt,
        Guid? reviewerDeveloperId = null)
    {
        if (pullRequestId == Guid.Empty)
            return Result.Failure<PullRequestReview>(new Error("pullRequestReview.pullRequestId.required", "Pull Request ID is required.", ErrorType.Validation));

        if (gitHubReviewId <= 0)
            return Result.Failure<PullRequestReview>(new Error("pullRequestReview.githubReviewId.invalid", "GitHub Review ID must be greater than zero.", ErrorType.Validation));

        return Result.Success(new PullRequestReview(
            pullRequestId,
            gitHubReviewId,
            state,
            submittedAt,
            reviewerDeveloperId));
    }
}