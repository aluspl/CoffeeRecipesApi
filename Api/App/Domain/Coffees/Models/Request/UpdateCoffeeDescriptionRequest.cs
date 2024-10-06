using Api.App.Common.Enums;

namespace Api.App.Domain.Coffees.Models.Request;

public class UpdateCoffeeDescriptionRequest
{
    public Guid Id { get; set; }
    
    public string Description { get; set; }

    public Lang Lang { get; set; } = Lang.Pl;
}