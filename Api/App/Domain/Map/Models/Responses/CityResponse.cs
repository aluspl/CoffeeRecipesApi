using Api.App.Domain.Map.Entities;

namespace Api.App.Domain.Map.Models.Responses;

public class CityResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid? ProvinceId { get; set; }

    public static CityResponse FromEntity(City city)
    {
        return new CityResponse()
        {
            Id = city.Id,
            Name = city.Name,
            ProvinceId = city.ProvinceId,
        };
    }
}