namespace Api.App.Media.Handlers.Commands;

public record CommandUploadRoasterFile(Guid Id, string FileExtensions, byte[] File);