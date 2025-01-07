using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiClient.Services;
using JetBrains.Annotations;
using NSubstitute;

namespace ApiClient.Tests.Unit.Services;

[TestSubject(typeof(ExternalHttpService))]
public class ExternalHttpServiceTests
{
    private IExternalHttpService _sut;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public ExternalHttpServiceTests()
    {
        _httpClientWrapper = Substitute.For<IHttpClientWrapper>();
        _sut = new ExternalHttpService(_httpClientWrapper);
    }
    
    public record TestData([property: JsonRequired] string Id, [property: JsonRequired] bool ShouldPass);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GetAsync_WithAnInvalidUrl_ThrowsArgumentException(string invalidUrl)
    {
        var ex = await Record.ExceptionAsync(() => _sut.GetAsync<object>(invalidUrl));

        Assert.IsType<ArgumentException>(ex);
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetAsync_WithAnUnsuccessfulResponse_ThrowsHttpRequestException(
        HttpStatusCode statusCode
    )
    {
        _httpClientWrapper.GetAsync(Arg.Any<string>()).Returns(new HttpResponseMessage(statusCode));

        var ex = await Record.ExceptionAsync(
            () => _sut.GetAsync<object>("https://valid.test-url.com")
        );

        Assert.IsType<HttpRequestException>(ex);
    }

    [Fact]
    public async Task GetAsync_WithValidResponse_ReturnsCorrectObject()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{"Id":"TestID","ShouldPass":true}""", System.Text.Encoding.UTF8, "application/json")
        };
        _httpClientWrapper.GetAsync(Arg.Any<string>()).Returns(response);
        var expected = new TestData("TestID", true);

        var result = await _sut.GetAsync<TestData>("https://valid.test-url.com");
        
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public async Task GetAsync_WithIncorrectResponse_ThrowsJsonParseException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{"Testid":"TestID","ShouldPass":false}""", System.Text.Encoding.UTF8, "application/json")
        };
        _httpClientWrapper.GetAsync(Arg.Any<string>()).Returns(response);

        var result = await Record.ExceptionAsync(() => _sut.GetAsync<TestData>("https://valid.test-url.com"));

        Assert.IsType<JsonException>(result);
    }
}

