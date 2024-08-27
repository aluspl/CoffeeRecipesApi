namespace Api.App.Domain.Coffees.Models.Request;

public class UpdateCoffeeRoasterRequest
{
    public Guid Id { get; set; }

    public Guid RoasterId { get; set; }
}