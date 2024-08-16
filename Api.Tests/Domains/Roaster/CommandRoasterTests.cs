using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Marten;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Roaster;

[Collection("integration")]
public class CommandRoasterTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Add_Roaster()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var founded = DateTime.Now;
        var command = new CommandCreateCoffeeRoaster("Pope Roaster", city.Id, founded);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(command);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        status.Status.ShouldBe(TrackingStatus.Completed);
        
        result.ShouldNotBeNull();
        result.CityId.ShouldBe(city.Id);
        result.Name.ShouldBe(command.Name);
        result.Founded.ShouldBe(command.Founded);
        
        var item = await Store.QuerySession().Query<CoffeeRoaster>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(result.Id);
    }
}