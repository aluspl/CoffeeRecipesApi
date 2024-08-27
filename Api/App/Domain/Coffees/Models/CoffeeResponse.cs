using Api.App.Common.Enums;
using Api.App.Common.Models;
using Api.App.Domain.Common.Models;
using Api.App.Domain.Media.Models;

namespace Api.App.Domain.Coffees.Models;

public class CoffeeResponse : BaseResponse
{
    public Guid RoasterId { get; set; }

    public string Name { get; set; }

    public IDictionary<Lang, string> Description { get; set; }

    public IEnumerable<UrlResponse> Urls { get; set; }
 
    public CoverFileResponse Image { get; set; }
}