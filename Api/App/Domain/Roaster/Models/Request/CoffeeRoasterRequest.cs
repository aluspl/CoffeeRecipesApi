namespace Api.App.Domain.Roaster.Models.Request;

public class CoffeeRoasterRequest
{
    public Guid CityId { get; set; }

    public string Name { get; set; }

    public DateTime Founded { get; set; }
}