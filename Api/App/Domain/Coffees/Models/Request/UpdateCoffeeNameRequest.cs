using Api.App.Domain.Common.Models;

namespace Api.App.Domain.Coffees.Models.Request;

public class UpdateCoffeeLinksRequest
{
    public Guid Id { get; set; }

    public List<UrlRequest> Urls { get; set; }
}