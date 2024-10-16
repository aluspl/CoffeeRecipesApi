using Api.App.Common.Enums;

namespace Api.App.Domain.Map.Models.Request;

public class CityRequest
{
    public string Name { get; set; }
 
    public string Province { get; set; }

    public CountryCodes Country { get; set; }
}