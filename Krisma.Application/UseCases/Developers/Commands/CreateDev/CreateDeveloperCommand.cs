
using Krisma.Domain.Common;
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
    int Seniority,
    DateOnly HireDate,
    int Position,
    int Department
) : IRequest<Result<Guid>>;
