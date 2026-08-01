
using Krisma.Domain.Common;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Commands.CreateDev;

public record CreateDeveloperCommand(
    string Name,
    string LastName,
    string GitHubLogin,
    string Email,
    int Seniority,
    DateOnly HireDate,
    int Position,
    int Department
) : IRequest<Result<Guid>>;
