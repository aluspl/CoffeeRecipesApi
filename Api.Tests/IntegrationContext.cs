using Alba;
using Api.Tests.Consts;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public abstract class IntegrationContext : IAsyncLifetime
{
    protected IntegrationContext(AppFixture fixture)
    {
        Host = fixture.Host;
        Store = Host.Services.GetRequiredService<IDocumentStore>();
    }
     
    public IAlbaHost Host { get; }
    public IDocumentStore Store { get; }
     
    public async Task InitializeAsync()
    {
        // Using Marten, wipe out all data and reset the state
        await Store.Advanced.ResetAllData();
    }
 
    // This is required because of the IAsyncLifetime 
    // interface. Note that I do *not* tear down database
    // state after the test. That's purposeful
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    protected async Task SeedProvince()
    {
        await using var documentSession = Store.LightweightSession();
        documentSession.Store(ProvinceConsts.SampleProvince);
        await documentSession.SaveChangesAsync();
    }
}