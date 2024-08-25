using Api.App.Common.Exceptions;
using Api.App.Domain.Media.Handlers.Results;
using Api.App.Media.Handlers.Commands;
using Marten;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Wolverine.Tracking;

namespace Api.Tests.Domains.File;

[Collection("integration")]
public class CommandFileUploadTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Should_Add_Image()
    {
        // Assert
        var imageFile = CreateImage(1200, 1200);
        var command = new CommandUploadRoasterFile(Guid.NewGuid(), "png", imageFile);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<FileUploaded>(command);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        status.Status.ShouldBe(TrackingStatus.Completed);
        var item = await Store
            .QuerySession()
            .Query<App.Domain.Media.Entity.CoverFile>()
            .Where(p => p.Id == result.Id)
            .FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Name.ShouldContain(command.Id.ToString());
    }

    [Fact]
    public async Task Should_Add_Image_Twice()
    {
        // Assert
        var imageFile = CreateImage(1200, 1200);
        var command = new CommandUploadRoasterFile(Guid.NewGuid(), "png", imageFile);

        // Act
        var tracked = await Host.InvokeMessageAndWaitAsync<FileUploaded>(command);
        var status = tracked.Item1;
        var result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        status.Status.ShouldBe(TrackingStatus.Completed);
        var item = await Store
            .QuerySession()
            .Query<App.Domain.Media.Entity.CoverFile>()
            .Where(p => p.Id == result.Id)
            .FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Name.ShouldContain(command.Id.ToString());

        tracked = await Host.InvokeMessageAndWaitAsync<FileUploaded>(command);
        result = tracked.Item2;

        // Assert
        result.ShouldNotBeNull();
        status.Status.ShouldBe(TrackingStatus.Completed);
        item = await Store
            .QuerySession()
            .Query<App.Domain.Media.Entity.CoverFile>()
            .Where(p => p.Id == result.Id)
            .FirstOrDefaultAsync();
        item.ShouldNotBeNull();
        item.Name.ShouldContain(command.Id.ToString());
    }

    [Fact]
    public async Task Should_Not_Add_Image_When_Image_Too_Small()
    {
        // Assert
        var imageFile = CreateImage(700, 700);
        var command = new CommandUploadRoasterFile(Guid.NewGuid(), "png", imageFile);

        // Act
        await Assert.ThrowsAsync<BusinessException>(async () =>
            await Host.InvokeMessageAndWaitAsync<FileUploaded>(command));
    }

    private static byte[] CreateImage(int width, int height)
    {
        using var memory = new MemoryStream();
        var image = new Image<Rgba64>(width, height);
        image.SaveAsPng(memory);
        return memory.ToArray();
    }
}