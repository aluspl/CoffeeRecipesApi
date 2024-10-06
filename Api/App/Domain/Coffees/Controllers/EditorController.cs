using Api.App.Common.Controller;
using Api.App.Domain.Coffees.Handlers.Commands;
using Api.App.Domain.Coffees.Models;
using Api.App.Domain.Coffees.Models.Records;
using Api.App.Domain.Coffees.Models.Request;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Api.App.Domain.Roaster.Models.Records;
using Api.App.Domain.Roaster.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Wolverine;

namespace Api.App.Domain.Coffees.Controllers;

[Area("Coffee")]
[Route("[area]/[controller]")]
public class EditorController(IMessageBus bus) : ApiKeyController
{
    [HttpPost()]
    [ProducesResponseType(typeof(CoffeeResponse), 200)]
    public async Task<ActionResult<CoffeeResponse>> Create([FromBody] CreateCoffeeRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeResponse>(new CommandCreateCoffee(request.Name, request.RoasterId));
        return Ok(responses);
    }
    
    [HttpPut("name")]
    [ProducesResponseType(typeof(CoffeeUpdated), 200)]
    public async Task<ActionResult<CoffeeUpdated>> UpdateName([FromBody] UpdateCoffeeNameRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeUpdated>(new CommandUpdateCoffeeName(request.Id, request.Name));
        return Ok(responses);
    }
    
    [HttpPut("roaster")]
    [ProducesResponseType(typeof(CoffeeUpdated), 200)]
    public async Task<ActionResult<CoffeeUpdated>> UpdateRoaster([FromBody] UpdateCoffeeRoasterRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeUpdated>(new CommandUpdateCoffeeRoaster(request.Id, request.RoasterId));
        return Ok(responses);
    }
    
    [HttpPut("links")]
    [ProducesResponseType(typeof(CoffeeUpdated), 200)]
    public async Task<ActionResult<CoffeeUpdated>> UpdateLinks([FromBody] UpdateCoffeeLinksRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeUpdated>(new CommandUpdateCoffeeLinks(request.Id, request.Urls));
        return Ok(responses);
    }
    
    [HttpPut("description")]
    [ProducesResponseType(typeof(CoffeeUpdated), 200)]
    public async Task<ActionResult<CoffeeUpdated>> UpdateDescription([FromBody] UpdateCoffeeNameRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeUpdated>(new CommandUpdateCoffeeDescription(request.Id, request.Name));
        return Ok(responses);
    }
}