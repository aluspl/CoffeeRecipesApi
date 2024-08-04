using Marten.Schema;

namespace Api.App.Infrastructure.Database.Entities;

public interface IEntity
{
    [Identity]
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }
}