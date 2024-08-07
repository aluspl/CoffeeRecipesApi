using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Domain.Map.Entities;

public class Province : IEntity
{
    public string Name { get; set; }
    
    public Guid Id { get; set; }
    
    public DateTime Created { get; set; }
 
    public DateTime? Updated { get; set; }
}