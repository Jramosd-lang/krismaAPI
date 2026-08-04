using Krisma.Domain.Common;
using Krisma.Domain.Enums;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Commands.CreateDev;

public record CreateDeveloperCommand(
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
) : IRequest<Result<Guid>>;
