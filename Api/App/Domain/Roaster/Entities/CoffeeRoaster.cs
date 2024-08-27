using Api.App.Common.Enums;
using Api.App.Domain.Common.Models;
using Api.App.Domain.Media.Entity;
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

    public IList<UrlDetail> Urls { get; set; } = new List<UrlDetail>();

    public IDictionary<Lang, string> Description { get; set; } = new Dictionary<Lang, string>();

    #endregion
    #region References

    public Guid CityId { get; set; }

    public CoverFile Image { get; set; }

    #endregion

    public void AddCoverImage(Uri url)
    {
        Image ??= new CoverFile();
        Image.ImageUrl = url;
        Updated = DateTime.UtcNow;
    }

    public void AddCoverThumbnail(Uri thumbnailUrl)
    {
        Image ??= new CoverFile();
        Image.ThumbnailUrl = thumbnailUrl;
        Updated = DateTime.UtcNow;
    }

    public void UpdateDescription(string description, Lang language)
    {
        Description[language] = description;
    }
    
    public void UpdateLinks(ICollection<UrlRequest> urls)
    {
        Urls = urls?
            .Select(pair => new UrlDetail(pair.Url, pair.Description))
            .ToList();
        Updated = DateTime.UtcNow;
    }
}