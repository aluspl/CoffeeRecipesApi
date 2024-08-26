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
        IDocumentSession session)
    {
        await ValidateName(command.Name, session);
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

    private static async Task ValidateName(string name, IDocumentSession session)
    {
        if (await session.Query<CoffeeRoaster>().Where(o => o.Name == name).AnyAsync())
        {
            throw new BusinessException("Another Coffee Roaster with selected name already exists");
        }
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