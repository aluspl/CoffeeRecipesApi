using Api.App.Domain.Media.Entity;
using Api.App.Domain.Media.Enum;
using Api.App.Domain.Media.Handlers.Results;
using Api.App.Media.Handlers.Commands;
using Api.App.Media.Interfaces.Provider;
using Api.App.Media.Utils;
using Marten;

namespace Api.App.Domain.Media.Handlers;

public class CommandUploadFileHandler
{
    public async Task<FileUploaded> Handle(CommandUploadRoasterFile command, IDocumentSession session, IBlobProvider blobProvider)
    {
        var file = await GetOrCreateFile(session, command);
        
        await UploadImage(command, blobProvider, file);
        await GenerateThumbnails(command, blobProvider, file);

        session.Store(file);
        await session.SaveChangesAsync();
        return new FileUploaded(true, file.Id);
    }

    private static async Task<CoverFile> GetOrCreateFile(IDocumentSession session, CommandUploadRoasterFile command)
    {
        var file = await session
            .Query<CoverFile>()
            .Where(file1 => file1.Name.Contains(command.Id.ToString()))
            .FirstOrDefaultAsync() ?? new CoverFile
        {
            Name = $"{command.Id}{command.FileExtensions}"
        };

        return file;
    }

    private static async Task UploadImage(CommandUploadRoasterFile command, IBlobProvider blobProvider, CoverFile coverFile)
    {
        var container = await blobProvider.CreateContainer(ImageType.Cover);

        ImageUtils.ValidateImage(command.File);
        var image = await blobProvider.UploadFile(command.File, container, coverFile.Name);
        coverFile.ImageUrl = image.ToString();
    }

    private static async Task GenerateThumbnails(CommandUploadRoasterFile command, IBlobProvider blobProvider, CoverFile coverFile)
    {
        var thumbContainer = await blobProvider.CreateContainer(ImageType.Thumbnail);

        var thumbnailFile = await ImageUtils.ResizeImage(command.File);
        var thumbnail = await blobProvider.UploadFile(thumbnailFile, thumbContainer, coverFile.Name);
        coverFile.ThumbnailUrl = thumbnail.ToString();
    }
}