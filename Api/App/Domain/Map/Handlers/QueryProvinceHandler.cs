using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Api.App.Domain.Roaster.Entities;
using JasperFx.Core;
using Marten;

namespace Api.App.Domain.Map.Handlers;

public class QueryProvinceHandler(IDocumentStore store)
{
    public async Task<IEnumerable<ProvinceResponse>> HandleAsync(QueryProvinceList command)
    {
        await using var session = store.QuerySession();

        var provinces = await session
            .Query<Province>()
            .ToListAsync();

        var mappedCities = await GetCities();
        return provinces.Select(p => p.Map(GetCities(mappedCities, p.Id)));
    }

    private async Task<IEnumerable<CityResponse>> GetCities()
    {
        await using var session = store.QuerySession();

        var cities = await session
            .Query<City>().ToListAsync();

        var cityResponses = cities
            .Select(p => p.Map())
            .ToList();
        foreach (var cityResponse in cityResponses)
        {
            cityResponse.RoastersCount = await GetRoastersCount(cityResponse.Id);
        }

        return cityResponses;
    }

    private async Task<int> GetRoastersCount(Guid cityId)
    {
        await using var session = store.QuerySession();

        return await session
            .Query<CoffeeRoaster>()
            .Where(p => p.CityId == cityId)
            .CountAsync();
    }

    private static ICollection<CityResponse> GetCities(IEnumerable<CityResponse> mappedCity, Guid provinceId)
    {
        return mappedCity
            .Where(o => o.ProvinceId == provinceId)
            .ToList();
    }
}