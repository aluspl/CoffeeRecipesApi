using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Marten;

namespace Api.App.Domain.Map.Handlers;

public class QueryCityHandler
{
    public static async Task<IEnumerable<CityResponse>> HandleAsync(QueryCity command, IDocumentStore store)
    {
        await using var session = store.QuerySession();

        var sessionQuery = command.ProvinceId.HasValue
            ? session
                .Query<City>()
                .Where(x => x.ProvinceId == command.ProvinceId)
            : session
                .Query<City>();

        var sessions = await sessionQuery.ToListAsync();
        return sessions.Select(p => new CityResponse()
        {
            Id = p.Id,
            Name = p.Name,
            ProvinceId = p.ProvinceId,
        });
    }
}