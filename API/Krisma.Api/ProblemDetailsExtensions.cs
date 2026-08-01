using FluentValidation;
using Krisma.Domain.Common;

namespace Krisma.Api;

public static class ProblemDetailsExtensions
{
    public static IResult ToProblem(this Error error, HttpContext context) => Results.Problem(
        statusCode: error.Type switch { ErrorType.Validation => 400, ErrorType.NotFound => 404, ErrorType.Conflict => 409, _ => 500 },
        title: error.Type.ToString(),
        type: $"https://httpstatuses.com/{error.Type switch { ErrorType.Validation => 400, ErrorType.NotFound => 404, ErrorType.Conflict => 409, _ => 500 }}",
        detail: error.Message,
        instance: context.Request.Path,
        extensions: new Dictionary<string, object?> { ["code"] = error.Code });

    public static IResult ToValidationProblem(this ValidationException exception, HttpContext context)
    {
        var errors = exception.Errors.GroupBy(x => x.PropertyName).ToDictionary(group => group.Key, group => group.Select(x => x.ErrorMessage).Distinct().ToArray());
        return Results.ValidationProblem(errors, detail: "One or more validation errors occurred.", instance: context.Request.Path);
    }
}
