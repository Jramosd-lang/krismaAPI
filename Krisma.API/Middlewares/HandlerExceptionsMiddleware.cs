using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Krisma.API.Middlewares;

public class HandlerExceptionsMiddleware
{
    private readonly RequestDelegate _next;

    public HandlerExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        var title = "server.error";

        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                title = "auth.unauthorized";
                break;
            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                title = "resource.not.found";
                break;
        }

        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        };

        var json = JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(json);
    }
}

public static class UseHandlerExceptionsMiddlewareExtensions
{
    public static IApplicationBuilder UseHandlerExceptionsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HandlerExceptionsMiddleware>();
    }
}