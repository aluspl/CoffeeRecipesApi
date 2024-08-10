using Api.App.Common.Extensions;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Models;

namespace Api.App.Domain.Roaster.Extensions;

public static class MapExtensions
{
    public static CoffeeRoasterResponse Map(this CoffeeRoaster entity)
    {
        var response = entity.InitResponse<CoffeeRoasterResponse>();
        response.CityId = entity.CityId;
        response.Name = entity.Name;
        response.Founded = entity.Founded;
        response.Urls = entity.Urls;
        return response;
    }
}