using Api.App.Domain.Media.Handlers.Commands;
using Api.App.Domain.Media.Handlers.Results;

namespace Api.App.Domain.Media.Handlers;

public interface IFileSetter
{
    Task<FileUploaded> Upload(UploadFileModel command);
}