using Api.App.Common.Extensions;
using Api.App.Domain.Coffees.Entities;
using Api.App.Domain.Coffees.Models;
using Api.App.Domain.Common.Extensions;
using Api.App.Domain.Media.Extensions;

namespace Api.App.Domain.Coffees.Extensions;

public static class MapExtensions
{
    public static CoffeeResponse Map(this Coffee entity)
    {
        var response = entity.InitResponse<CoffeeResponse>();
        response.RoasterId = entity.RoasterId;
        response.Name = entity.Name;
        response.Urls = entity.Urls?.Select(p => p.Map());
        response.Description = entity.Description;
        response.Image = entity.Image?.Map();
        return response;
    }
}