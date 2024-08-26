namespace Api.App.Domain.Media.Handlers.Commands;

public record UploadFileModel(Guid Id, string FileExtensions, Stream File);