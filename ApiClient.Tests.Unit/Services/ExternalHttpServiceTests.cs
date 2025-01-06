using System.Runtime.InteropServices.JavaScript;
using ApiClient.Services;
using JetBrains.Annotations;

namespace ApiClient.Tests.Unit.Services;

[TestSubject(typeof(ExternalHttpService))]
public class ExternalHttpServiceTests
{
    private IExternalHttpService _sut;

    public ExternalHttpServiceTests()
    {
        _sut = new ExternalHttpService(new HttpClient());
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
}
