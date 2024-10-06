using Api.App.Domain.Common.Models;

namespace Api.App.Domain.Roaster.Handlers.Commands;

public record CommandUpdateRoasterLinks(Guid Id, List<UrlRequest> Urls);