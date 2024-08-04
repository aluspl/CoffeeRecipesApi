using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Map.Controllers;

[Area("Map")]
[Route("[area]/[controller]")]
public class ProvinceController(IMessageBus bus) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProvinceResponse>), 200)]
    public async Task<ActionResult<IEnumerable<ProvinceResponse>>> GetProvinces()
    {
        var provinces = await bus.InvokeAsync<IEnumerable<ProvinceResponse>>(new QueryProvince());
        return Ok(provinces);
    }
}