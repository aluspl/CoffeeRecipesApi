using Api.App.Common.Exceptions;
using Api.App.Domain.Map.Controllers;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Models.Responses;
using Marten;

namespace Api.App.Domain.Map.Handlers;

public class InsertCityHandler
{
    public static async Task<CityResponse> HandleAsync(CommandInsertCity command, IDocumentSession session)
    {
        var province = await session
                           .Query<Province>()
                           .Where(p => p.Id == command.ProvinceId)
                           .FirstOrDefaultAsync()
                       ?? throw new NotFoundException("Province not found");
        var city = new InsertCity()
        {
            ProvinceId = province.Id,
            Name = command.Name,
            Created = DateTime.UtcNow,
        };
        session.Store(city);
        await session.SaveChangesAsync();
        return CityResponse.FromEntity(city);
    }
}