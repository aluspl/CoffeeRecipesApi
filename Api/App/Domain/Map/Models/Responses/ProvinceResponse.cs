using Api.App.Common.Extensions;
using Api.App.Common.Models;
using Api.App.Domain.Map.Entities;

namespace Api.App.Domain.Map.Models.Responses;

public class ProvinceResponse : BaseResponse
{
    public string Name { get; set; }

    public ICollection<CityResponse> Cities { get; set; }
}