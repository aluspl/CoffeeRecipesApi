namespace Api.App.Media.Utils;

public class ContentTypeHelper
{
    private static readonly Dictionary<string, string> MimeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { ".txt", "text/plain" },
        { ".html", "text/html" },
        { ".htm", "text/html" },
        { ".css", "text/css" },
        { ".js", "application/javascript" },
        { ".json", "application/json" },
        { ".xml", "application/xml" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".gif", "image/gif" },
        { ".bmp", "image/bmp" },
        { ".webp", "image/webp" },
        { ".ico", "image/x-icon" },
        { ".svg", "image/svg+xml" },
        { ".pdf", "application/pdf" },
        { ".zip", "application/zip" },
        { ".rar", "application/x-rar-compressed" },
        { ".tar", "application/x-tar" },
        { ".7z", "application/x-7z-compressed" },
        { ".mp3", "audio/mpeg" },
        { ".wav", "audio/wav" },
        { ".ogg", "audio/ogg" },
        { ".mp4", "video/mp4" },
        { ".mov", "video/quicktime" },
        { ".avi", "video/x-msvideo" },
        { ".mkv", "video/x-matroska" },
    };

    public static string GetContentType(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));
        }

        var extension = System.IO.Path.GetExtension(fileName);

        return MimeMappings.GetValueOrDefault(extension, "application/octet-stream");
    }
}