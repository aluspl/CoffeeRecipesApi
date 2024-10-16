using Api.App.Common.Exceptions;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Marten;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Roaster;

[Collection("integration")]
public class CommandCreateRoasterTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Add_Roaster()
    {
        // Assert
        var city = await SeedCity("Slask");
        var command = new CommandCreateCoffeeRoaster("Pope Roaster", city.Id);

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
        
        var item = await Store.QuerySession().Query<CoffeeRoaster>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Id.ShouldBe(result.Id);
    }
    
    [Fact]
    public async Task Should_Not_Add_Roaster_When_Name_Already_Exists()
    {
        var city = await SeedCity("Slask");
        var command = new CommandCreateCoffeeRoaster("Pope Roaster", city.Id);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(command);
        var status = tracked.Item1;
        var result = tracked.Item2;
        
        // Act
        await Assert.ThrowsAsync<BusinessException>(async () => await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(command));
    }
    
    [Fact]
    public async Task Should_Not_Add_Roaster_When_City_Not_Valid()
    {
        // Assert
        var command = new CommandCreateCoffeeRoaster("Pope Roaster", Guid.NewGuid());

        // Act
        await Assert.ThrowsAsync<NotFoundException>(async () => await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(command));
    }
}