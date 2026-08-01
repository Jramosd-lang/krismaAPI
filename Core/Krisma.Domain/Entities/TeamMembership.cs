using Krisma.Domain.Common;
using Krisma.Domain.Enums;

namespace Krisma.Domain.Entities;

public class TeamMembership
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid TeamId { get; private set; }
    public Team? Team { get; private set; }
    public Guid DeveloperId { get; private set; }
    public Developer? Developer { get; private set; }
    public TeamRole Role { get; private set; }
    public DateOnly StartsOn { get; private set; }
    public DateOnly? EndsOn { get; private set; }

    private TeamMembership() { }

    private TeamMembership(Guid teamId, Guid developerId, TeamRole role, DateOnly startsOn, DateOnly? endsOn)
    {
        TeamId = teamId;
        DeveloperId = developerId;
        Role = role;
        StartsOn = startsOn;
        EndsOn = endsOn;
    }

    public static Result<TeamMembership> Create(Guid teamId, Guid developerId, TeamRole role, DateOnly startsOn, DateOnly? endsOn = null)
    {
        if (teamId == Guid.Empty)
            return Result.Failure<TeamMembership>(new Error("teamMembership.teamId.required", "Team ID is required.", ErrorType.Validation));

        if (developerId == Guid.Empty)
            return Result.Failure<TeamMembership>(new Error("teamMembership.developerId.required", "Developer ID is required.", ErrorType.Validation));

        if (endsOn.HasValue && endsOn.Value < startsOn)
            return Result.Failure<TeamMembership>(new Error("teamMembership.dates.invalid", "End date cannot be earlier than start date.", ErrorType.Validation));

        return Result.Success(new TeamMembership(teamId, developerId, role, startsOn, endsOn));
    }
}