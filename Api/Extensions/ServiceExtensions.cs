using System.ComponentModel;
using Api.App.Common.Converters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection UseSwagger(this IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.DescribeAllParametersInCamelCase();
        });

        return services;
    }

    public static IServiceCollection UseJson(this IServiceCollection services)
    {
        services.Configure<MvcNewtonsoftJsonOptions>(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });
        return services;
    }
}