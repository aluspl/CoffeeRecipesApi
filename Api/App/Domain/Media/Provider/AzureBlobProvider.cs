using Api.App.Common.Exceptions;
using Api.App.Common.Extensions;
using Api.App.Domain.Media.Config;
using Api.App.Domain.Media.Enum;
using Api.App.Domain.Media.Interfaces.Provider;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api.App.Domain.Media.Provider;

public class AzureBlobProvider(IConfigurationManager configuration) : IBlobProvider
{
    private readonly MediaConfig _mediaConfig = configuration.GetConfig<MediaConfig>(MediaConfig.Section);
    private readonly string _connectionString = configuration.GetConnectionString("BlobStorage");

    public async Task<string> CreateContainer(ImageType imageType)
    {
        var container = GetClient().GetBlobContainerClient(_mediaConfig.Folders[imageType]);
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

    public async Task<Uri> UploadFile(byte[] file, string containerName, string name)
    {
        var container = GetClient().GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(name);
        await blob.UploadAsync(BinaryData.FromBytes(file));
        return blob.Uri;
    }

    private BlobServiceClient GetClient()
    {
        var client = new BlobServiceClient(_connectionString);
        return client;
    }
}