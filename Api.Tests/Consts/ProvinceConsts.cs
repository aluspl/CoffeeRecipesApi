using Api.App.Domain.Map.Entities;

namespace Api.Tests.Consts;

public static class ProvinceConsts
{
    public static Province SampleProvince = new Province()
    {
        Id = Guid.NewGuid(),
        Name = "Sample",
    };
}