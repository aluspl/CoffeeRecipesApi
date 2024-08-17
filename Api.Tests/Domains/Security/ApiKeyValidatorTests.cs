using Api.App.Domain.Security.Handlers;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Security;

[Collection("integration")]
public class ApiKeyValidatorTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Theory]
    [InlineData("2137", true)]
    [InlineData("a2137", false)]
    public async Task Should_Allow_Api_Key(string key, bool expected)
    {
        // Assign
        var query = new QueryApiKey(key);
        
        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<ApiKeyChecked>(query);
        
        // Assert
        var result = tracked.Item2;
        result.ShouldNotBeNull();
        result.HasAccess.ShouldBe(expected);
    }
}