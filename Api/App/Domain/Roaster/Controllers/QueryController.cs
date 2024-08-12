using Api.App.Common.Controller;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Roaster.Controllers;

[Area("Roasters")]
[Route("[area]/[controller]")]
public class QueryController(IMessageBus bus) : ApiController
{
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<CoffeeRoasterResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CoffeeRoasterResponse>>> GetAllRoasters()
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeRoasterResponse>>(new QueryRoasterList(null, false));
        return Ok(responses);
    }

    [HttpGet("city/{cityId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetRoastersByCity(Guid cityId)
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeRoasterResponse>>(new QueryRoasterList(cityId, false));
        return Ok(responses);
    }
    
    [HttpGet("details/{roasterId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetDetailsOfRoaster(Guid roasterId)
    {
        if (roasterId == Guid.Empty)
        {
            return BadRequest("Invalid province ID");
        }

        var cities = await bus.InvokeAsync<CoffeeRoasterResponse>(new QueryRoasterDetail(roasterId));
        return Ok(cities);
    }
}

public record QueryRoasterDetail(Guid RoasterId);