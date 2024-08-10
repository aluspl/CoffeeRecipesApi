using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Domain.Map.Entities;

public class Province : IEntity
{
    public Guid Id { get; set; }
    
    public DateTime Created { get; set; }
 
    public DateTime? Updated { get; set; }

    public string Name { get; set; }
}