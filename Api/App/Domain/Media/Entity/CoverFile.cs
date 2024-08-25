using Api.App.Infrastructure.Database.Entities;

namespace Api.App.Domain.Media.Entity;

public class CoverFile : IEntity
{
    public Guid Id { get; set; }
    
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public DateTime? Updated { get; set; }
 
    public bool IsDeleted { get; set; }

    public string Name { get; set; }
 
    public string ImageUrl { get; set; }
 
    public string ThumbnailUrl { get; set; }
}