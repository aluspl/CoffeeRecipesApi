using Api.App.Common.Extensions;
using Api.App.Domain.Media.Getter;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Extensions;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Marten;

namespace Api.App.Domain.Roaster.Handlers;

public class QueryRoasterListHandler
{
    public static async Task<IEnumerable<CoffeeRoasterResponse>> HandleAsync(
        QueryRoasterList query,
        IDocumentSession session,
        IFileGetter fileGetter)
    {
        var sessionQuery = SessionQuery(query, session);

        var sessions = await sessionQuery.ToListAsync();

        var responses = new List<CoffeeRoasterResponse>();
        foreach (var roaster in sessions)
        {
            var response = roaster.Map();
            response.Cover = await fileGetter.GetCover(roaster.CoverId);
            responses.Add(response);
        }

        return responses;
    }

    private static IQueryable<CoffeeRoaster> SessionQuery(QueryRoasterList query, IQuerySession session)
    {
        IQueryable<CoffeeRoaster> queryable = session
            .Query<CoffeeRoaster>();

        if (query.CityId.HasValue)
        {
            queryable = session
                .Query<CoffeeRoaster>()
                .Where(x => x.CityId == query.CityId);
        }

        if (!query.Name.IsNullOrEmpty())
        {
            queryable = session
                .Query<CoffeeRoaster>()
                .Where(x => x.Name.Contains(query.Name));
        }

        return queryable;
    }
}