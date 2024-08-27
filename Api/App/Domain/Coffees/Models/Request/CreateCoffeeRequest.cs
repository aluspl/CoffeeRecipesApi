namespace Api.App.Domain.Coffees.Models.Request;

public class CreateCoffeeRequest
{
    public Guid RoasterId { get; set; }

    public string Name { get; set; }
}