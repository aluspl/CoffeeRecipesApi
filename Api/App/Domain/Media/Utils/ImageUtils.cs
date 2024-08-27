using Api.App.Common.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Api.App.Domain.Media.Utils;

public static class ImageUtils
{
    public static async Task ResizeImage(Stream stream)
    {
        using var image = await Image.LoadAsync(stream);
        var width = 800;
        var height = 800;
        
        image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));
        
        await image.SaveAsync(stream, PngFormat.Instance);
    }

    public static void ValidateImage(Stream stream)
    {
        stream.Position = 0;

        using var image = Image.Load(stream);
        if (image.Width != image.Height)
        {
            throw new BusinessException("Image should be Square");
        }
        
        if (image.Width < 1200)
        {
            throw new BusinessException($"Image Width and Height are smaller than 1200px");
        }
    }
}