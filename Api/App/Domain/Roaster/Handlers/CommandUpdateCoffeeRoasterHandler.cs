using Api.App.Common.Exceptions;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Extensions;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Marten;
using Wolverine.Attributes;

namespace Api.App.Domain.Roaster.Handlers;

[WolverineHandler]
public class CommandUpdateCoffeeRoasterHandler
{
    public static async Task<CoffeeRoasterResponse> HandleAsync(CommandUpdateRoasterCity command, IDocumentSession session)
    {
        var entity = await GetCoffeeRoaster(command.Id, session);

        await ValidateAndUpdateCity(command.CityId, session, entity);

        session.Store(entity);
        await session.SaveChangesAsync();
        
        return entity.Map();
    }

    public static async Task<CoffeeRoasterResponse> HandleAsync(CommandUpdateRoasterName command, IDocumentSession session)
    {
        if (await session.Query<CoffeeRoaster>().Where(o => o.Name == command.Name && o.Id != command.Id).AnyAsync())
        {
            throw new BusinessException("Another Coffee Roaster with selected name already exists");
        }
        
        var entity = await GetCoffeeRoaster(command.Id, session);

        entity.Name = command.Name;
        session.Store(entity);
        await session.SaveChangesAsync();
        
        return entity.Map();
    }

    public static async Task<CoffeeRoasterResponse> HandleAsync(CommandUpdateRoasterLinks command,
        IDocumentSession session)
    {
        var entity = await GetCoffeeRoaster(command.Id, session);
        entity.Urls = command.Urls.Select(p => new Uri(p));
        session.Store(entity);
        await session.SaveChangesAsync();

        return entity.Map();
    }
    
    private static async Task<CoffeeRoaster> GetCoffeeRoaster(Guid id, IDocumentSession session)
    {
        var entity = await session
            .Query<CoffeeRoaster>()
            .Where(roaster => roaster.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Coffee Roaster {id} not found");
        return entity;
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