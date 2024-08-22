namespace Api.App.Domain.Roaster.Models.Request;

public class UpdateCoffeeRoasterLinksRequest
{
    public Guid Id { get; set; }

    public List<string> Urls { get; set; }
}