using ApiClient.Configuration;
using JetBrains.Annotations;

namespace ApiClient.Tests.Unit.Configuration;

[TestSubject(typeof(ApiClientConfigurationBuilder))]
public class ApiClientConfigurationBuilderTests
{
    private const string BASE_URL = "https://valid.test.url";

    private readonly ApiClientConfigurationBuilder _sut;

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
    public void WithBearerToken_ChainedWithBuildMethod_ReturnsConfigurationWithBearerTokenSet()
    {
        var secret = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);

        ApiClientConfiguration result = _sut.WithBearerToken(secret).Build();

        Assert.NotNull(result.BearerToken);
        Assert.Contains(secret, result.BearerToken.Secret);
    }

    [Fact]
    public void WithBearerToken_NotChainedWithBuildMethod_ReturnsConfigurationWithBearerTokenSet()
    {
        var secret = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);
        _sut.WithBearerToken(secret);

        ApiClientConfiguration result = _sut.Build();

        Assert.NotNull(result.BearerToken);
        Assert.Contains(secret, result.BearerToken.Secret);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void WithBearerToken_WithInvalidSecret_ThrowsArgumentException(string secret)
    {
        _sut.WithBaseUrl(BASE_URL);

        Exception? result = Record.Exception(() => _sut.WithBearerToken(secret));

        Assert.NotNull(result);
        Assert.IsType<ArgumentException>(result);
    }

    [Fact]
    public void WithApiKey_ChainedWithBuildMethod_ReturnsConfigurationWithApiKeySet()
    {
        var key = Guid.NewGuid().ToString();
        var secret = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);

        ApiClientConfiguration result = _sut.WithApiKey(key, secret).Build();

        Assert.NotNull(result.ApiKey);
        Assert.Contains(secret, result.ApiKey.Key);
        Assert.Contains(secret, result.ApiKey.Secret);
    }

    [Fact]
    public void WithApiKey_NotChainedWithBuildMethod_ReturnsConfigurationWithApiKeySet()
    {
        var key = Guid.NewGuid().ToString();
        var secret = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);
        _sut.WithApiKey(key, secret);

        ApiClientConfiguration result = _sut.Build();

        Assert.NotNull(result.ApiKey);
        Assert.Contains(secret, result.ApiKey.Key);
        Assert.Contains(secret, result.ApiKey.Secret);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("  ", null)]
    public void WithApiKey_WithInvalidKey_ThrowsArgumentException(string secret)
    {
        _sut.WithBaseUrl(BASE_URL);

        Exception? result = Record.Exception(() => _sut.WithBearerToken(secret));

        Assert.NotNull(result);
        Assert.IsType<ArgumentException>(result);
    }
}
