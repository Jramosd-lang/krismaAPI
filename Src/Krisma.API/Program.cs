using Krisma.API.Middlewares;
using Krisma.Application;
using Krisma.Infraestructure;
using Scalar.AspNetCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseHandlerExceptionsMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options.WithOpenApiRoutePattern("/openapi/{documentName}.json"));

    var url = app.Urls.FirstOrDefault() ?? "http://localhost:5223";
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = $"{url}/scalar/v1",
            UseShellExecute = true
        });
    }
    catch (Exception)
    {
        // Ignorar si el entorno no permite abrir interfaces gráficas
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program;
