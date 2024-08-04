using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Map.Controllers;

[Area("map")]
[Route("[area]/[controller]")]
public class ProvinceController(IMessageBus bus) : ControllerBase
{
    [HttpGet("list")]
    [ProducesResponseType(typeof(IEnumerable<ProvinceResponse>), 200)]
    public async Task<ActionResult<IEnumerable<ProvinceResponse>>> GetProvinces()
    {
        try
        {
            var provinces = await bus.InvokeAsync<IEnumerable<ProvinceResponse>>(new QueryProvince());
            return Ok(provinces);
        }
        catch (Exception ex)
        {
            // Log the exception (using a logging framework or similar)
            return StatusCode(500, "Internal server error");
        }
    }
}