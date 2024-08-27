namespace Api.App.Domain.Roaster.Handlers.Commands;

public record CommandUpdateRoasterCover(Guid RoasterId, Uri ImageUrl, Uri ThumbnailUrl);