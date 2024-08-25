using Api.App.Domain.Media.Getter;
using Api.App.Domain.Media.Provider;
using Api.App.Media.Interfaces.Provider;

namespace Api.App.Domain.Media.Modules;

public static class Setup
{
    public static IServiceCollection SetupMediaModule(this IServiceCollection app)
    {
        app.AddScoped<IBlobProvider, AzureBlobProvider>();
        app.AddScoped<IFileGetter, FileGetter>();
        // Add Cache system here !! 
        return app;
    }
}