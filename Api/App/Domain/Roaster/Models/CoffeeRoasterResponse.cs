using Api.App.Common.Enums;
using Api.App.Common.Models;
using Api.App.Domain.Common.Extensions;
using Api.App.Domain.Common.Models;
using Api.App.Domain.Media.Models;

namespace Api.App.Domain.Roaster.Models;

public class CoffeeRoasterResponse : BaseResponse
{
    public Guid CityId { get; set; }

    public string Name { get; set; }

    public IDictionary<Lang, string> Description { get; set; }

    public DateTime? Founded { get; set; }

    public IEnumerable<UrlResponse> Urls { get; set; }
 
    public CoverFileResponse Image { get; set; }
}