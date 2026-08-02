using FluentValidation;
using Krisma.Domain.Common;
using Krisma.Domain.Entities;
using Krisma.Domain.Enums;
using MediatR;

namespace Krisma.Application.Developers;

public sealed record CreateDeveloperCommand(Guid OrganizationId, string Name, string LastName, long GitHubUserId, string GitHubNodeId, string GitHubLogin, string Email, Seniority Seniority, DateOnly HireDate, Position Position, Department Department) : IRequest<Result<DeveloperResponse>>;
public sealed record DeveloperResponse(Guid Id, Guid OrganizationId, string Name, string LastName, long GitHubUserId, string GitHubNodeId, string GitHubLogin, string Email);

public sealed class CreateDeveloperValidator : AbstractValidator<CreateDeveloperCommand>
{
    public CreateDeveloperValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.GitHubUserId).GreaterThan(0);
        RuleFor(x => x.GitHubNodeId).NotEmpty().MaximumLength(100);
        RuleFor(x => x.GitHubLogin).NotEmpty().MaximumLength(39).Matches("^[A-Za-z0-9](?:[A-Za-z0-9-]{0,37}[A-Za-z0-9])?$");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(254);
        RuleFor(x => x.HireDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.Seniority).IsInEnum();
        RuleFor(x => x.Position).IsInEnum();
        RuleFor(x => x.Department).IsInEnum();
    }
}

public sealed class CreateDeveloperHandler(IDeveloperRepository developers) : IRequestHandler<CreateDeveloperCommand, Result<DeveloperResponse>>
{
    public async Task<Result<DeveloperResponse>> Handle(CreateDeveloperCommand request, CancellationToken cancellationToken)
    {
        if (await developers.ExistsByGitHubUserIdAsync(request.OrganizationId, request.GitHubUserId, cancellationToken))
            return Result.Failure<DeveloperResponse>(new Error("developer.github-user.conflict", "GitHub user already exists in this organization.", ErrorType.Conflict));

        var creation = Developer.Create(request.OrganizationId, request.Name, request.LastName, request.GitHubUserId, request.GitHubNodeId, request.GitHubLogin, request.Email, request.Seniority, request.HireDate, request.Position, request.Department);
        if (creation.IsFailure) return Result.Failure<DeveloperResponse>(creation.Error);

        var developer = creation.Value;
        await developers.AddAsync(developer, cancellationToken);
        return Result.Success(new DeveloperResponse(developer.Id, developer.OrganizationId, developer.Name, developer.LastName, developer.GitHubUserId, developer.GitHubNodeId, developer.GitHubLogin, developer.Email.Value));
    }
}
