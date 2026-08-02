namespace Krisma.Application.UseCases.Developers.DTOs;

public record DeveloperResponseDto(
    Guid Id,
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
    bool IsActive,
    int Department
);
