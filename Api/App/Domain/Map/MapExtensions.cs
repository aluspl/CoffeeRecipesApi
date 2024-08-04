using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Map;

public static class MapExtensions
{
    public static IEndpointRouteBuilder MapEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/province/list", async ([FromServices] IMessageBus bus) =>
            {
                var provinces = await bus.InvokeAsync<IEnumerable<ProvinceResponse>>(new ProvinceQuery());
                return provinces;
            })
            .WithName("Get All Provinces")
            .WithOpenApi();
        
        endpoints.MapGet("/city/list", async ([FromServices] IMessageBus bus) =>
            {
                var provinces = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCity(null));
                return provinces;
            })
            .WithName("Get All Cities")
            .WithOpenApi();
        
        endpoints.MapGet("/city/list/{{provinceId:guid}}", async (Guid provinceId, [FromServices] IMessageBus bus) =>
            {
                var provinces = await bus.InvokeAsync<IEnumerable<CityResponse>>(new QueryCity(provinceId));
                return provinces;
            })
            .WithName("Get Cities By Province Id")
            .WithOpenApi();

        return endpoints;
    }
}