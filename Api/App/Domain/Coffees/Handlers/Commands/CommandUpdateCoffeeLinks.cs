using Api.App.Domain.Common.Models;

namespace Api.App.Domain.Coffees.Handlers.Commands;

public record CommandUpdateCoffeeLinks(Guid Id, ICollection<UrlRequest> Urls);