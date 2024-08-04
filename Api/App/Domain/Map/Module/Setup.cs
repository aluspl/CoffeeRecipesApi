namespace Api.App.Domain.Map.Module;

public static class Setup
{
    public static IEndpointRouteBuilder SetupMapModule(
        this IEndpointRouteBuilder app)
    {
        app.MapEndpoint();
        return app;
    }
}