using Krisma.Domain.Common;

namespace Krisma.Domain.Entities;

public class Team
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid OrganizationId { get; private set; }
    public Organization? Organization { get; private set; }
    public string Name { get; private set; } = null!;

    private readonly List<TeamMembership> _memberships = [];
    public IReadOnlyCollection<TeamMembership> Memberships => _memberships.AsReadOnly();

    private Team() { }

    private Team(Guid organizationId, string name)
    {
        OrganizationId = organizationId;
        Name = name;
    }

    public static Result<Team> Create(Guid organizationId, string name)
    {
        if (organizationId == Guid.Empty)
            return Result.Failure<Team>(new Error("team.organizationId.required", "Organization ID is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Team>(new Error("team.name.required", "Team name is required.", ErrorType.Validation));

        return Result.Success(new Team(organizationId, name.Trim()));
    }
}