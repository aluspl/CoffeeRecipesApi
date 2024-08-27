namespace Api.App.Infrastructure.Database.Entities;

public class UrlDetail(string url, string description)
{
    public string Url { get; set; } = url;

    public string Description { get; set; } = description;
}
