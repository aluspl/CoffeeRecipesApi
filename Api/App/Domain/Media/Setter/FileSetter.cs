using Api.App.Domain.Media.Enum;
using Api.App.Domain.Media.Handlers;
using Api.App.Domain.Media.Handlers.Commands;
using Api.App.Domain.Media.Handlers.Results;
using Api.App.Domain.Media.Interfaces;
using Api.App.Domain.Media.Interfaces.Provider;
using Api.App.Domain.Media.Utils;

namespace Api.App.Domain.Media.Setter;

public class FileSetter(IBlobProvider blobProvider) : IFileSetter
{
    public async Task<FileUploaded> Upload(UploadFileModel command)
    {
        var name = $"{command.Id}{command.FileExtensions}";

        var imageUrl = await UploadImage(command, blobProvider, name);
        var thumbnailUrl = await GenerateThumbnails(command, blobProvider, name);

        return new FileUploaded(true, imageUrl, thumbnailUrl);
    }

    private static async Task<Uri> UploadImage(UploadFileModel command, IBlobProvider blobProvider, string name)
    {
        var container = await blobProvider.CreateContainer(ImageType.Cover);

        ImageUtils.ValidateImage(command.File);
        var image = await blobProvider.UploadImage(command.File, container, name);
        return image;
    }

    private static async Task<Uri> GenerateThumbnails(UploadFileModel command, IBlobProvider blobProvider, string name)
    {
        command.File.Position = 0;
        var thumbContainer = await blobProvider.CreateContainer(ImageType.Thumbnail);

        await ImageUtils.ResizeImage(command.File);
        var thumbnail = await blobProvider.UploadImage(command.File, thumbContainer, name);
        return thumbnail;
    }
}