using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Krisma.Tests;

public class DeveloperEndpointsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Invalid_request_returns_rfc7807_validation_problem()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("/api/developers", new
        {
            name = "Ada",
            lastName = "Lovelace",
            gitHubLogin = "ada-lovelace",
            email = "invalid",
            seniority = 3,
            hireDate = "2020-01-01",
            position = 1,
            department = 1
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}
