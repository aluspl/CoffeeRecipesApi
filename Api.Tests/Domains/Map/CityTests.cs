using Api.App.Common.Enums;
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
        var command = new CommandInsertCity(ProvinceConsts.SampleCity.Name, ProvinceConsts.ProvinceName, CountryCodes.PL);

        var tracked = await Host.InvokeMessageAndWaitAsync(command);
        var result = tracked.FindSingleTrackedMessageOfType<CityResponse>();

        result.ShouldNotBeNull();
        await using var nested = Host.Services.As<IContainer>().GetNestedContainer();
        var context = nested.GetInstance<IDocumentSession>();

        var item = await context.Query<City>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Query_For_City_By_ProvinceId()
    {
        // Assert
        var city = await SeedCity(ProvinceName);
        var query = new QueryCityListByProvinceName(ProvinceName);

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
        cityResponse.Province.ShouldBe(ProvinceConsts.ProvinceName);
        cityResponse.Id.ShouldBe(city.Id);
        cityResponse.Name.ShouldBe(city.Name);
        
        var item = await Store.QuerySession().Query<City>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(city.Id);
        item.Id.ShouldBe(cityResponse.Id);
        item.Name.ShouldBe(city.Name);
        item.Name.ShouldBe(cityResponse.Name);
    }
    
    [Fact]
    public async Task Should_Query_For_City_by_Name()
    {
        // Assert
        var city = await SeedCity(ProvinceConsts.ProvinceName);
        var query = new QueryCityListByName(city.Name);

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
        cityResponse.Province.ShouldBe(ProvinceConsts.ProvinceName);
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