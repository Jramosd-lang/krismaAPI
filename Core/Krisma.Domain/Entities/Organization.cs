using Krisma.Domain.Common;

namespace Krisma.Domain.Entities;

public class Organization
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public long GitHubOrganizationId { get; private set; }
    public string GitHubNodeId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string TimeZoneId { get; private set; } = "UTC";

    // Propiedades de navegación opcionales pero recomendadas para el dominio

    private readonly List<Project> _projects = [];
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

    private readonly List<Team> _teams = [];
    public IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();

    private Organization() { }

    private Organization(long gitHubOrganizationId, string gitHubNodeId, string name, string timeZoneId)
    {
        GitHubOrganizationId = gitHubOrganizationId;
        GitHubNodeId = gitHubNodeId;
        Name = name;
        TimeZoneId = timeZoneId;
    }

    public static Result<Organization> Create(long gitHubOrganizationId, string gitHubNodeId, string name, string timeZoneId = "UTC")
    {
        if (gitHubOrganizationId <= 0)
            return Result.Failure<Organization>(new Error("organization.githubId.invalid", "GitHub Organization ID must be greater than zero.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(gitHubNodeId))
            return Result.Failure<Organization>(new Error("organization.nodeId.required", "GitHub Node ID is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Organization>(new Error("organization.name.required", "Organization name is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(timeZoneId))
            timeZoneId = "UTC";

        return Result.Success(new Organization(gitHubOrganizationId, gitHubNodeId.Trim(), name.Trim(), timeZoneId.Trim()));
    }
}