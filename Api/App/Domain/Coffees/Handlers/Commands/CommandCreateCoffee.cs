namespace Api.App.Domain.Coffees.Handlers.Commands;

public record CommandCreateCoffee(string Name, Guid RoasterId);