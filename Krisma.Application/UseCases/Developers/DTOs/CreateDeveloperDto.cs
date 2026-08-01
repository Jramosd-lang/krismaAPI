using Krisma.Domain.Enums;

namespace Krisma.Application.UseCases.Developers.DTOs;

public sealed record CreateDeveloperDto(
    string Name,
    string LastName,
    string GitHubLogin,
    string Email,
    int Seniority,
    DateOnly HireDate,
    int Position,
    int Department
);