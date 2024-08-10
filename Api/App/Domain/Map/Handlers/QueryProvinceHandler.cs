using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Marten;
using MapExtensions = Api.App.Domain.Map.Models.Responses.MapExtensions;

namespace Api.App.Domain.Map.Handlers;

public class QueryProvinceHandler
{
    public static async Task<IEnumerable<ProvinceResponse>> HandleAsync(QueryProvinceList command, IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var provinces =await session
            .Query<Province>().
            ToListAsync();

        var cities =await session
            .Query<City>().
            ToListAsync();
        
        var mappedCity = cities.Select(p => p.Map());
        return provinces.Select(p => p.Map(GetCities(mappedCity, p.Id)));
    }

    private static ICollection<CityResponse> GetCities(IEnumerable<CityResponse> mappedCity, Guid provinceId)
    {
       return mappedCity
            .Where(o => o.ProvinceId == provinceId)
            .ToList();
    }
}