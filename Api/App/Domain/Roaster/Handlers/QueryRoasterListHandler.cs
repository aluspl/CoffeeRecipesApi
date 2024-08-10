using Api.App.Domain.Roaster.Extensions;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Marten;

namespace Api.App.Domain.Roaster.Handlers;

public class QueryRoasterListHandler
{
    public static async Task<IEnumerable<CoffeeRoasterResponse>> HandleAsync(QueryRoasterList query, IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var sessionQuery = query.CityId.HasValue
            ? session
                .Query<Entities.CoffeeRoaster>()
                .Where(x => x.CityId == query.CityId)
            : session
                .Query<Entities.CoffeeRoaster>();

        var sessions = await sessionQuery.ToListAsync();
        return sessions.Select(o => o.Map());
    }
}