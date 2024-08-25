using Api.App.Common.Configs;
using Api.App.Common.Consts;
using Api.App.Common.Extensions;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Roaster.Entities;
using Api.App.Infrastructure.Database.Entities;
using JasperFx.CodeGeneration;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Microsoft.Extensions.Options;
using Weasel.Core;
using Wolverine.Marten;
using CombGuidIdGeneration = Marten.Schema.Identity.CombGuidIdGeneration;

namespace Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection UseDatabase(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString(CommonConsts.ConnectionString);
        var settings = configurationManager.GetConfig<MartenSettings>("Marten");
        services.AddMarten(opts =>
            {
                // Establish the connection string to your Marten database
                opts.Connection(connectionString!);
                
                // Register Schemas
                opts.Schema.For<IEntity>().IdStrategy(new CombGuidIdGeneration());
                opts.UseSystemTextJsonForSerialization(EnumStorage.AsString);

                opts.RegisterDocumentType<City>();
                opts.RegisterDocumentType<Province>();
                opts.RegisterDocumentType<CoffeeRoaster>();

                if (!string.IsNullOrEmpty(settings.SchemaName))
                {
                    opts.Events.DatabaseSchemaName = settings.SchemaName;
                    opts.DatabaseSchemaName = settings.SchemaName;
                }
            })
            .OptimizeArtifactWorkflow()
            .UseLightweightSessions()
            .AddAsyncDaemon(DaemonMode.HotCold)
            .IntegrateWithWolverine();

        return services;
    }
}