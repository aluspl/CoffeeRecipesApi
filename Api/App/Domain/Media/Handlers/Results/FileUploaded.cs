namespace Api.App.Domain.Media.Handlers.Results;

public record FileUploaded(bool IsSuccess, Uri ImageUrl, Uri ThumbnailUrl);
