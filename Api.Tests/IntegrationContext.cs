﻿using Alba;
using Api.App.Domain.Map.Entities;
using Api.Tests.Consts;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public abstract class IntegrationContext : IAsyncLifetime
{
    protected IntegrationContext(AppFixture fixture)
    {
        Host = fixture.Host;
        Store = Host.Services.GetRequiredService<IDocumentStore>();
    }
     
    public IAlbaHost Host { get; }
    public IDocumentStore Store { get; }
     
    public async Task InitializeAsync()
    {
        // Using Marten, wipe out all data and reset the state
        await Store.Advanced.ResetAllData();
    }
 
    // This is required because of the IAsyncLifetime 
    // interface. Note that I do *not* tear down database
    // state after the test. That's purposeful
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    protected async Task<Province> SeedProvince()
    {
        await using var documentSession = Store.LightweightSession();
        var entity = new Province()
        {
            Name = ProvinceConsts.SampleProvince.Name,
        };
        
        documentSession.Store(entity);
        await documentSession.SaveChangesAsync();
        return entity;
    }
    
    protected async Task<City> SeedCity(Guid provinceId)
    {
        await using var session = Store.LightweightSession();
        var entity = new City()
        {
            ProvinceId = provinceId,
            Name = ProvinceConsts.SampleCity.Name,
        };
        session.Store(entity);
        await session.SaveChangesAsync();
        return entity;
    }
}