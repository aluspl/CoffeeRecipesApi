using Api.App.Media.Utils;

namespace Api.Tests.Domains.File.Utils;

public class ContentTypeHelperTests
{
    [Theory]
    [InlineData("example.txt", "text/plain")]
    [InlineData("document.html", "text/html")]
    [InlineData("style.css", "text/css")]
    [InlineData("script.js", "application/javascript")]
    [InlineData("data.json", "application/json")]
    [InlineData("image.jpg", "image/jpeg")]
    [InlineData("image.jpeg", "image/jpeg")]
    [InlineData("image.png", "image/png")]
    [InlineData("archive.zip", "application/zip")]
    [InlineData("video.mp4", "video/mp4")]
    public void GetContentType_ValidExtension_ReturnsCorrectMimeType(string fileName, string expectedMimeType)
    {
        // Act
        string actualMimeType = ContentTypeHelper.GetContentType(fileName);

        // Assert
        Assert.Equal(expectedMimeType, actualMimeType);
    }

    [Theory]
    [InlineData("unknownfile.unknownextension")]
    [InlineData("filewithoutextension")]
    public void GetContentType_UnknownOrNoExtension_ReturnsOctetStream(string fileName)
    {
        // Act
        string actualMimeType = ContentTypeHelper.GetContentType(fileName);

        // Assert
        Assert.Equal("application/octet-stream", actualMimeType);
    }

    [Fact]
    public void GetContentType_NullFileName_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ContentTypeHelper.GetContentType(null));
    }

    [Fact]
    public void GetContentType_WhiteSpaceFileName_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ContentTypeHelper.GetContentType("   "));
        Assert.Throws<ArgumentException>(() => ContentTypeHelper.GetContentType(""));

    }
}