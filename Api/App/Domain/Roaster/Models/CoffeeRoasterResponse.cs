using Api.App.Common.Extensions;
using Api.App.Common.Models;
using Api.App.Domain.Roaster.Entities;

namespace Api.App.Domain.Roaster.Models;

public class CoffeeRoasterResponse : BaseResponse
{
    public Guid CityId { get; set; }

    public string Name { get; set; }

    public DateTime? Founded { get; set; }

    public IEnumerable<Uri> Urls { get; set; }
}