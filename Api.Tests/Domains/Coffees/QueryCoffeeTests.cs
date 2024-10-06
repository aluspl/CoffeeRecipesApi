using Api.App.Domain.Coffees.Entities;
using Api.App.Domain.Coffees.Handlers.Queries;
using Api.App.Domain.Coffees.Models;
using Marten;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Coffees;

[Collection("integration")]
public class QueryCoffeeTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Query_All_Coffees()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        var coffee = await SeedCoffee(roaster.Id);

        var query = new QueryCoffeeList(null, null, false);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<CoffeeResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var response = result.FirstOrDefault();
        response.ShouldNotBeNull();
        response.RoasterId.ShouldBe(roaster.Id);
        response.Name.ShouldBe(coffee.Name);
        response.Urls.All(p => coffee.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<Coffee>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(coffee.Id);
        item.Id.ShouldBe(response.Id);
    }

    [Fact]
    public async Task Should_Query_Coffees_By_RoasterId()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        var coffee = await SeedCoffee(roaster.Id);

        var query = new QueryCoffeeList(roaster.Id, null, false);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<CoffeeResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var response = result.FirstOrDefault();
        response.ShouldNotBeNull();
        response.RoasterId.ShouldBe(roaster.Id);
        response.Name.ShouldBe(coffee.Name);
        response.Urls.All(p => coffee.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<Coffee>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(coffee.Id);
        item.Id.ShouldBe(response.Id);
    }
    
    [Fact]
    public async Task Should_Query_Coffee_By_Name()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        var coffee = await SeedCoffee(roaster.Id);

        var query = new QueryCoffeeList(city.Id, coffee.Name, false);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<CoffeeResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var response = result.FirstOrDefault();
        response.ShouldNotBeNull();
        response.RoasterId.ShouldBe(roaster.Id);
        response.Name.ShouldBe(coffee.Name);
        response.Urls.All(p => coffee.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<Coffee>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(coffee.Id);
        item.Id.ShouldBe(response.Id);
    }
    
    [Fact]
    public async Task Should_Query_Coffee_Detail()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        var coffee = await SeedCoffee(roaster.Id);

        var query = new QueryCoffeeDetail(coffee.Id);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<CoffeeResponse>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;


        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();
        result.RoasterId.ShouldBe(roaster.Id);
        result.Name.ShouldBe(coffee.Name);
        result.Urls.All(p => coffee.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<Coffee>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(coffee.Id);
        item.Id.ShouldBe(result.Id);
    }
}