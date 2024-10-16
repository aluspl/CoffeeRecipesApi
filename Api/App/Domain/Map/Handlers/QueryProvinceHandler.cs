using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Marten;

namespace Api.App.Domain.Map.Handlers;

public class QueryProvinceHandler(IDocumentStore store)
{
    public async Task<IEnumerable<ProvinceResponse>> HandleAsync(QueryProvinceList command)
    {
        await using var session = store.QuerySession();

        var provinces = await session
            .Query<City>()
            .ToListAsync();

        var response = provinces
            .GroupBy(city => city.Province)
            .Select(grouping => new ProvinceResponse()
            {
                Name = grouping.Key,
                Cities = grouping
                    .Select(city => city.Map())
                    .OrderByDescending(city => city.RoastersCount)
                    .ToList(),
            });
        return response;
    }
}