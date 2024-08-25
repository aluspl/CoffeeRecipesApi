using Api.App.Domain.Map.Module;
using Api.App.Domain.Media.Modules;

namespace Api.App.Common.Extensions;

public static class ModuleExtensions
{
    public static IServiceCollection AddModules(this IServiceCollection provider)
    {
        // Setup Modules
        provider
            .SetupMapModule()
            .SetupMediaModule();

        return provider;
    }
}