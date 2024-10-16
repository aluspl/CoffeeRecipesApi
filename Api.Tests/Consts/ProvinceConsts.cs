using Api.App.Domain.Map.Entities;

namespace Api.Tests.Consts;

public static class ProvinceConsts
{
    public const string ProvinceName = "Slask";

    public static City SampleCity { get; } = new City()
    {
        Id = Guid.NewGuid(),
        Name = "Zywiec",
        Province = "Śląsk",
    };
}