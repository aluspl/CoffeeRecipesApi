namespace Api.App.Domain.Roaster.Models.Request;

public class CreateCoffeeRoasterRequest
{
    public Guid CityId { get; set; }

    public string Name { get; set; }

    public DateTime Founded { get; set; }
}