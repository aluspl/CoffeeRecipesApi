#region Using(s)

#endregion

using Marten;
using Weasel.Core;

namespace Api.Extensions;

public static class DiExtensions
{
    public static IServiceCollection UseDatabase(this IServiceCollection services,
        string connectionString, int commandTimeout = 30)
    {
        services.AddMarten(options =>
        {
            // Establish the connection string to your Marten database
            options.Connection(connectionString!);

            // Specify that we want to use STJ as our serializer
            options.UseSystemTextJsonForSerialization();

            options.AutoCreateSchemaObjects = AutoCreate.All;
        });

        return services;
    }
}