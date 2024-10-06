using Api.App.Common.Enums;

namespace Api.App.Domain.Coffees.Handlers.Commands;

public record CommandUpdateCoffeeDescription(Guid Id, string Description, Lang Language = Lang.Pl);