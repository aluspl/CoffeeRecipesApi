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
    
    [HttpPost("many")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult> CreateMany([FromBody] ICollection<CreateCoffeeRoasterRequest> request)
    {
        foreach (var roasterRequest in request)
        {
            await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandCreateCoffeeRoaster(roasterRequest.Name, roasterRequest.CityId, roasterRequest.Founded));
        }
        return Ok();
    }
    
    [HttpPut("name")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateName([FromBody] UpdateCoffeeRoasterNameRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandUpdateRoasterName(request.Id, request.Name));
        return Ok(responses);
    }
    
    [HttpPut("city")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateCity([FromBody] UpdateCoffeeRoasterCityRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandUpdateRoasterCity(request.Id, request.CityId));
        return Ok(responses);
    }
    
    [HttpPut("links")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateLinks([FromBody] UpdateCoffeeRoasterLinksRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandUpdateRoasterLinks(request.Id, request.Urls));
        return Ok(responses);
    }
}