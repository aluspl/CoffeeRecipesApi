namespace Api.App.Domain.Roaster.Models.Request;

public class UpdateCoffeeRoasterCityRequest
{
    public Guid Id { get; set; }

    public Guid CityId { get; set; }
}