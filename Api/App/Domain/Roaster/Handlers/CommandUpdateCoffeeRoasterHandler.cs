using Api.App.Common.Exceptions;
using Api.App.Common.Extensions;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Extensions;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Marten;

namespace Api.App.Domain.Roaster.Handlers;

public class CommandUpdateCoffeeRoasterHandler
{
    public static async Task<CoffeeRoasterResponse> HandleAsync(CommandUpdateCoffeeRoaster command, IDocumentStore store)
    {
        await using var session = store.LightweightSession();
        var entity = await session.Query<CoffeeRoaster>().Where(roaster => roaster.Id == command.Id).FirstOrDefaultAsync();
        await ValidateAndUpdateCity(command.CityId, session, entity);

        if (!command.Name.IsNullOrEmpty())
        {
            entity.Name = command.Name;
            entity.Updated = DateTime.UtcNow;
        }
        session.Store(entity);
        await session.SaveChangesAsync();
        
        return entity.Map();
    }

    private static async Task ValidateAndUpdateCity(Guid? cityId, IDocumentSession session, CoffeeRoaster entity)
    {
        if (!cityId.HasValue)
        {
            return;
        }

        var cityExists = await session.Query<City>().Where(o => o.Id == cityId).AnyAsync();
        if (!cityExists)
        {
            throw new NotFoundException($"City {cityId} not found");
        }

        entity.CityId = cityId.Value;
    }
}