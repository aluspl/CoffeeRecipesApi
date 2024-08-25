using Api.App.Domain.Media.Interfaces.Provider;
using Api.App.Domain.Media.Provider;

namespace Api.App.Domain.Media.Modules;

public static class Setup
{
    public static IServiceCollection SetupMediaModule(this IServiceCollection app)
    {
        app.AddScoped<IBlobProvider, AzureBlobProvider>();

        return app;
    }
}