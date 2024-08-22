namespace Api.App.Domain.Roaster.Models.Request;

public class UpdateCoffeeRoasterNameRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}