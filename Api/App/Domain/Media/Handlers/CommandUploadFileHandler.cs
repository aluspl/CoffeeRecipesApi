using Api.App.Domain.Media.Interfaces.Provider;
using Marten;

namespace Api.App.Domain.Media.Handlers;

public class CommandUploadFileHandler
{
    public async Task<FileUploaded> Handle(CommandUploadFile command, IDocumentSession session, IBlobProvider blobProvider)
    {
        var container = await blobProvider.CreateContainer(command.ImageType);
        var file = await blobProvider.UploadFile(command.File, container, command.Name);
        return new FileUploaded(true, file);
    }
}