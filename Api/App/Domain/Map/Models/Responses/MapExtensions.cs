using Api.App.Common.Extensions;
using Api.App.Domain.Map.Entities;

namespace Api.App.Domain.Map.Models.Responses;

public static class MapExtensions
{
    public static CityResponse Map(this City entity)
    {
        var response = entity.InitResponse<CityResponse>();
        response.ProvinceId = entity.ProvinceId;
        response.Name = entity.Name;

        return response;
    }
    
    public static ProvinceResponse Map(this Province entity, ICollection<CityResponse> cities = null)
    {
        var response = entity.InitResponse<ProvinceResponse>();
        response.Name = entity.Name;
        response.Cities = cities ?? new List<CityResponse>();

        return response;
    }
}