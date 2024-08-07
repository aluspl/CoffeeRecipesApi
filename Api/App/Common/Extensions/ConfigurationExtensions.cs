namespace Api.App.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T GetConfig<T>(this IConfigurationManager configurationManager, string name) where T : class, new()
    {
        T settings = new T();
        var section = configurationManager.GetSection(name);
        section.Bind(settings);
        return settings;
    }
}