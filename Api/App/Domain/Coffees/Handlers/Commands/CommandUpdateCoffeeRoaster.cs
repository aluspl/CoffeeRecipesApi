namespace Api.App.Domain.Coffees.Handlers.Commands;

public record CommandUpdateCoffeeRoaster(Guid Id, Guid? RoasterId);