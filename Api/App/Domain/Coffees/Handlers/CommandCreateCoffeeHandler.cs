using Api.App.Common.Exceptions;
using Api.App.Domain.Coffees.Entities;
using Api.App.Domain.Coffees.Extensions;
using Api.App.Domain.Coffees.Handlers.Commands;
using Api.App.Domain.Coffees.Models;
using Api.App.Domain.Roaster.Entities;
using Marten;

namespace Api.App.Domain.Coffees.Handlers;

public class CommandCreateCoffeeHandler
{
    public static async Task<CoffeeResponse> HandleAsync(CommandCreateCoffee command,
        IDocumentSession session)
    {
        await ValidateName(command.Name, session);
        await ValidateRoaster(command.RoasterId, session);
        
        var entity = new Coffee()
        {
            Name = command.Name,
            RoasterId = command.RoasterId,
        };
        session.Store(entity);
        await session.SaveChangesAsync();

        return entity.Map();
    }

    private static async Task ValidateName(string name, IDocumentSession session)
    {
        if (await session.Query<Coffee>().Where(o => o.Name == name).AnyAsync())
        {
            throw new BusinessException("Another Coffee Roaster with selected name already exists");
        }
    }

    private static async Task ValidateRoaster(Guid roasterId, IDocumentSession session)
    {
        var cityExists = await session.Query<CoffeeRoaster>().Where(o => o.Id == roasterId).AnyAsync();
        if (!cityExists)
        {
            throw new NotFoundException($"City {roasterId} not exists");
        }
    }
}