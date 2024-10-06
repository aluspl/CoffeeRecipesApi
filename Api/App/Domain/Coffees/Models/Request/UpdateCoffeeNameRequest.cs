namespace Api.App.Domain.Coffees.Models.Request;

public class UpdateCoffeeLinksRequest
{
    public Guid Id { get; set; }

    public List<string> Urls { get; set; }
}