using Api.App.Common.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Api.App.Media.Utils;

public static class ImageUtils
{
    public static async Task<byte[]> ResizeImage(byte[] commandFile)
    {
        using var image = Image.Load(commandFile);
        var width = 800;
        var height = 800;
        
        image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));
        
        using var memory = new MemoryStream();
        await image.SaveAsync(memory, PngFormat.Instance);
        return memory.ToArray();
    }

    public static void ValidateImage(byte[] commandFile)
    {
        using var image = Image.Load(commandFile);
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