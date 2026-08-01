using Krisma.Domain.Common;
using Krisma.Domain.Enums;

namespace Krisma.Domain.Entities;

public class Technology
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Name { get; private set; } = null!;
    public TechnologyType Type { get; private set; }
    public string LogoUrl { get; private set; } = null!;
    public string? Description { get; private set; }
    public string DocumentationUrl { get; private set; } = null!;

    private readonly List<TechnologyDeveloper> _developers = [];
    public IReadOnlyCollection<TechnologyDeveloper> Developers => _developers.AsReadOnly();

    private Technology() { }

    private Technology(string name, TechnologyType type, string logoUrl, string? description, string documentationUrl)
    {
        Name = name;
        Type = type;
        LogoUrl = logoUrl;
        Description = description;
        DocumentationUrl = documentationUrl;
    }

    public static Result<Technology> Create(string name, TechnologyType type, string logoUrl, string? description, string documentationUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Technology>(new Error("technology.name.required", "Technology name is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(logoUrl))
            return Result.Failure<Technology>(new Error("technology.logoUrl.required", "Logo URL is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(documentationUrl))
            return Result.Failure<Technology>(new Error("technology.documentationUrl.required", "Documentation URL is required.", ErrorType.Validation));

        return Result.Success(new Technology(name.Trim(), type, logoUrl.Trim(), description?.Trim(), documentationUrl.Trim()));
    }
}