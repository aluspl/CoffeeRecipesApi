using Api.App.Domain.Map.Entities;

namespace Api.App.Domain.Map.Models.Responses;

public class ProvinceResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<CityResponse> Cities { get; set; }

    public ProvinceResponse FromEntity(Province province)
    {
        return new ProvinceResponse()
        {
            Id = province.Id,
            Name = province.Name,
            Cities = new List<CityResponse>(),
        };
    }
}