namespace Api.App.Domain.Coffees.Models.Request;

public class UpdateCoffeeNameRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}