using Api.App.Common.Extensions;
using Api.App.Domain.Media.Entity;
using Api.App.Domain.Media.Models;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Models;

namespace Api.App.Domain.Media.Extensions;

public static class MapExtensions
{
    public static CoverFileResponse Map(this CoverFile entity)
    {
        var response = new CoverFileResponse()
        {
            Cover = entity.ImageUrl,
            Thumbnail = entity.ThumbnailUrl,
        };
        
        return response;
    }
}