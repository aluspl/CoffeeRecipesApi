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

// Database.Infrastructure
builder.Services.UseSwagger();
builder.Services.UseJson();

// Add Wolverine to project
builder.Host.UseWolverine(connectionString);
builder.Host.UseResourceSetupOnStartup();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
// Setup Modules
app.SetupMapModule();
await app.RunOaktonCommands(args);