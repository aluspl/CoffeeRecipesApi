using Api.App.Domain.Coffees.Entities;
using Api.App.Domain.Coffees.Extensions;
using Api.App.Domain.Coffees.Handlers.Queries;
using Api.App.Domain.Coffees.Models;
using Marten;

namespace Api.App.Domain.Coffees.Handlers;

public class QueryCoffeeDetailHandler
{
    public static async Task<CoffeeResponse> HandleAsync(
        QueryCoffeeDetail query, 
        IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var entity = await session
            .Query<Coffee>()
            .Where(x => x.Id == query.CoffeeId)
            .FirstOrDefaultAsync();

        return entity.Map();
    }
}