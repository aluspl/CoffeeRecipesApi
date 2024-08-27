using Api.App.Common.Exceptions;
using Api.App.Domain.Coffees.Entities;
using Api.App.Domain.Coffees.Handlers.Commands;
using Api.App.Domain.Coffees.Models.Records;
using Api.App.Domain.Roaster.Entities;
using Marten;
using Wolverine.Attributes;

namespace Api.App.Domain.Coffees.Handlers;

[WolverineHandler]
public class CommandUpdateCoffeeHandler
{
    public static async Task<CoffeeUpdated> HandleAsync(CommandUpdateCoffeeRoaster command, IDocumentSession session)
    {
        var entity = await GetCoffeeRoaster(command.Id, session);

        await ValidateAndUpdateRoaster(command.RoasterId, session, entity);

        entity.Updated = DateTime.UtcNow;
        session.Store(entity);
        await session.SaveChangesAsync();
        
        return new CoffeeUpdated();
    }

    public static async Task<CoffeeUpdated> HandleAsync(CommandUpdateCoffeeName command, IDocumentSession session)
    {
        if (await session.Query<Coffee>().Where(o => o.Name == command.Name && o.Id != command.Id).AnyAsync())
        {
            throw new BusinessException("Another Coffee Roaster with selected name already exists");
        }
        
        var entity = await GetCoffeeRoaster(command.Id, session);

        entity.Name = command.Name;
        entity.Updated = DateTime.UtcNow;
        session.Store(entity);
        await session.SaveChangesAsync();
        
        return new CoffeeUpdated();
    }

    public static async Task<CoffeeUpdated> HandleAsync(CommandUpdateCoffeeLinks command, IDocumentSession session)
    {
        var entity = await GetCoffeeRoaster(command.Id, session);
        entity.UpdateLinks(command.Urls);
        session.Store(entity);
        await session.SaveChangesAsync();

        return new CoffeeUpdated();
    }
    
    public static async Task<CoffeeUpdated> HandleAsync(CommandUpdateCoffeeDescription command, IDocumentSession session)
    {
        var entity = await GetCoffeeRoaster(command.Id, session);
        entity.UpdateDescription(command.Description, command.Language);
        session.Store(entity);
        await session.SaveChangesAsync();

        return new CoffeeUpdated();
    }
    
    public static async Task<CoffeeUpdated> HandleAsync(CommandUpdateCoffeeCover command, IDocumentSession session)
    {
        var entity = await GetCoffeeRoaster(command.Id, session);
        entity.AddCoverImage(command.ImageUrl);
        entity.AddCoverThumbnail(command.ThumbnailUrl);
        session.Store(entity);
        await session.SaveChangesAsync();

        return new CoffeeUpdated();
    }
    
    private static async Task<Coffee> GetCoffeeRoaster(Guid id, IDocumentSession session)
    {
        var entity = await session
            .Query<Coffee>()
            .Where(roaster => roaster.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Coffee Roaster {id} not found");
        return entity;
    }

    private static async Task ValidateAndUpdateRoaster(Guid? roasterId, IDocumentSession session, Coffee entity)
    {
        if (!roasterId.HasValue)
        {
            return;
        }

        var exists = await session.Query<CoffeeRoaster>().Where(o => o.Id == roasterId).AnyAsync();
        if (!exists)
        {
            throw new NotFoundException($"Roaster {roasterId} not found");
        }

        entity.RoasterId = roasterId.Value;
    }
}