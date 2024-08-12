using Api.App.Common.Models;
using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Common.Extensions;

public static class MapExtensions
{
    public static T InitResponse<T>(this IEntity entity) where T : BaseResponse, new()
    {
        var fromBaseEntity = new T()
        {
            Id = entity.Id,
            Created = entity.Created,
            Updated = entity.Updated,
        };
        
        return fromBaseEntity;
    }
}