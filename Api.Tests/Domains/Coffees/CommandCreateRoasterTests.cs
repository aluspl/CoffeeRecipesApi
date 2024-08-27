﻿using Api.App.Common.Exceptions;
using Api.App.Domain.Roaster.Entities;
using Api.App.Domain.Roaster.Handlers.Commands;
using Api.App.Domain.Roaster.Models;
using Api.App.Infrastructure.Database.Entities;
using Marten;
using Shouldly;
using Wolverine.Tracking;

namespace Api.Tests.Domains.Coffees;

[Collection("integration")]
public class CommandCreateCoffeeTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Add_Roaster()
    {
        // Assert
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
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
        var province = await SeedProvince();
        var city = await SeedCity(province.Id);
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
    private async Task<CoffeeRoaster> SeedRoaster(Guid cityId)
    {
        await using var session = Store.LightweightSession();
        var entity = new CoffeeRoaster()
        {
            CityId = cityId,
            Name = "Pope Roaster",
            Urls = new List<UrlDetail>()
            {
                new UrlDetail("https://2137.it", "Main Page"),
                new UrlDetail("https://vatican.it", "Common Page")
            }
        };
        session.Store(entity);
        await session.SaveChangesAsync();
        return entity;
    }
}