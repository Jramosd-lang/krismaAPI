using Krisma.Domain.Common;
using Krisma.Domain.Enums;
using Krisma.Domain.ValueObjects;

namespace Krisma.Domain.Entities;

public class Developer
{
    private readonly List<TechnologyDeveloper> _technologies = [];

    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid OrganizationId { get; private set; }
    public Organization? Organization { get; private set; }
    public string Name { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public long GitHubUserId { get; private set; }
    public string GitHubNodeId { get; private set; } = null!;
    public string GitHubLogin { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Seniority Seniority { get; private set; }
    public DateOnly HireDate { get; private set; }
    public Position Position { get; private set; }
    public bool IsActive { get; private set; } = true;
    public Department Department { get; private set; }
    public Guid? ManagerId { get; private set; }
    public IReadOnlyCollection<TechnologyDeveloper> Technologies => _technologies.AsReadOnly();

    private Developer() { }

    private Developer(Guid organizationId, string name, string lastName, long gitHubUserId, string gitHubNodeId, string gitHubLogin, Email email, Seniority seniority, DateOnly hireDate, Position position, Department department)
    {
        OrganizationId = organizationId;
        Name = name;
        LastName = lastName;
        GitHubUserId = gitHubUserId;
        GitHubNodeId = gitHubNodeId;
        GitHubLogin = gitHubLogin;
        Email = email;
        Seniority = seniority;
        HireDate = hireDate;
        Position = position;
        Department = department;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public static Result<Developer> Create(Guid organizationId, string name, string lastName, long gitHubUserId, string gitHubNodeId, string gitHubLogin, string email, Seniority seniority, DateOnly hireDate, Position position, Department department)
    {
        if (organizationId == Guid.Empty)
            return Result.Failure<Developer>(new Error("developer.organizationId.required", "Organization ID is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(gitHubLogin))
            return Result.Failure<Developer>(new Error("developer.required", "Name, last name and GitHub login are required.", ErrorType.Validation));

        if (gitHubUserId <= 0)
            return Result.Failure<Developer>(new Error("developer.githubUserId.invalid", "GitHub user ID must be greater than zero.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(gitHubNodeId))
            return Result.Failure<Developer>(new Error("developer.githubNodeId.required", "GitHub node ID is required.", ErrorType.Validation));

        if (!Enum.IsDefined(seniority) || !Enum.IsDefined(position) || !Enum.IsDefined(department))
            return Result.Failure<Developer>(new Error("developer.enum.invalid", "Developer enum value is invalid.", ErrorType.Validation));

        if (hireDate > DateOnly.FromDateTime(DateTime.UtcNow))
            return Result.Failure<Developer>(new Error("developer.hire-date.future", "Hire date cannot be in the future.", ErrorType.Validation));

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure<Developer>(emailResult.Error);

        return Result.Success(new Developer(organizationId, name.Trim(), lastName.Trim(), gitHubUserId, gitHubNodeId.Trim(), gitHubLogin.Trim(), emailResult.Value, seniority, hireDate, position, department));
    }

    public Result<Developer> Update(string name, string lastName, long gitHubUserId, string gitHubNodeId, string gitHubLogin, string email, Seniority seniority, DateOnly hireDate, Position position, Department department)
    {
        if (OrganizationId == Guid.Empty)
            return Result.Failure<Developer>(new Error("developer.organizationId.invalid", "Organization ID cannot be empty on update.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(gitHubLogin))
            return Result.Failure<Developer>(new Error("developer.required", "Name, last name and GitHub login are required.", ErrorType.Validation));

        if (gitHubUserId <= 0)
            return Result.Failure<Developer>(new Error("developer.githubUserId.invalid", "GitHub user ID must be greater than zero.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(gitHubNodeId))
            return Result.Failure<Developer>(new Error("developer.githubNodeId.required", "GitHub node ID is required.", ErrorType.Validation));

        if (!Enum.IsDefined(seniority) || !Enum.IsDefined(position) || !Enum.IsDefined(department))
            return Result.Failure<Developer>(new Error("developer.enum.invalid", "Developer enum value is invalid.", ErrorType.Validation));

        if (hireDate > DateOnly.FromDateTime(DateTime.UtcNow))
            return Result.Failure<Developer>(new Error("developer.hire-date.future", "Hire date cannot be in the future.", ErrorType.Validation));

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure<Developer>(emailResult.Error);

        Name = name.Trim();
        LastName = lastName.Trim();
        GitHubUserId = gitHubUserId;
        GitHubNodeId = gitHubNodeId.Trim();
        GitHubLogin = gitHubLogin.Trim();
        Email = emailResult.Value;
        Seniority = seniority;
        HireDate = hireDate;
        Position = position;
        Department = department;

        return Result.Success(this);
    }
}
