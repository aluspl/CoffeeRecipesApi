using Api.App.Infrastructure.Database.Entities;
using Marten;
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

    public static IServiceCollection UseDatabase(this IServiceCollection services,
        string connectionString, int commandTimeout = 30)
    {
        services.AddMarten(options =>
            {
                // Establish the connection string to your Marten database
                options.Connection(connectionString!);

                // Specify that we want to use STJ as our serializer
                options.UseSystemTextJsonForSerialization();

                options.Schema.For<IEntity>().IdStrategy(new CombGuidIdGeneration());
            })
            .IntegrateWithWolverine();

        return services;
    }
}