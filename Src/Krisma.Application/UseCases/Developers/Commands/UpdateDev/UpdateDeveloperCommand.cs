using Krisma.Domain.Common;
using Krisma.Domain.Enums;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Commands.UpdateDev;

public record UpdateDeveloperCommand(
    Guid Id,
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
