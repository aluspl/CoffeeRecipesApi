using Api.App.Common.Enums;

namespace Api.App.Domain.Roaster.Handlers.Commands;

public record CommandUpdateRoasterDescription(Guid Id, string Description, Lang Language = Lang.Pl);