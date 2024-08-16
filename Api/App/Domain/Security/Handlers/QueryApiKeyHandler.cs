using Api.App.Common.Consts;

namespace Api.App.Domain.Security.Handlers;

public class QueryApiKeyHandler()
{
    public static bool Handle(QueryApiKey query, IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(query.ApiKey))
        {
            return false;
        }

        string apiKey = configuration.GetValue<string>(ApiKeyConstants.ApiKeyName);
        if (apiKey == null || query.ApiKey != apiKey)
        {
            return false;
        }

        return true;
    }
}