using Api.App.Common.Enums;
using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Domain.Map.Entities;

public class City : IEntity
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime? Updated { get; set; }

    public bool IsDeleted { get; set; }

    public string Name { get; set; }

    public string Province { get; set; }

    public CountryCodes Country { get; set; }

    public int RoastersCount { get; set; }
}