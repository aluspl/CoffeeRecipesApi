﻿using Api.App.Common.Exceptions;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
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
        var tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(cityCommand);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();
        result.CityId.ShouldBe(city.Id);

        // Assign
        var nameCommand = new CommandUpdateRoasterName(roaster.Id, "");

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(nameCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();
        result.Name.ShouldBe(nameCommand.Name);
        
        // Assign
        var urlsCommand = new CommandUpdateRoasterLinks(roaster.Id, [
            "https://2137.vat",
            "https://test.test"
        ]);

        // Act
        tracked = await Host.InvokeMessageAndWaitAsync<CoffeeRoasterResponse>(urlsCommand);
        status = tracked.Item1;
        result = tracked.Item2;

        // Assert
        status.Status.ShouldBe(TrackingStatus.Completed);
        result.ShouldNotBeNull();
        result.Urls.ShouldNotBeNull();
        result.Urls.ShouldNotBeEmpty();
        result.Urls.Count().ShouldBe(urlsCommand.Urls.Count);
        result.Urls.ShouldBe(urlsCommand.Urls.Select(p => new Uri(p)));
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
    
    private async Task<CoffeeRoaster> SeedRoaster(Guid cityId)
    {
        await using var session = Store.LightweightSession();
        var entity = new CoffeeRoaster()
        {
            CityId = cityId,
            Name = "Pope Roaster",
            Urls = new List<Uri>()
            {
                new Uri("https://2137.it"),
                new Uri("https://vatican.it")
            }
        };
        session.Store(entity);
        await session.SaveChangesAsync();
        return entity;
    }
}