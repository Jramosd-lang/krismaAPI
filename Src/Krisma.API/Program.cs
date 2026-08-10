using Krisma.API.Middlewares;
using Krisma.Application;
using Krisma.Infraestructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Aqui se añaden los servicios de la aplicación, infraestructura y controladores al contenedor de inyección de dependencias

builder.Services.AddApplication()
                .AddInfrastructure()
                .AddControllers();

// OpenAPI nativo moderno

builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Middleware global de manejo de excepciones al inicio del pipeline
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options.WithOpenApiRoutePattern("/openapi/{documentName}.json"));

    // NOTA: Se removió Process.Start por buenas prácticas en contenedores/Linux.
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;