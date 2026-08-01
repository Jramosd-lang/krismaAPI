using FluentValidation;
using Krisma.Api;
using Krisma.Application;
using Krisma.Application.Developers;
using Krisma.Domain.Enums;
using Krisma.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();
app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
    if (exception is ValidationException validationException)
    {
        await validationException.ToValidationProblem(context).ExecuteAsync(context);
        return;
    }
    await Results.Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Unexpected error").ExecuteAsync(context);
}));

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.MapPost("/api/developers", async (CreateDeveloperRequest request, ISender sender, HttpContext context, CancellationToken cancellationToken) =>
{
    var result = await sender.Send(new CreateDeveloperCommand(request.Name, request.LastName, request.GitHubLogin, request.Email, request.Seniority, request.HireDate, request.Position, request.Department), cancellationToken);
    return result.IsSuccess ? Results.Created($"/api/developers/{result.Value.Id}", result.Value) : result.Error.ToProblem(context);
}).WithName("CreateDeveloper");

app.Run();

public sealed record CreateDeveloperRequest(string Name, string LastName, string GitHubLogin, string Email, Seniority Seniority, DateOnly HireDate, Position Position, Department Department);
public partial class Program;
