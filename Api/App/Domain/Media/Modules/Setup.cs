﻿using Api.App.Domain.Media.Handlers;
using Api.App.Domain.Media.Provider;
using Api.App.Domain.Media.Setter;
using Api.App.Media.Interfaces.Provider;

namespace Api.App.Domain.Media.Modules;

public static class Setup
{
    public static IServiceCollection SetupMediaModule(this IServiceCollection app)
    {
        app.AddScoped<IBlobProvider, AzureBlobProvider>();
        app.AddScoped<IFileSetter, FileSetter>();

        // Add Cache system here !! 
        return app;
    }
}