using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Marten;

namespace Api.App.Domain.Map.Handlers;

public class QueryProvinceHandler
{
    public static async Task<IEnumerable<ProvinceResponse>> HandleAsync(QueryProvince command, IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var provinces =await session
            .Query<Province>().
            ToListAsync();

        var cities =await session
            .Query<City>().
            ToListAsync();
        var mappedCity = cities.Select(CityResponse.FromEntity);
        
        return provinces.Select(p => new ProvinceResponse()
        {
            Id = p.Id,
            Name = p.Name,
            Cities = mappedCity
                .Where(o => o.ProvinceId == p.Id)
                .ToList(),
        });
    }
}