using System.Configuration;
using Api.App.Common.Configs;

namespace Api.App.Common.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddOptions<MartenSettings>()
            .Bind(configuration.GetSection(MartenSettings.Section))
            .ValidateDataAnnotations();
        services.AddOptions<MediaSettings>()
            .Bind(configuration.GetSection(MediaSettings.Section))
            .ValidateDataAnnotations();

        return services;
    }

    public static T GetConfig<T>(this IConfigurationManager configurationManager, string name) where T : class, new()
    {
        T settings = new T();
        var section = configurationManager.GetSection(name);
        section.Bind(settings);
        return settings;
    }
}