using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Domain.Roaster.Entities;

public class CoffeeRoaster : IEntity
{
    #region Base

    public Guid Id { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime? Updated { get; set; }

    public bool IsDeleted { get; set; }

    #endregion

    #region Data

    public string Name { get; set; }

    public DateTime? Founded { get; set; }

    public IEnumerable<Uri> Urls { get; set; } = new List<Uri>();

    public string Description { get; set; }

    #endregion
    #region References

    public Guid CityId { get; set; }

    #endregion
} 