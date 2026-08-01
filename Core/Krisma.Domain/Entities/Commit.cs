using Krisma.Domain.Common;

namespace Krisma.Domain.Entities;
public class Commit
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid RepositoryId { get; private set; } = default!;
    public Repository? Repository { get; private set; }
    public Guid? DeveloperId { get; private set; }
    public Developer? Developer { get; private set; }
    public string Sha { get; private set; } = null!;
    public string AuthorLogin { get; private set; } = null!;
    public string Message { get; private set; } = null!;
    public DateTimeOffset AuthoredAt { get; private set; }
    public DateTimeOffset CommittedAt { get; private set; }
    public int Additions { get; private set; }
    public int Deletions { get; private set; }
    private Commit() { }

    //constructor

    private Commit(
    Guid repositoryId,
    string sha,
    string authorLogin,
    string message,
    DateTimeOffset authoredAt,
    DateTimeOffset committedAt,
    int additions,
    int deletions,
    Guid? developerId = null)
    {
        RepositoryId = repositoryId;
        Sha = sha;
        AuthorLogin = authorLogin;
        Message = message;
        AuthoredAt = authoredAt;
        CommittedAt = committedAt;
        Additions = additions;
        Deletions = deletions;
        DeveloperId = developerId;
    }


    //funcion create

    public static Result<Commit> Create(
        Guid repositoryId,
        string sha,
        string authorLogin,
        string message,
        DateTimeOffset authoredAt,
        DateTimeOffset committedAt,
        int additions,
        int deletions,
        Guid? developerId = null)
    {
        if (repositoryId == Guid.Empty)
            return Result.Failure<Commit>(new Error("commit.repositoryId.required", "Repository ID is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(sha))
            return Result.Failure<Commit>(new Error("commit.sha.required", "Commit SHA is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(authorLogin))
            return Result.Failure<Commit>(new Error("commit.authorLogin.required", "Author login is required.", ErrorType.Validation));

        if (additions < 0 || deletions < 0)
            return Result.Failure<Commit>(new Error("commit.metrics.invalid", "Additions and deletions cannot be negative.", ErrorType.Validation));

        if (committedAt < authoredAt)
            return Result.Failure<Commit>(new Error("commit.dates.invalid", "Committed date cannot be earlier than authored date.", ErrorType.Validation));

        return Result.Success(new Commit(
            repositoryId,
            sha.Trim(),
            authorLogin.Trim(),
            message.Trim(),
            authoredAt,
            committedAt,
            additions,
            deletions,
            developerId));
    }


}
