namespace Api.App.Domain.Roaster.Handlers.Commands;

public class CommandCreateCoffeeRoaster(string name, Guid cityId, DateTime founded)
{
    public Guid CityId { get; set; } = cityId;

    public string Name { get; set; } = name;

    public DateTime Founded { get; set; } = founded;
}