using Api.App.Common.Models;

namespace Api.App.Domain.Map.Models.Responses;

public class CityResponse : BaseResponse
{
    public string Name { get; set; }

    public Guid? ProvinceId { get; set; }

    public int RoastersCount { get; set; }
}