using Api.App.Domain.Media.Enum;

namespace Api.App.Domain.Media.Config;

public class MediaConfig
{
    public const string Section = "Media";
 
    public IDictionary<ImageType, string> Folders { get; set; }
}