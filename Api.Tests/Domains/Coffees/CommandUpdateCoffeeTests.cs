using Api.App.Common.Exceptions;
using Api.App.Domain.Coffees.Handlers.Commands;
using Api.App.Domain.Coffees.Handlers.Queries;
using Api.App.Domain.Coffees.Models;
using Api.App.Domain.Coffees.Models.Records;
using Api.App.Domain.Common.Models;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Coffees;

[Collection("integration")]
public class CommandUpdateCoffeeTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Update_Coffee()
    {
        // Assert
        var city = await SeedCity();

        var roaster = await SeedRoaster(city.Id);
        var coffee = await SeedCoffee(roaster.Id);
        var commandUpdateCoffeeRoaster = new CommandUpdateCoffeeRoaster(coffee.Id, roaster.Id);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<CoffeeUpdated>(commandUpdateCoffeeRoaster);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();

        // Assign
        var nameCommand = new CommandUpdateCoffeeName(coffee.Id, "Coffee 2");

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeUpdated>(nameCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();

        // Assign
        var urlsCommand = new CommandUpdateCoffeeLinks(coffee.Id, [
            new UrlRequest("https://2137.vat", "Main Page"),
            new UrlRequest("https://test.vat", "Test Page"),
        ]);

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeUpdated>(urlsCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();

        // Assign
        var descriptionCommand = new CommandUpdateCoffeeDescription(coffee.Id, "Test");

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeUpdated>(descriptionCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();

        var responseTrack = await Host.InvokeMessageAndWaitAsync<CoffeeResponse>(new QueryCoffeeDetail(coffee.Id));
        var response = responseTrack.Item2;
        response.ShouldNotBeNull();
        response.Name.ShouldBe(nameCommand.Name);
        response.Urls.ShouldNotBeNull();
        response.Urls.ShouldNotBeEmpty();
        response.Urls.Count().ShouldBe(urlsCommand.Urls.Count);
        response.Urls
            .Count()
            .ShouldBe(roaster.Urls.Count());
        response.Description[descriptionCommand.Language].ShouldBe(descriptionCommand.Description);
        response.RoasterId.ShouldBe(roaster.Id);
    }

    [Fact]
    public async Task Should_Not_Update_Roaster_When_City_Not_Exists()
    {
        // Assert
        var city = await SeedCity();

        var roaster = await SeedRoaster(city.Id);
        var coffee = await SeedCoffee(roaster.Id, "coffee 1");

        var command = new CommandUpdateCoffeeRoaster(coffee.Id, Guid.NewGuid());

        // Act
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await Host.InvokeMessageAndWaitAsync<CoffeeUpdated>(command));
    }

    [Fact]
    public async Task Should_Not_Update_Coffee_When_Name_Already_Exists()
    {
        // Assert
        var city = await SeedCity();

        var roaster = await SeedRoaster(city.Id);
        var coffeeOld = await SeedCoffee(roaster.Id, "coffee 1");
        var coffee = await SeedCoffee(roaster.Id, "coffee 1");

        var command = new CommandUpdateCoffeeName(coffee.Id, coffeeOld.Name);

        // Act
        await Assert.ThrowsAsync<BusinessException>(async () =>
            await Host.InvokeMessageAndWaitAsync<CoffeeUpdated>(command));
    }
}