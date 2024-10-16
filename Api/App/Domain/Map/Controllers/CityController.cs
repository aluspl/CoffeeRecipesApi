using Api.App.Common.Controller;
using Api.App.Common.Extensions;
using Api.App.Domain.Map.Handlers.Commands;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Request;
using Api.App.Domain.Map.Models.Responses;
using Api.App.Domain.Security.Attributes;
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
        var cities = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCityListByName(null));
        return Ok(cities);
    }

    [HttpGet("{province}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCitiesByProvinceId(string province)
    {
        if (province.IsNullOrEmpty())
        {
            return BadRequest("Missing province name");
        }

        var cities = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCityListByProvinceName(province));
        return Ok(cities);
    }
    
    [HttpGet("search/{name}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCitiesByName(string name)
    {
        if (name.IsNullOrEmpty())
        {
            return BadRequest("Invalid City Name");
        }

        var cities = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCityListByName(name));
        return Ok(cities);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CityResponse), 200)]
    [ApiKey]
    public async Task<ActionResult<CityResponse>> Insert(CityRequest city)
    {
        if (city.Province.IsNullOrEmpty())
        {
            return BadRequest("Missing province");
        }

        var response = await bus.InvokeAsync<CityResponse>(new CommandInsertCity(city.Name, city.Province, city.Country));
        return Ok(response);
    }
}