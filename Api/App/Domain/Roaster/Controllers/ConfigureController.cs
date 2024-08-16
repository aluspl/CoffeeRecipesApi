using Api.App.Common.Controller;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Api.App.Domain.Roaster.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Roaster.Controllers;

[Area("Roasters")]
[Route("[area]/[controller]")]
public class ConfigureController(IMessageBus bus) : ApiController
{
    [HttpPost()]
    [ProducesResponseType(typeof(CoffeeRoasterResponse), 200)]
    public async Task<ActionResult<CoffeeRoasterResponse>> Create(CoffeeRoasterRequest request)
    {
        var responses = await bus.InvokeAsync<CoffeeRoasterResponse>(new CommandCreateCoffeeRoaster(request.Name, request.CityId, request.Founded));
        return Ok(responses);
    }
}