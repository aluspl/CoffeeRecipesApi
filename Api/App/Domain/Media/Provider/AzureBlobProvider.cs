using Api.App.Common.Configs;
using Api.App.Common.Exceptions;
using Api.App.Domain.Media.Enum;
using Api.App.Media.Interfaces.Provider;
using Api.App.Media.Utils;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Api.App.Domain.Media.Provider;

public class AzureBlobProvider(IOptions<MediaSettings> mediaSetting) : IBlobProvider
{
    private readonly MediaSettings _mediaSettings = mediaSetting.Value;

    public async Task<string> CreateContainer(ImageType imageType)
    {
        var containerName = _mediaSettings.Folders[imageType];
        var container = GetClient().GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();
        await container.SetAccessPolicyAsync(PublicAccessType.Blob);
        return container.Name;
    }

    public async Task<byte[]> DownloadFile(string containerName, string name)
    {
        var container = GetClient().GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(name);
        var exists = await blob.ExistsAsync();
        if (!exists)
        {
            throw new NotFoundException($"File {name} in container {containerName} doesn't exists");
        }

        var file = await blob.DownloadContentAsync();
        return file.Value.Content.ToArray();
    }

    public async Task RemoveFile(string containerName, string name)
    {
        var container = GetClient().GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(name);
        var exists = await blob.ExistsAsync();
        if (!exists)
        {
            return;
        }

        await blob.DeleteAsync();
    }

    public async Task<Uri> UploadImage(Stream file, string containerName, string name)
    {
        file.Position = 0;
        var container = GetClient().GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(name);
        await blob.DeleteIfExistsAsync();
        await blob.UploadAsync(file);
        await blob.SetHttpHeadersAsync(new BlobHttpHeaders()
        {
            ContentType = ContentTypeHelper.GetContentType(name),
        });
        return blob.Uri;
    }

    private BlobServiceClient GetClient()
    {
        var client = new BlobServiceClient(_mediaSettings.ConnectionStrings);
        return client;
    }
}