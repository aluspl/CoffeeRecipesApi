using Api.App.Common.Extensions;
using Api.App.Domain.Common.Extensions;
using Api.App.Domain.Media.Extensions;
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
        response.Urls = entity.Urls?.Select(p => p.Map());
        response.Description = entity.Description;
        response.Image = entity.Image?.Map();
        return response;
    }
}