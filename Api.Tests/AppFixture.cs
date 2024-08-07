using Alba;
using Api.App.Common.Configs;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using Wolverine;

namespace Api.Tests;

public class AppFixture : IAsyncLifetime
{
    private string SchemaName { get; } = "sch" + Guid.NewGuid().ToString().Replace("-", string.Empty);
    public IAlbaHost Host = null!;
    public AppFixture()
    {
        OaktonEnvironment.AutoStartHost = true;
    }

    public async Task InitializeAsync()
    {
        Host = await AlbaHost.For<Program>(b =>
        {
            b.ConfigureServices((context, services) =>
            {
                services.Configure<MartenSettings>(s =>
                {
                    s.SchemaName = SchemaName;
                });
                services.DisableAllExternalWolverineTransports();
            });
        });
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }
}