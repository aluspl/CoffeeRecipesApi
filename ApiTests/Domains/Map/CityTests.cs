using Alba;
using Api.App.Domain.Map.Controllers;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Models.Responses;
using JasperFx.Core.Reflection;
using Lamar;
using Marten;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;
using Oakton;
using Shouldly;
using Wolverine.Tracking;

namespace ApiTests.Domains.Map;

public class CityTests
{
    private readonly IDocumentSession _documentSession = Substitute.For<IDocumentSession>();

    private readonly Province _sampleProvince = new Province()
    {
        Id = Guid.NewGuid(),
        Name = "Sample",
    };

    public CityTests()
    {
        OaktonEnvironment.AutoStartHost = true;
    }

    private async Task SeedProvince()
    {
        _documentSession.Store(_sampleProvince);
        await _documentSession.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_Add_City_To_Selected_Province()
    {
        await using var host = await AlbaHost.For<Program>();

        await SeedProvince();
        var command = new CommandInsertCity("Zywiec", _sampleProvince.Id);
        var tracked = await host.InvokeMessageAndWaitAsync(command);
        var result = tracked.FindSingleTrackedMessageOfType<CityResponse>();
        result.ShouldNotBeNull();
        await using var nested = host.Services.As<IContainer>().GetNestedContainer();
        var context = nested.GetInstance<IDocumentSession>();

        var item = await context.Query<City>().FirstOrDefaultAsync();
        item.ShouldNotBeNull();
    }
}