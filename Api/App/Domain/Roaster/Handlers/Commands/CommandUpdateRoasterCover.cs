namespace Api.App.Roaster.Handlers.Commands;

public record CommandUpdateRoasterCover(Guid RoasterId, Uri ImageUrl, Uri ThumbnailUrl);