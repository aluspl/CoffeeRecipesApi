using Alba;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Commands;
using Api.App.Domain.Map.Models.Responses;
using Api.Tests.Consts;
using JasperFx.Core.Reflection;
using Lamar;
using Marten;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Map;

[Collection("integration")]
public class CityTests : IntegrationContext
{
    private readonly IAlbaHost theHost;

    public CityTests(AppFixture fixture) : base(fixture)
    {
        theHost = fixture.Host;
    }

    [Fact]
    public async Task Should_Add_City_To_Selected_Province()
    {
        await SeedProvince();
        var command = new CommandInsertCity("Zywiec", ProvinceConsts.SampleProvince.Id);
        var tracked = await Host.InvokeMessageAndWaitAsync(command);
        var result = tracked.FindSingleTrackedMessageOfType<CityResponse>();
        result.ShouldNotBeNull();
        await using var nested = Host.Services.As<IContainer>().GetNestedContainer();
        var context = nested.GetInstance<IDocumentSession>();

        var item = await context.Query<City>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
    }
}