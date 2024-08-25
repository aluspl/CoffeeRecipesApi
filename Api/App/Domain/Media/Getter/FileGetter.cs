using Api.App.Domain.Media.Entity;
using Api.App.Domain.Media.Extensions;
using Api.App.Domain.Media.Models;
using Marten;

namespace Api.App.Domain.Media.Getter;

public class FileGetter(IDocumentSession documentSession) : IFileGetter
{
    public async Task<CoverFileResponse> GetCover(Guid? fileId)
    {
        if (!fileId.HasValue)
        {
            return null;
        }
        
        var entity = await documentSession.Query<CoverFile>().FirstOrDefaultAsync(p => p.Id == fileId);
        return entity?.Map();
    }
}