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
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Information("Starting up");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.ApplyOaktonExtensions();
    builder.Services.AddHostedService<DatabaseInitializer>();
// Add Wolverine to project
    builder.Host.UseWolverine();
    builder.Host.UseResourceSetupOnStartup();

    builder.Services.AddSerilog();
    builder.Services.UseSwagger();
    builder.Services.UseJson();
    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();
    builder.Services.UseDatabase(builder.Configuration);

    var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseRouting();
    app.MapControllers();

// Setup Modules
    app.SetupMapModule();

    app.MapHealthChecks("/healthz");

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