namespace Api.App.Domain.Roaster.Models.Request;

public class UpdateCoffeeRoasterRequest
{
    public string Name { get; set; }
    public Guid CityId { get; set; }
}