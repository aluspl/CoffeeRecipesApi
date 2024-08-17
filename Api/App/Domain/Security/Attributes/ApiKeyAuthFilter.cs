using Api.App.Common.Consts;
using Api.App.Domain.Security.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wolverine;

namespace Api.App.Domain.Security.Attributes;

public class ApiKeyAuthFilter(IMessageBus messageBus) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string userApiKey = context.HttpContext.Request.Headers[ApiKeyConstants.ApiKeyHeaderName].ToString();
        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var value = await messageBus.InvokeAsync<ApiKeyChecked>(new QueryApiKey(userApiKey));
        if (!value.HasAccess)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}