using Api.App.Domain.Common.Models;

namespace Api.App.Domain.Roaster.Models.Request;

public class UpdateCoffeeRoasterLinksRequest
{
    public Guid Id { get; set; }

    public List<UrlRequest> Urls { get; set; }
}