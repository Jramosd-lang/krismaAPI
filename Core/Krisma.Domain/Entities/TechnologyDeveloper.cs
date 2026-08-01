using Krisma.Domain.Common;

namespace Krisma.Domain.Entities;

public class TechnologyDeveloper
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid TechnologyId { get; private set; }
    public Technology? Technology { get; private set; }
    public Guid DeveloperId { get; private set; }
    public Developer? Developer { get; private set; }
    public int ExperienceYears { get; private set; }
    public string MainVersion { get; private set; } = null!;

    private TechnologyDeveloper() { }

    private TechnologyDeveloper(Guid technologyId, Guid developerId, int experienceYears, string mainVersion)
    {
        TechnologyId = technologyId;
        DeveloperId = developerId;
        ExperienceYears = experienceYears;
        MainVersion = mainVersion;
    }

    public static Result<TechnologyDeveloper> Create(Guid technologyId, Guid developerId, int experienceYears, string mainVersion)
    {
        if (technologyId == Guid.Empty)
            return Result.Failure<TechnologyDeveloper>(new Error("technologyDeveloper.technologyId.required", "Technology ID is required.", ErrorType.Validation));

        if (developerId == Guid.Empty)
            return Result.Failure<TechnologyDeveloper>(new Error("technologyDeveloper.developerId.required", "Developer ID is required.", ErrorType.Validation));

        if (experienceYears < 0)
            return Result.Failure<TechnologyDeveloper>(new Error("technologyDeveloper.experienceYears.invalid", "Experience years cannot be negative.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(mainVersion))
            return Result.Failure<TechnologyDeveloper>(new Error("technologyDeveloper.mainVersion.required", "Main version is required.", ErrorType.Validation));

        return Result.Success(new TechnologyDeveloper(technologyId, developerId, experienceYears, mainVersion.Trim()));
    }
}