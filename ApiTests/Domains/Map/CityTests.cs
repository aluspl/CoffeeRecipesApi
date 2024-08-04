using Api.App.Domain.Map.Controllers;
using Api.App.Domain.Map.Entities;
using Api.App.Domain.Map.Handlers;
using Marten;
using NSubstitute;
using Shouldly;

namespace ApiTests.Domains.Map;

public class CityTests : IAsyncLifetime
{
    private readonly IDocumentSession _documentSession = Substitute.For<IDocumentSession>();
    private readonly Province _sampleProvince = new Province()
    {
        Id = Guid.NewGuid(),
        Name = "Sample",
    };

    public async Task InitializeAsync()
    {
        _documentSession.Store(_sampleProvince);
        await _documentSession.SaveChangesAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    [Fact]
    public async Task Should_Add_City_To_Selected_Province()
    {
        var command = new CommandInsertCity("Zywiec", _sampleProvince.Id);
        var city = await InsertCityHandler.HandleAsync(command, _documentSession);
        city.ShouldNotBeNull();
        city.Id.ShouldNotBe(Guid.Empty);
        city.ProvinceId.ShouldBe(_sampleProvince.Id);
    }
}