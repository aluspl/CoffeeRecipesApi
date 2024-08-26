using Api.App.Common.Exceptions;
using Api.App.Domain.Media.Handlers;
using Api.App.Domain.Media.Handlers.Commands;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Api.Tests.Domains.File;

[Collection("integration")]
public class FileSetter(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Add_Image()
    {
        // Assert
        var imageFile = CreateImage(1200, 1200);
        var command = new UploadFileModel(Guid.NewGuid(), "png", imageFile);
        var setter = Host.Services.GetService(typeof(IFileSetter)) as App.Domain.Media.Setter.FileSetter;
        setter.ShouldNotBeNull();

        // Act
        var result = await setter.Upload(command);

        // Assert
        result.ShouldNotBeNull();
        result.ImageUrl.ShouldNotBeNull();
        result.ThumbnailUrl.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Add_Image_Twice()
    {
        // Assert
        var imageFile = CreateImage(1200, 1200);
        var command = new UploadFileModel(Guid.NewGuid(), "png", imageFile);
        var setter = Host.Services.GetService(typeof(IFileSetter)) as App.Domain.Media.Setter.FileSetter;
        setter.ShouldNotBeNull();
        // Act

        var result = await setter.Upload(command);

        // Assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Not_Add_Image_When_Image_Too_Small()
    {
        // Assert
        var imageFile = CreateImage(700, 700);
        var command = new UploadFileModel(Guid.NewGuid(), "png", imageFile);
        var setter = Host.Services.GetService(typeof(IFileSetter)) as App.Domain.Media.Setter.FileSetter;
        setter.ShouldNotBeNull();

        // Act
        await Assert.ThrowsAsync<BusinessException>(async () => await setter.Upload(command));
    }

    private static Stream CreateImage(int width, int height)
    {
        var memory = new MemoryStream();
        var image = new Image<Rgba64>(width, height);
        image.SaveAsPng(memory);
        return memory;
    }
}