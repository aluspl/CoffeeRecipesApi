using Api.App.Common.Controller;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Api.App.Domain.Roaster.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Wolverine;

namespace Api.App.Domain.Roaster.Controllers;

[Area("Roasters")]
[Route("[area]/[controller]")]
public class EditorController(IMessageBus bus) : ApiKeyController
{
    [HttpPost()]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> Create([FromBody] CreateCoffeeRoasterRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandCreateCoffeeRoaster(request.Name, request.CityId));
        return Ok(responses);
    }
    
    [HttpPost("many")]
    [ProducesResponseType(typeof(ICollection<CoffeeRoasterResponse>), 200)]
    public async Task<ActionResult> CreateMany([FromBody] ICollection<CreateCoffeeRoasterRequest> request)
    {
        var results = new List<CoffeeRoasterResponse>();
        foreach (var roasterRequest in request)
        {
            try
            {
                await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandCreateCoffeeRoaster(roasterRequest.Name, roasterRequest.CityId));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, $"Create Many Result {roasterRequest.Name}");
            }
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
    
    [HttpPut("description")]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateDescription([FromBody] UpdateCoffeeRoasterNameRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandUpdateRoasterDescription(request.Id, request.Name));
        return Ok(responses);
    }
}