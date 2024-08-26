using Api.App.Domain.Media.Enum;
using Api.App.Media.Interfaces.Provider;

namespace Api.Tests.MockProviders;

public class MockBlobProviders : IBlobProvider
{
    public Task<string> CreateContainer(ImageType imageType)
    {
        return Task.FromResult(imageType.ToString());
    }

    public Task<byte[]> DownloadFile(string container, string name)
    {
        return Task.FromResult(new byte[2137]);
    }

    public Task RemoveFile(string container, string name)
    {
        return Task.CompletedTask;
    }

    public Task<Uri> UploadImage(Stream file, string container, string name)
    {
        return Task.FromResult(new Uri("https://google.pl"));
    }
}