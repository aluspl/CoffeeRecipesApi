using Api.App.Common.Exceptions;
using Api.App.Domain.Map.Controllers;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Commands;
using Api.App.Domain.Map.Models.Responses;
using Marten;
using MapExtensions = Api.App.Domain.Map.Models.Responses.MapExtensions;

namespace Api.App.Domain.Map.Handlers;

public class InsertCityHandler
{
    public static async Task<CityResponse> HandleAsync(CommandInsertCity command, IDocumentSession session)
    {
        var city = new City()
        {
            Name = command.Name,
            Created = DateTime.UtcNow,
            Province = command.Province,
            Country = command.Country,
        };
        
        session.Store(city);
        await session.SaveChangesAsync();
        return city.Map();
    }
}