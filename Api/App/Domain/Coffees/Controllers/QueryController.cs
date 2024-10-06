using Api.App.Common.Controller;
using Api.App.Domain.Map.Models.Responses;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Coffees.Controllers;

[Area("Coffee")]
[Route("[area]/[controller]")]
public class QueryController(IMessageBus bus) : ApiController
{
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<CoffeeRoasterResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CoffeeRoasterResponse>>> GetAllRoasters()
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeRoasterResponse>>(new QueryRoasterList(null, null, false));
        return Ok(responses);
    }

    [HttpGet("city/{cityId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetRoastersByCity(Guid cityId)
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeRoasterResponse>>(new QueryRoasterList(cityId, null, false));
        return Ok(responses);
    }
    
    [HttpGet("search/{name}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetRoastersByName(string name)
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeRoasterResponse>>(new QueryRoasterList(null, name, false));
        return Ok(responses);
    }
    
    [HttpGet("details/{roasterId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetDetailsOfRoaster(Guid roasterId)
    {
        if (roasterId == Guid.Empty)
        {
            return BadRequest("Invalid roaster ID");
        }

        var cities = await bus.InvokeAsync<CoffeeRoasterResponse>(new QueryRoasterDetail(roasterId));
        return Ok(cities);
    }
}