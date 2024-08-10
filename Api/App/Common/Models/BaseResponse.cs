using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Common.Models;

public abstract class BaseResponse
{
    public DateTime? Updated { get; set; }

    public DateTime Created { get; set; }

    public Guid Id { get; set; }
}