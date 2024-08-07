using Api.App.Common.Controller;
using Api.App.Domain.Map.Handlers.Commands;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Request;
using Api.App.Domain.Map.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Map.Controllers;

[Area("Map")]
[Route("[area]/[controller]")]
public class CityController(IMessageBus bus) : ApiController
{
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
    {
        var cities = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCity(null));
        return Ok(cities);
    }

    [HttpGet("{provinceId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCitiesByProvinceId(Guid provinceId)
    {
        if (provinceId == Guid.Empty)
        {
            return BadRequest("Invalid province ID");
        }

        var cities = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCity(provinceId));
        return Ok(cities);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CityResponse), 200)]
    public async Task<ActionResult<CityResponse>> Insert(CityRequest city)
    {
        if (city.ProvinceId == Guid.Empty)
        {
            return BadRequest("Invalid province ID");
        }

        var response = await bus.InvokeAsync<CityResponse>(new CommandInsertCity(city.Name, city.ProvinceId));
        return Ok(response);
    }
}