using Api.App.Domain.Roaster.Extensions;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Marten;

namespace Api.App.Domain.Roaster.Handlers;

public class QueryRoasterDetailHandler
{
    public static async Task<CoffeeRoasterResponse> HandleAsync(
        QueryRoasterDetail query, 
        IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var entity = await session
            .Query<Entities.CoffeeRoaster>()
            .Where(x => x.Id == query.RoasterId)
            .FirstOrDefaultAsync();

        return entity.Map();
    }
}