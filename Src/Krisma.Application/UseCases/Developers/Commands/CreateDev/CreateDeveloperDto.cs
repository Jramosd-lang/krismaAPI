using Krisma.Domain.Enums;

namespace Krisma.Application.UseCases.Developers.Commands.CreateDev;

public sealed record CreateDeveloperDto(
    Guid OrganizationId,
    string Name,
    string LastName,
    long GitHubUserId,
    string GitHubNodeId,
    string GitHubLogin,
    string Email,
    Seniority Seniority,
    DateOnly HireDate,
    Position Position,
    Department Department
);
