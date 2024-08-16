using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Extensions;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Marten;

namespace Api.App.Domain.Roaster.Handlers;

public class CommandCreateCoffeeRoasterHandler
{
    public static async Task<CoffeeRoasterResponse> HandleAsync(CommandCreateCoffeeRoaster command, IDocumentStore store)
    {
        await using var session = store.LightweightSession();

        var entity = new CoffeeRoaster()
        {
            Name = command.Name,
            CityId = command.CityId, // Validate If Exists
            Founded = command.Founded,
        };
        session.Store(entity);
        await session.SaveChangesAsync();
        
        return entity.Map();
    } 
}