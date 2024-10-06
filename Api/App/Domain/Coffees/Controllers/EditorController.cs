using Api.App.Common.Controller;
using Api.App.Domain.Coffees.Handlers.Commands;
using Api.App.Domain.Coffees.Models;
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
    
    [HttpPost("many")]
    [ProducesResponseType(typeof(ICollection<CoffeeRoasterResponse>), 200)]
    public async Task<ActionResult> CreateMany([FromBody] ICollection<CreateCoffeeRequest> request)
    {
        var results = new List<CoffeeRoasterResponse>();
        foreach (var coffeeRequest in request)
        {
            try
            {
                await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandCreateCoffeeRoaster(coffeeRequest.Name, coffeeRequest.RoasterId));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, $"Create Many Result {coffeeRequest.Name}");
            }
        }
        return Ok(results);
    }
    
    [HttpPut("name")]
    [ProducesResponseType(typeof(CoffeeRoasterUpdated), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateName([FromBody] UpdateCoffeeRoasterNameRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterUpdated>(new CommandUpdateRoasterName(request.Id, request.Name));
        return Ok(responses);
    }
    
    [HttpPut("roaster")]
    [ProducesResponseType(typeof(CoffeeRoasterUpdated), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateRoaster([FromBody] UpdateCoffeeRoasterCityRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterUpdated>(new CommandUpdateRoasterCity(request.Id, request.CityId));
        return Ok(responses);
    }
    
    [HttpPut("links")]
    [ProducesResponseType(typeof(CoffeeRoasterUpdated), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateLinks([FromBody] UpdateCoffeeRoasterLinksRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterUpdated>(new CommandUpdateRoasterLinks(request.Id, request.Urls));
        return Ok(responses);
    }
    
    [HttpPut("description")]
    [ProducesResponseType(typeof(CoffeeRoasterUpdated), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> UpdateDescription([FromBody] UpdateCoffeeRoasterNameRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterUpdated>(new CommandUpdateRoasterDescription(request.Id, request.Name));
        return Ok(responses);
    }
}