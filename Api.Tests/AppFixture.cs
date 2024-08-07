using Alba;
using Api.App.Common.Configs;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using Wolverine;

namespace Api.Tests;

public class AppFixture : IAsyncLifetime
{
    private string MartenSchemaNameValue { get; } = "sch" + Guid.NewGuid().ToString().Replace("-", string.Empty);
    private const string MartenSchemaName = "Marten:SchemaName";
    public IAlbaHost Host = null!;

    public AppFixture()
    {
        OaktonEnvironment.AutoStartHost = true;
    }

    public async Task InitializeAsync()
    {
        Host = await AlbaHost.For<Program>(b =>
        {
            Environment.SetEnvironmentVariable(MartenSchemaName, MartenSchemaNameValue);

            // b.ConfigureAppConfiguration((context, configurationBuilder) =>
            // {
            //     configurationBuilder.AddInMemoryCollection(AddConfiguration());
            // });
            b.ConfigureTestServices(collection => collection.Configure<MartenSettings>(o =>
            {
                o.SchemaName = MartenSchemaNameValue;
            }));
            b.ConfigureServices((context, services) => { services.DisableAllExternalWolverineTransports(); });
        });
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }

    private IEnumerable<KeyValuePair<string, string?>> AddConfiguration()
    {
        return new List<KeyValuePair<string, string?>>()
        {
            new (MartenSchemaName, MartenSchemaNameValue),
        };
    }
}