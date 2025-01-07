using System.Net;
using System.Runtime.CompilerServices;
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
}
