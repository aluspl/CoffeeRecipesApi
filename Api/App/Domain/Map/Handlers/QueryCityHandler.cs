using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Marten;

namespace Api.App.Domain.Map.Handlers;

public class QueryCityHandler
{
    public static async Task<IEnumerable<CityResponse>> HandleAsync(QueryCity query, IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var sessionQuery = query.ProvinceId.HasValue
            ? session
                .Query<City>()
                .Where(x => x.ProvinceId == query.ProvinceId)
            : session
                .Query<City>();

        var sessions = await sessionQuery.ToListAsync();
        return sessions.Select(CityResponse.FromEntity);
    }
}