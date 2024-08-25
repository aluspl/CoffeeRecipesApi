using Api.App.Domain.Media.Enum;

namespace Api.App.Common.Configs;

public class MediaSettings
{
    public const string Section = "Media";

    public string ConnectionStrings { get; set; }
    
    public IDictionary<ImageType, string> Folders { get; set; }
    
    public int MinimalWidth { get; set; }
 
    public int MinimalHeight { get; set; }
        
    public int ThumbnailWidth { get; set; }
 
    public int ThumbnailHeight { get; set; }
}