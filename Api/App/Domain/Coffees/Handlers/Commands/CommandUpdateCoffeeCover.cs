namespace Api.App.Domain.Coffees.Handlers.Commands;

public record CommandUpdateCoffeeCover(Guid Id, Uri ImageUrl, Uri ThumbnailUrl);