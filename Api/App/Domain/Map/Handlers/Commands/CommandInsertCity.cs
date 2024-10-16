using Api.App.Common.Enums;

namespace Api.App.Domain.Map.Handlers.Commands;

public record CommandInsertCity(string Name, string Province, CountryCodes Country);