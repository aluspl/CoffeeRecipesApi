using Api.App.Domain.Map.Entities;

namespace Api.Tests.Consts;

public static class ProvinceConsts
{
    public static Province SampleProvince = new Province()
    {
        Id = Guid.NewGuid(),
        Name = "Slask",
    };

    public static City SampleCity { get; } = new City()
    {
        Id = Guid.NewGuid(),
        Name = "Zywiec",
        ProvinceId = SampleProvince.Id,
    };
}