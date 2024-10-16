using Api.App.Common.Enums;
using Api.App.Common.Models;

namespace Api.App.Domain.Map.Models.Responses;

public class CityResponse : BaseResponse
{
    public string Name { get; set; }

    public string Province { get; set; }

    public int RoastersCount { get; set; }
 
    public CountryCodes Country { get; set; }
}