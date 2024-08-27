using Api.App.Domain.Common.Models;
using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Domain.Common.Extensions;

public static class MapExtensions
{
    public static UrlResponse Map(this UrlDetail entity)
    {
        return new UrlResponse()
        {
            Url = entity.Url,
            Description = entity.Description,
        };
    }
}