namespace Api.App.Domain.Roaster.Handlers.Commands;

public record CommandUpdateRoasterLinks(Guid Id, List<string> Urls);