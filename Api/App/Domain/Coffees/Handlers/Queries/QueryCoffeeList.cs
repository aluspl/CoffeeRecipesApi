namespace Api.App.Domain.Coffees.Handlers.Queries;

public record QueryCoffeeList(Guid? RoasterId, string Name, bool IsPromoted);