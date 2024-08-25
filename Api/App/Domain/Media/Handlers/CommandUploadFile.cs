using Api.App.Domain.Media.Enum;

namespace Api.App.Domain.Media.Handlers;

public record CommandUploadFile(string Name, ImageType ImageType, byte[] File);