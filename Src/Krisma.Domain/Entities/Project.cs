using Krisma.Domain.Common;

namespace Krisma.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid OrganizationId { get; private set; }
    public Organization? Organization { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Propiedad de navegación opcional hacia los repositorios que componen este proyecto

    private readonly List<Repository> _repositories = [];
    public IReadOnlyCollection<Repository> Repositories => _repositories.AsReadOnly();

    private Project() { }

    private Project(Guid organizationId, string name, string? description)
    {
        OrganizationId = organizationId;
        Name = name;
        Description = description;
    }

    public static Result<Project> Create(Guid organizationId, string name, string? description = null)
    {
        if (organizationId == Guid.Empty)
            return Result.Failure<Project>(new Error("project.organizationId.required", "Organization ID is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Project>(new Error("project.name.required", "Project name is required.", ErrorType.Validation));

        return Result.Success(new Project(organizationId, name.Trim(), description?.Trim()));
    }
}