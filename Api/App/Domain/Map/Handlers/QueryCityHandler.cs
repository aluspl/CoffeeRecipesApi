using Api.App.Common.Extensions;
using Api.App.Domain.Map.Entities;
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

        var sessionQuery = SessionQuery(query, session);

        var sessions = await sessionQuery.ToListAsync();
        return sessions.Select(MapExtensions.Map);
    }

    private static IQueryable<City> SessionQuery(QueryCityList query, IQuerySession session)
    {
        IQueryable<City> queryable = session.Query<City>();
        if (query.Name.IsNullOrEmpty())
        {
            queryable = session
                .Query<Entities.City>()
                .Where(x => x.Name.Contains(query.Name));
        }

        if (query.ProvinceId.HasValue)
        {
            queryable = session
                .Query<Entities.City>()
                .Where(x => x.ProvinceId == query.ProvinceId);
        }
        
        return queryable;
    }
}