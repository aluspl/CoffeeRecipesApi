﻿using Api.App.Common.Exceptions;
using Api.App.Domain.Common.Models;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Handlers.Queries;
using Api.App.Domain.Roaster.Models;
using Api.App.Domain.Roaster.Models.Records;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Roaster;

[Collection("integration")]
public class CommandUpdateRoasterTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Update_Roaster()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        var cityCommand = new CommandUpdateRoasterCity(roaster.Id, city.Id);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterUpdated>(cityCommand);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();

        // Assign
        var nameCommand = new CommandUpdateRoasterName(roaster.Id, "");

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterUpdated>(nameCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();
        
        // Assign
        var urlsCommand = new CommandUpdateRoasterLinks(roaster.Id, [
            new UrlRequest("https://2137.vat", "Main Page"),
           new UrlRequest( "https://test.test", "Test Page")
        ]);

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterUpdated>(urlsCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();
        
        // Assign
        var descriptionCommand = new CommandUpdateRoasterDescription(roaster.Id, "Test");

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterUpdated>(descriptionCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();

        var responseTrack = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(new QueryRoasterDetail(roaster.Id));
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
        response.CityId.ShouldBe(city.Id);
    }
    
    [Fact]
    public async Task Should_Not_Update_Roaster_When_City_Not_Exists()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roaster = await SeedRoaster(city.Id);
        var command = new CommandUpdateRoasterCity(roaster.Id, Guid.NewGuid());

        // Act
        await Assert.ThrowsAsync<NotFoundException>(async () => await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(command));
    }
    
    [Fact]
    public async Task Should_Not_Update_Roaster_When_Name_Already_Exists()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
        var roasterOld = await SeedRoaster(city.Id);
        var roaster = await SeedRoaster(city.Id);

        var command = new CommandUpdateRoasterName(roaster.Id, roasterOld.Name);

        // Act
        await Assert.ThrowsAsync<BusinessException>(async () => await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(command));
    }
}