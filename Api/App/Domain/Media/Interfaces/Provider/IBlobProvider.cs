using Api.App.Domain.Media.Enum;

namespace Api.App.Media.Interfaces.Provider;

public interface IBlobProvider
{
    Task<string> CreateContainer(ImageType imageType);

    Task<byte[]> DownloadFile(string container, string name);

    Task RemoveFile(string container, string name);

    Task<Uri> UploadFile(byte[] file, string container, string name);
}