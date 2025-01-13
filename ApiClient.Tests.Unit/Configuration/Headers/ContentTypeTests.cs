using ApiClient.Configuration.Headers;
using JetBrains.Annotations;

namespace ApiClient.Tests.Unit.Configuration.Headers;

[TestSubject(typeof(ContentType))]
public class ContentTypeTests
{
    public const string JSON_MIME_TYPE = "application/json";
    public const string XML_MIME_TYPE = "application/xml";
    public const string JAVASCRIPT_MIME_TYPE = "text/javascript";

    [Fact]
    public void ContentType_UsingStaticFactoryMethodJson_ReturnsExpectedMimeType()
    {
        var expectedMimeType = JSON_MIME_TYPE;

        var result = ContentType.Json();

        Assert.Equal(expectedMimeType, result.Value);
    }

    [Fact]
    public void ContentType_UsingStaticFactoryMethodJavaScript_ReturnsExpectedMimeType()
    {
        var expectedMimeType = JAVASCRIPT_MIME_TYPE;

        var result = ContentType.JavaScript();

        Assert.Equal(expectedMimeType, result.Value);
    }

    [Fact]
    public void ContentType_UsingStaticFactoryMethodXml_ReturnsExpectedMimeType()
    {
        var expectedMimeType = XML_MIME_TYPE;

        var result = ContentType.Xml();

        Assert.Equal(expectedMimeType, result.Value);
    }

    [Theory]
    [InlineData("custom/mime-type")]
    [InlineData("custom/mime-type; charset=UTF-8")]
    [InlineData("custom/mime-type+json")]
    [InlineData("custom/x-mime-type")]
    public void ContentType_UsingConstructorCustomMimeTypeWithValidInput_ReturnsCustomMimeType(
        string customMimeType
    )
    {
        var result = new ContentType(customMimeType);

        Assert.Equal(customMimeType, result.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ContentType_UsingConstructorCustomMimeTypeWithInvalidInput_ThrowsArgumentError(
        string? customMimeType
    )
    {
        Exception? exception = Record.Exception(() => new ContentType(customMimeType!));

        Assert.NotNull(exception);
        Assert.IsAssignableFrom<ArgumentException>(exception);
    }
}
