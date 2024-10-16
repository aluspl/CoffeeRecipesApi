using Api.App.Common.Consts;
using Api.App.Common.Enums;
using Api.App.Domain.Map.Entities;
using Marten;

namespace Api.App.Infrastructure.Database.Utils;

public class DatabaseInitializer(IDocumentStore store) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await PopulateProvincesAndCities();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task PopulateProvincesAndCities()
    {
        await using var session = store.LightweightSession();
        var count = await session.Query<City>().CountAsync();
        if (count != 0)
        {
            return;
        }

        foreach (var city in ProvincesConsts.Provinces.SelectMany(province => province.Value.Select(cityName => new City()
                 {
                     Name = cityName, 
                     Province = province.Key,
                     Country = CountryCodes.PL,
                     Created = DateTime.UtcNow
                 })))
        {
            session.Store(city);
        }

        await session.SaveChangesAsync();
    }
}