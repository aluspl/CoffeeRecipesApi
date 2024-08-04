namespace Api.App.Domain.Map.Models.Request;

public class CityRequest
{
    public string Name { get; set; }
 
    public Guid ProvinceId { get; set; }
}