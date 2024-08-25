using Api.App.Domain.Media.Models;

namespace Api.App.Domain.Media.Getter;

public interface IFileGetter
{
    Task<CoverFileResponse> GetCover(Guid? fileId);
}