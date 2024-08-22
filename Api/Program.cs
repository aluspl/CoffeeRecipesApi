using Api.App.Common.Exceptions;
using Api.App.Common.Middleware;
using Api.App.Domain.Map.Module;
using Api.App.Infrastructure.Database.Utils;
using Api.Extensions;
using Oakton;
using Oakton.Resources;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)
    .MinimumLevel.Override("Npgsql", LogEventLevel.Error)
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(TelemetryConverter.Traces)
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Information("Starting up");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.ApplyOaktonExtensions();
    builder.Services.AddHostedService<DatabaseInitializer>();
// Add Wolverine to project
    builder.Host.UseWolverine(builder.Environment.IsProduction());
    builder.Host.UseResourceSetupOnStartup();

    builder.Services.AddSerilog();
    builder.Services.UseSwagger();
    builder.Services.AddControllers().AddNewtonsoftJson();
    builder.Services.UseJson();
    builder.Services.AddHealthChecks();
    builder.Services.AddCors();
    builder.Services.UseDatabase(builder.Configuration);
    builder.Services.AddApplicationInsightsTelemetry();
    builder.Services.AddDefaultExceptionHandler(
        (exception, _) => exception switch
        {
            NotFoundException => exception.MapToProblemDetails(StatusCodes.Status404NotFound),
            BusinessException => exception.MapToProblemDetails(StatusCodes.Status400BadRequest),
            _ => null
        });
    var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseExceptionHandler();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin());
    app.UseRouting();
    app.MapControllers();

// Setup Modules
    app.SetupMapModule();

    app.MapHealthChecks("/healthz");
    app.MapGet("/", () => "OK");

    await app.RunOaktonCommands(args);
}
catch (Exception e)
{
    Log.Fatal(e, "Unhandled Exception");
}
finally
{
    await Log.CloseAndFlushAsync();
}