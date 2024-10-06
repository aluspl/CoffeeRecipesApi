using Api.App.Domain.Media.Entity;
using Api.App.Domain.Media.Models;

namespace Api.App.Domain.Media.Extensions;

public static class MapExtensions
{
    public static CoverFileResponse Map(this CoverFile entity)
    {
        var response = new CoverFileResponse()
        {
            Cover = entity.ImageUrl?.ToString(),
            Thumbnail = entity.ThumbnailUrl?.ToString(),
        };
        
        return response;
    }
}