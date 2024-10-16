using Api.App.Common.Extensions;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Marten;
using Wolverine.Attributes;
using MapExtensions = Api.App.Domain.Map.Models.Responses.MapExtensions;

namespace Api.App.Domain.Map.Handlers;

[WolverineHandler]
public static class QueryCityHandler
{
    public static async Task<IEnumerable<CityResponse>> HandleAsync(QueryCityListByProvinceName query,
        IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var sessionQuery = session
            .Query<City>()
            .Where(x => x.Province.Contains(query.Province));

        var sessions = await sessionQuery.ToListAsync();
        return sessions.Select(MapExtensions.Map);
    }

    public static async Task<IEnumerable<CityResponse>> HandleAsync(QueryCityListByName query, IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var sessionQuery = query.Name.IsNullOrEmpty()
            ? session.Query<City>()
            : session.Query<City>().Where(x => x.Name.Contains(query.Name));

        var sessions = await sessionQuery.ToListAsync();
        return sessions.Select(MapExtensions.Map);
    }
}