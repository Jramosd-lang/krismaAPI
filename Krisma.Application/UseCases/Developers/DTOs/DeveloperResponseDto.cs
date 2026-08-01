namespace Krisma.Application.UseCases.Developers.DTOs;

public record DeveloperResponseDto(
    Guid Id,
    string Name,
    string LastName,
    string GitHubLogin,
    string Email,
    int Seniority,
    DateOnly HireDate,
    int Position,
    bool IsActive,
    int Department
);