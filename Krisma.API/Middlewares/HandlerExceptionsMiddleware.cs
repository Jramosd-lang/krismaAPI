using System.Net;
using System.Text.Json;
using Krisma.Domain.Common;

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
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var error = new Error("server.error", exception.Message, ErrorType.Failure);

        switch (exception)
        {
            case UnauthorizedAccessException:
                httpStatusCode = HttpStatusCode.Unauthorized;
                error = new Error("auth.unauthorized", exception.Message, ErrorType.Validation);
                break;
            case KeyNotFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                error = new Error("resource.not.found", exception.Message, ErrorType.NotFound);
                break;
        }

        context.Response.StatusCode = (int)httpStatusCode;

        var response = Result.Failure<object>(error);
        var json = JsonSerializer.Serialize(response);

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