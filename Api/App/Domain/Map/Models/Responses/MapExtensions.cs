using Api.App.Common.Extensions;
using Api.App.Domain.Map.Entities;

namespace Api.App.Domain.Map.Models.Responses;

public static class MapExtensions
{
    public static CityResponse Map(this City entity)
    {
        var response = entity.InitResponse<CityResponse>();
        response.Province = entity.Province;
        response.Country = entity.Country;
        response.RoastersCount = entity.RoastersCount;
        response.Name = entity.Name;

        return response;
    }
}