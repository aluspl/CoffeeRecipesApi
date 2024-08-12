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
public class ProvinceTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Query_For_Provinces_return_provinces_with_City()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var query = new QueryProvinceList();

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<IEnumerable<ProvinceResponse>>(query);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(1);
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        var provinceResponse = result.FirstOrDefault();
        provinceResponse.ShouldNotBeNull();
        provinceResponse.Id.ShouldBe(province.Id);
        provinceResponse.Cities.ShouldNotBeNull();
        provinceResponse.Cities.ShouldNotBeEmpty();

        var item = await Store.QuerySession().Query<Province>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(province.Id);
        item.Id.ShouldBe(provinceResponse.Id);
    }
}