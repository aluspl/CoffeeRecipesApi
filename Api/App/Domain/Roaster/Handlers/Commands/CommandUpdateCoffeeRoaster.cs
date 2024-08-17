namespace Api.App.Domain.Roaster.Handlers.Commands;

public class CommandUpdateCoffeeRoaster(Guid id, string name, Guid? cityId)
{
    public Guid Id { get; set; } = id;

    public Guid? CityId { get; set; } = cityId;

    public string Name { get; set; } = name;
}