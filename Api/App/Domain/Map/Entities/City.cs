using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Domain.Map.Entities;

public class City : IEntity
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }
    
    public string Name { get; set; }

    public Guid ProvinceId { get; set; }
}