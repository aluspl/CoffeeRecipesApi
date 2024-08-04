using Api.App.Common.Consts;
using Api.App.Domain.Map.Module;
using Api.App.Infrastructure.Database.Utils;
using Api.Extensions;
using Oakton;
using Oakton.Resources;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ApplyOaktonExtensions();
builder.Services.AddHostedService<DatabaseInitializer>();

var connectionString = builder.Configuration.GetConnectionString(CommonConsts.ConnectionString);

// Add Wolverine to project
builder.Host.UseWolverine();
builder.Host.UseResourceSetupOnStartup();

// Database.Infrastructure
builder.Services.UseSwagger();
builder.Services.UseJson();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.UseDatabase(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

// Setup Modules
app.SetupMapModule();

app.MapHealthChecks("/healthz");

await app.RunOaktonCommands(args);