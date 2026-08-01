using Krisma.Domain.Common;
using Krisma.Domain.Enums;
using Krisma.Domain.ValueObjects;

namespace Krisma.Domain.Entities;

public class Developer
{
    private readonly List<TechnologyDeveloper> _technologies = [];

    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Name { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
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

    private Developer(string name, string lastName, string gitHubLogin, Email email, Seniority seniority, DateOnly hireDate, Position position, Department department)
    {
        Name = name;
        LastName = lastName;
        GitHubLogin = gitHubLogin;
        Email = email;
        Seniority = seniority;
        HireDate = hireDate;
        Position = position;
        Department = department;
    }

    public static Result<Developer> Create(string name, string lastName, string gitHubLogin, string email, Seniority seniority, DateOnly hireDate, Position position, Department department)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(gitHubLogin))
            return Result.Failure<Developer>(new Error("developer.required", "Name, last name and GitHub login are required.", ErrorType.Validation));

        if (!Enum.IsDefined(seniority) || !Enum.IsDefined(position) || !Enum.IsDefined(department))
            return Result.Failure<Developer>(new Error("developer.enum.invalid", "Developer enum value is invalid.", ErrorType.Validation));

        if (hireDate > DateOnly.FromDateTime(DateTime.UtcNow))
            return Result.Failure<Developer>(new Error("developer.hire-date.future", "Hire date cannot be in the future.", ErrorType.Validation));

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure<Developer>(emailResult.Error);

        return Result.Success(new Developer(name.Trim(), lastName.Trim(), gitHubLogin.Trim(), emailResult.Value, seniority, hireDate, position, department));
    }
}
