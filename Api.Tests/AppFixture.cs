using Alba;
using Api.App.Common.Configs;
using Api.App.Domain.Media.Interfaces.Provider;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using Wolverine;

namespace Api.Tests;

public class AppFixture : IAsyncLifetime
{
    private string MartenSchemaNameValue { get; } = "sch" + Guid.NewGuid().ToString().Replace("-", string.Empty);
    private const string MartenSchemaName = "Marten:SchemaName";
    private const string MartenUseStatic = "Marten:UseStatic";
    
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
            Environment.SetEnvironmentVariable(MartenUseStatic, "false");

            b.ConfigureTestServices(collection => collection.Configure<MartenSettings>(o =>
            {
                o.SchemaName = MartenSchemaNameValue;
            }));
            b.ConfigureServices((context, services) =>
            {
                AddMockServices(services);
                services.DisableAllExternalWolverineTransports();
            });
        });
    }

    private void AddMockServices(IServiceCollection services)
    {
        var blobProvider = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IBlobProvider));
        if (blobProvider != null)
        {
            services.Remove(blobProvider);
        }
        
        services.AddScoped<IBlobProvider, MockProviders.MockBlobProviders>();
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }
}