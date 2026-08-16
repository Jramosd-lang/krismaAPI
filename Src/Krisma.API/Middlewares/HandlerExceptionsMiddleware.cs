using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Krisma.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Error no controlado procesando la petición. TraceId: {TraceId} - Mensaje: {Message}",
            traceId,
            exception.Message);

        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "validation.error"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "auth.unauthorized"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "resource.not.found"),
            _ => (StatusCodes.Status500InternalServerError, "server.error")
        };

        httpContext.Response.StatusCode = statusCode;

        ProblemDetails problemDetails = exception switch
        {
            ValidationException validationEx => new ValidationProblemDetails(
                validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = statusCode,
                Title = title,
                Detail = "Se encontraron errores de validación en los datos enviados."
            },
            _ => new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = statusCode == StatusCodes.Status500InternalServerError && !_environment.IsDevelopment()
                    ? "Ocurrió un error interno en el servidor. Por favor, intente más tarde."
                    : exception.Message
            }
        };

        problemDetails.Extensions["traceId"] = traceId;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}