namespace Api.App.Domain.Roaster.Handlers.Queries;

public record QueryRoasterList(Guid? CityId, string Name, bool IsPromoted);