using Api.App.Common.Extensions;
using Api.App.Domain.Coffees.Entities;
using Api.App.Domain.Coffees.Extensions;
using Api.App.Domain.Coffees.Handlers.Queries;
using Api.App.Domain.Coffees.Models;
using Marten;

namespace Api.App.Domain.Coffees.Handlers;

public class QueryCoffeeListHandler
{
    public static async Task<IEnumerable<CoffeeResponse>> HandleAsync(
        QueryCoffeeList query,
        IDocumentSession session)
    {
        var sessionQuery = SessionQuery(query, session);

        var sessions = await sessionQuery.ToListAsync();

        return sessions.Select(roaster => roaster.Map()).ToList();
    }

    private static IQueryable<Coffee> SessionQuery(QueryCoffeeList query, IQuerySession session)
    {
        IQueryable<Coffee> queryable = session
            .Query<Coffee>();

        if (query.RoasterId.HasValue)
        {
            queryable = session
                .Query<Coffee>()
                .Where(x => x.RoasterId == query.RoasterId);
        }

        if (!query.Name.IsNullOrEmpty())
        {
            queryable = session
                .Query<Coffee>()
                .Where(x => x.Name.Contains(query.Name));
        }

        return queryable;
    }
}