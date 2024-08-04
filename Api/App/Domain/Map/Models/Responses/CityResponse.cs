namespace Api.App.Domain.Map.Models.Responses;

public class CityResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ProvinceResponse Province { get; set; }
}