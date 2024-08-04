using Api.App.Common.Controller;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Map.Controllers;

[Area("map")]
[Route("[area]/[controller]")]
public class CityController(IMessageBus bus) : ApiController
{
    [HttpGet("list")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
    {
        try
        {
            var cities = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCity(null));
            return Ok(cities);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("list/{provinceId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), 200)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCitiesByProvinceId(Guid provinceId)
    {
        try
        {
            if (provinceId == Guid.Empty)
            {
                return BadRequest("Invalid province ID");
            }

            var cities = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCity(provinceId));
            return Ok(cities);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "Internal server error");
        }
    }
}