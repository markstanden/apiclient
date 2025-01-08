using ApiClient.Configuration;
using JetBrains.Annotations;

namespace ApiClient.Tests.Unit.Configuration;

[TestSubject(typeof(ApiClientConfigurationBuilder))]
public class ApiClientConfigurationBuilderTests
{
    private const string BASE_URL = "https://valid.test.url";

    private ApiClientConfigurationBuilder _sut;

    public ApiClientConfigurationBuilderTests()
    {
        _sut = new ApiClientConfigurationBuilder();
    }

    [Fact]
    public void Build_WithoutConfiguration_ThrowsInvalidOperationException()
    {
        var exception = Record.Exception(() => _sut.Build());

        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Build_WithoutSettingBaseUrl_ThrowsInvalidOperationException()
    {
        var token = Guid.NewGuid().ToString();
        _sut.WithBearerToken(token);

        var exception = Record.Exception(() => _sut.Build());

        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Build_WithBaseUrlOnly_BuildsConfiguration()
    {
        _sut.WithBaseUrl(BASE_URL);

        ApiClientConfiguration result = _sut.Build();

        Assert.NotNull(result);
        Assert.IsType<ApiClientConfiguration>(result);
    }

    [Fact]
    public void Build_WithBaseUrlOnly_SetsConfigurationBaseUrl()
    {
        _sut.WithBaseUrl(BASE_URL);

        ApiClientConfiguration result = _sut.Build();

        Assert.Equal(BASE_URL, result.BaseUrl);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void WithBaseUrl_WhenUrlIsInvalid_ThrowsArgumentException(string invalidUrl)
    {
        var exception = Record.Exception(() => _sut.WithBaseUrl(invalidUrl));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void WithBaseUrl_WithValidBaseUrl_ReturnsBuilder()
    {
        _sut.WithBaseUrl(BASE_URL);

        var result = _sut;

        Assert.NotNull(result);
        Assert.IsType<ApiClientConfigurationBuilder>(result);
    }

    [Fact]
    public void WithBaseUrl_ChainedWithBuildMethod_ReturnsConfigurationWithBaseUrlSet()
    {
        ApiClientConfiguration result = _sut.WithBaseUrl(BASE_URL).Build();

        Assert.Equal(BASE_URL, result.BaseUrl);
    }

    [Fact]
    public void WithBearerToken_ChainedWithBuildMethod_ReturnsConfigurationWithBearerAuthSet()
    {
        var token = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);

        ApiClientConfiguration result = _sut.WithBearerToken(token).Build();

        Assert.NotNull(result.BearerToken);
        Assert.Contains(token, result.BearerToken.Token);
    }

    [Fact]
    public void WithBearerToken_NotChainedWithBuildMethod_ReturnsConfigurationWithBearerAuthSet()
    {
        var token = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);
        _sut.WithBearerToken(token);

        ApiClientConfiguration result = _sut.Build();

        Assert.NotNull(result.BearerToken);
        Assert.Contains(token, result.BearerToken.Token);
    }
}
