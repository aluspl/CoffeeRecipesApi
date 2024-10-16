using Alba;
using Api.App.Common.Enums;
using Api.App.Domain.Coffees.Entities;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Roaster.Entities;
using Api.App.Infrastructure.Database.Entities;
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
    protected const string ProvinceName = "Slask";

    protected async Task<City> SeedCity(string provinceName = ProvinceConsts.ProvinceName)
    {
        await using var session = Store.LightweightSession();
        var entity = new City()
        {
            Country = CountryCodes.PL,
            Province = provinceName,
            Name = ProvinceConsts.SampleCity.Name,
        };
        session.Store(entity);
        await session.SaveChangesAsync();
        return entity;
    }
    
    protected async Task<CoffeeRoaster> SeedRoaster(Guid cityId, string name = "Roaster")
    {
        await using var session = Store.LightweightSession();
        var entity = new CoffeeRoaster()
        {
            CityId = cityId,
            Name = name,
            Urls = new List<UrlDetail>()
            {
                new UrlDetail("https://2137.it", "Main Page"),
                new UrlDetail("https://vatican.it", "Common Page")
            }
        };
        session.Store(entity);
        await session.SaveChangesAsync();
        return entity;
    }
    
    protected async Task<Coffee> SeedCoffee(Guid roasterId, string name = "Coffee")
    {
        await using var session = Store.LightweightSession();
        var entity = new Coffee()
        {
            RoasterId = roasterId,
            Name = name,
            Urls = new List<UrlDetail>()
            {
                new UrlDetail("https://2137.it", "Main Page"),
                new UrlDetail("https://vatican.it", "Common Page")
            }
        };
        session.Store(entity);
        await session.SaveChangesAsync();
        return entity;
    }
}