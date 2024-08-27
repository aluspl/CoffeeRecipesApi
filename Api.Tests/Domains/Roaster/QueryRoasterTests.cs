using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Marten;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Roaster;

[Collection("integration")]
public class QueryRoasterTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Query_All_Roasters()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        
        var query = new QueryRoasterList(null, null, false);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<CoffeeRoasterResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var roasterResponse = result.FirstOrDefault();
        roasterResponse.ShouldNotBeNull();
        roasterResponse.CityId.ShouldBe(city.Id);
        roasterResponse.Name.ShouldBe(roaster.Name);
        roasterResponse.Founded.ShouldBe(roaster.Founded);
        roasterResponse.Urls.All(p => roaster.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<CoffeeRoaster>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(roaster.Id);
        item.Id.ShouldBe(roasterResponse.Id);
    }

    [Fact]
    public async Task Should_Query_Roasters_By_City()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        
        var query = new QueryRoasterList(city.Id, null, false);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<CoffeeRoasterResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var roasterResponse = result.FirstOrDefault();
        roasterResponse.ShouldNotBeNull();
        roasterResponse.CityId.ShouldBe(city.Id);
        roasterResponse.Name.ShouldBe(roaster.Name);
        roasterResponse.Founded.ShouldBe(roaster.Founded);
        roasterResponse.Urls.All(p => roaster.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<CoffeeRoaster>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(roaster.Id);
        item.Id.ShouldBe(roasterResponse.Id);
    }
    
    [Fact]
    public async Task Should_Query_Roasters_By_Name()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        
        var query = new QueryRoasterList(city.Id, roaster.Name, false);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<CoffeeRoasterResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var roasterResponse = result.FirstOrDefault();
        roasterResponse.ShouldNotBeNull();
        roasterResponse.CityId.ShouldBe(city.Id);
        roasterResponse.Name.ShouldBe(roaster.Name);
        roasterResponse.Founded.ShouldBe(roaster.Founded);
        roasterResponse.Urls.All(p => roaster.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<CoffeeRoaster>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(roaster.Id);
        item.Id.ShouldBe(roasterResponse.Id);
    }
    
    [Fact]
    public async Task Should_Query_Roaster_Detail()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        
        var query = new QueryRoasterDetail(roaster.Id);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;


        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();
        result.CityId.ShouldBe(city.Id);
        result.Name.ShouldBe(roaster.Name);
        result.Founded.ShouldBe(roaster.Founded);
        result.Urls.All(p => roaster.Urls.Any(o => o.Url == p.Url)).ShouldBeTrue();
        
        var item = await Store.QuerySession().Query<CoffeeRoaster>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(roaster.Id);
        item.Id.ShouldBe(result.Id);
    }
}