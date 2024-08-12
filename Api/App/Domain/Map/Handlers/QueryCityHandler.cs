using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Marten;
using MapExtensions = Api.App.Domain.Map.Models.Responses.MapExtensions;

namespace Api.App.Domain.Map.Handlers;

public class QueryCityHandler
{
    public static async Task<IEnumerable<CityResponse>> HandleAsync(QueryCityList query, IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var sessionQuery = query.ProvinceId.HasValue
            ? session
                .Query<Entities.City>()
                .Where(x => x.ProvinceId == query.ProvinceId)
            : session
                .Query<Entities.City>();

        var sessions = await sessionQuery.ToListAsync();
        return sessions.Select(MapExtensions.Map);
    }
}