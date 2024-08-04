using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Api.App.Domain.Map;

public static class MapExtensions
{
    public static IEndpointRouteBuilder MapEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}