using Api.App.Common.Controller;
using Api.App.Domain.Coffees.Handlers.Queries;
using Api.App.Domain.Coffees.Models;
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
    [ProducesResponseType(typeof(IEnumerable<CoffeeResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CoffeeResponse>>> GetAllRoasters()
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeRoasterResponse>>(new QueryCoffeeList(null, null, false));
        return Ok(responses);
    }

    [HttpGet("roaster/{roasterId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CoffeeResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetRoastersByCity(Guid roasterId)
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeResponse>>(new QueryCoffeeList(roasterId, null, false));
        return Ok(responses);
    }
    
    [HttpGet("search/{name}")]
    [ProducesResponseType(typeof(IEnumerable<CoffeeResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CoffeeResponse>>> GetRoastersByName(string name)
    {
        var responses = await bus.InvokeAsync<IEnumerable<CoffeeResponse>>(new QueryCoffeeList(null, name, false));
        return Ok(responses);
    }
    
    [HttpGet("details/{coffeeId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CoffeeResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CoffeeResponse>>> GetDetailsOfRoaster(Guid coffeeId)
    {
        if (coffeeId == Guid.Empty)
        {
            return BadRequest("Invalid Coffee Id");
        }

        var cities = await bus.InvokeAsync<CoffeeResponse>(new QueryCoffeeDetail(coffeeId));
        return Ok(cities);
    }
}