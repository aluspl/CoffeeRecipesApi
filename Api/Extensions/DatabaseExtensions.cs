using Api.App.Common.Configs;
using Api.App.Common.Consts;
using Api.App.Infrastructure.Database.Entities;
using Marten;
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

    public static IServiceCollection UseDatabase(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString(CommonConsts.ConnectionString);
        var settings = configurationManager.Get<MartenSettings>();

        services.AddMarten(options =>
            {
                // Establish the connection string to your Marten database
                options.Connection(connectionString!);

                // Specify that we want to use STJ as our serializer
                options.UseSystemTextJsonForSerialization();

                options.Schema.For<IEntity>().IdStrategy(new CombGuidIdGeneration());
                if (!string.IsNullOrEmpty(settings.SchemaName))
                {
                    options.Events.DatabaseSchemaName = settings.SchemaName;
                    options.DatabaseSchemaName = settings.SchemaName;
                }
            })
            .UseLightweightSessions()
            .IntegrateWithWolverine();

        return services;
    }
}