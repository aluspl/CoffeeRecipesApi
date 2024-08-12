using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers.Commands;
using Api.App.Domain.Map.Handlers.Queries;
using Api.App.Domain.Map.Models.Responses;
using Api.Tests.Consts;
using JasperFx.Core.Reflection;
using Lamar;
using Marten;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Map;

[Collection("integration")]
public class CityTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Add_City_To_Selected_Province()
    {
        var province = await SeedProvince();
        var command = new CommandInsertCity(ProvinceConsts.SampleCity.Name, province.Id);

        var tracked = await Host.InvokeMessageAndWaitAsync(command);
        var result = tracked.FindSingleTrackedMessageOfType<CityResponse>();

        result.ShouldNotBeNull();
        await using var nested = Host.Services.As<IContainer>().GetNestedContainer();
        var context = nested.GetInstance<IDocumentSession>();

        var item = await context.Query<City>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Query_For_City()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var query = new QueryCityList(province.Id);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<CityResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var cityResponse = result.FirstOrDefault();
        cityResponse.ShouldNotBeNull();
        cityResponse.ProvinceId.ShouldBe(province.Id);
        cityResponse.Id.ShouldBe(city.Id);
        cityResponse.Name.ShouldBe(city.Name);
        
        var item = await Store.QuerySession().Query<City>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(city.Id);
        item.Id.ShouldBe(cityResponse.Id);
        item.Name.ShouldBe(city.Name);
        item.Name.ShouldBe(cityResponse.Name);
    }
}