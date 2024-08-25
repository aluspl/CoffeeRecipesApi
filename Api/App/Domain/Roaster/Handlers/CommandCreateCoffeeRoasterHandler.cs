using Api.App.Common.Exceptions;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Extensions;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Marten;

namespace Api.App.Domain.Roaster.Handlers;

public class CommandCreateCoffeeRoasterHandler
{
    public static async Task<CoffeeRoasterResponse> HandleAsync(CommandCreateCoffeeRoaster command,
        IDocumentStore store)
    {
        await using var session = store.LightweightSession();
        await ValidateCity(command.CityId, session);
        
        var entity = new CoffeeRoaster()
        {
            Name = command.Name,
            CityId = command.CityId,
        };
        session.Store(entity);
        await session.SaveChangesAsync();

        return entity.Map();
    }

    private static async Task ValidateCity(Guid cityId, IDocumentSession session)
    {
        var cityExists = await session.Query<City>().Where(o => o.Id == cityId).AnyAsync();
        if (!cityExists)
        {
            throw new NotFoundException($"City {cityId} not exists");
        }
    }
}