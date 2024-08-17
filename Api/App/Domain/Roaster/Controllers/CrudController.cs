using Api.App.Common.Controller;
using Api.App.Domain.Roaster.Handlers;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Api.App.Domain.Roaster.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Roaster.Controllers;

[Area("Roasters")]
[Route("[area]/[controller]")]
public class CrudController(IMessageBus bus) : ApiKeyController
{
    [HttpPost()]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> Create([FromBody] CreateCoffeeRoasterRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandCreateCoffeeRoaster(request.Name, request.CityId, request.Founded));
        return Ok(responses);
    }
    
    [HttpPut("{coffeeRoasterId}")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> Update([FromRoute] Guid coffeeRoasterId, [FromBody] UpdateCoffeeRoasterRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandUpdateCoffeeRoaster(coffeeRoasterId, request.Name, request.CityId));
        return Ok(responses);
    }
}