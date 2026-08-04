using FluentValidation;
using Krisma.Domain.Enums;

namespace Krisma.Application.UseCases.Developers.Commands.CreateDev;

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
