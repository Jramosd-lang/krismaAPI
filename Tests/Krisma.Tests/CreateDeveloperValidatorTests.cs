using Krisma.Application.Developers;
using Krisma.Domain.Enums;
namespace Krisma.Tests;
public class CreateDeveloperValidatorTests
{
    [Fact]
    public async Task Invalid_email_fails_validation()
    {
        var result = await new CreateDeveloperValidator().ValidateAsync(new CreateDeveloperCommand("Ada", "Lovelace", "ada-lovelace", "invalid", Seniority.Senior, new DateOnly(2020, 1, 1), Position.Backend, Department.Engineering));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Email");
    }
}
