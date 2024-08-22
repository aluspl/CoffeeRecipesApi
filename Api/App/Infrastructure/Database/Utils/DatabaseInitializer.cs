using Api.App.Common.Consts;
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
        var count = await session.Query<Province>().CountAsync();
        if (count != 0)
        {
            return;
        }

        foreach (var province in ProvincesConsts.Provinces)
        {
            var provinceEntity = new Province()
            {
                Name = province.Key,
                Created = DateTime.UtcNow,
            };

            session.Store(provinceEntity);

            foreach (var city in province.Value.Select(cityName => new City()
                         { Name = cityName, ProvinceId = provinceEntity.Id, Created = DateTime.UtcNow }))
            {
                session.Store(city);
            }
        }

        await session.SaveChangesAsync();
    }
}