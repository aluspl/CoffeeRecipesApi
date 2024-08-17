using Api.App.Common.Consts;

namespace Api.App.Domain.Security.Handlers;

public class QueryApiKeyHandler()
{
    public static ApiKeyChecked Handle(QueryApiKey query, IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(query.ApiKey))
        {
            return new ApiKeyChecked(false);
        }

        string apiKey = configuration.GetValue<string>(ApiKeyConstants.ApiKeyName);
        if (apiKey == null || query.ApiKey != apiKey)
        {
            return new ApiKeyChecked(false);
        }

        return new ApiKeyChecked(true);
    }
}