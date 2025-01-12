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

    /*
     * Build method tests.
     */

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

    /*
     * WithBaseUrl config builder method tests.
     */

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void WithBaseUrl_WhenUrlIsInvalid_ThrowsArgumentException(string invalidUrl)
    {
        var shouldThrow = () => _sut.WithBaseUrl(invalidUrl);

        AssertThrowsArgumentException(() => shouldThrow());
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

    /*
     * WithBearerToken config builder method tests.
     */

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
        var sut = GetSutWithBaseUrlSet();

        var shouldThrow = () => sut.WithBearerToken(secret);

        AssertThrowsArgumentException(() => shouldThrow());
    }

    /*
     * WithApiKey config builder method tests.
     */
    [Fact]
    public void WithApiKey_ChainedWithBuildMethod_ReturnsConfigurationWithApiKeyAndSecretSet()
    {
        var key = Guid.NewGuid().ToString();
        var secret = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);

        ApiClientConfiguration result = _sut.WithApiKey(key, secret).Build();

        Assert.NotNull(result.ApiKey);
        Assert.Contains(key, result.ApiKey.Key);
        Assert.Contains(secret, result.ApiKey.Secret);
    }

    [Fact]
    public void WithApiKey_NotChainedWithBuildMethod_ReturnsConfigurationWithApiKeyAndSecretSet()
    {
        var key = Guid.NewGuid().ToString();
        var secret = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);
        _sut.WithApiKey(key, secret);

        ApiClientConfiguration result = _sut.Build();

        Assert.NotNull(result.ApiKey);
        Assert.Contains(key, result.ApiKey.Key);
        Assert.Contains(secret, result.ApiKey.Secret);
    }

    [Fact]
    public void WithApiKey_WithJustSecret_ReturnsApiConfigurationWithDefaultKey()
    {
        var secret = Guid.NewGuid().ToString();
        _sut.WithBaseUrl(BASE_URL);

        ApiClientConfiguration result = _sut.WithApiKey(secret).Build();

        Assert.NotNull(result.ApiKey);
        Assert.Equal("X-API-KEY", result.ApiKey.Key);
        Assert.Equal(secret, result.ApiKey.Secret);
    }

    [Theory]
    [InlineData(null, "validSecret")]
    [InlineData("", "validSecret")]
    [InlineData("  ", "validSecret")]
    [InlineData("validKey", "")]
    [InlineData("validKey", "  ")]
    public void WithApiKey_WithInvalidCredentials_ThrowsArgumentException(string key, string secret)
    {
        var sut = GetSutWithBaseUrlSet();

        var shouldThrow = () => sut.WithApiKey(key, secret);

        AssertThrowsArgumentException(() => shouldThrow());
    }

    [Theory]
    [InlineData("validKey", "validSecret")]
    [InlineData("k", "s")]
    [InlineData("key-with-special-characters-!@£$%^&*()", "validSecret")]
    [InlineData("validKey", "secret-with-special-characters-!@£$%^&*()")]
    [InlineData(
        "key-with-special-characters-!@£$%^&*()",
        "secret-with-special-characters-!@£$%^&*()"
    )]
    public void WithApiKey_BuiltWithValidCredentials_ReturnsConfigurationWithApiKeyValuesSet(
        string key,
        string secret
    )
    {
        var sut = GetSutWithBaseUrlSet();

        var result = sut.WithApiKey(key, secret).Build();

        Assert.NotNull(result.ApiKey);
        Assert.Equal(key, result.ApiKey.Key);
        Assert.Equal(secret, result.ApiKey.Secret);
    }

    /*
     * WithContentType method tests
     */

    [Fact]
    public void WithContentType_ChainedWithBuildMethod_ReturnsConfigurationWithContentTypeSet()
    {
        var validContentType = "application/json";
        var sut = GetSutWithBaseUrlSet();

        ApiClientConfiguration result = sut.WithContentType(validContentType).Build();

        Assert.NotNull(result.ContentType);
        Assert.Contains(validContentType, result.ContentType);
    }

    [Fact]
    public void WithContentType_NotChainedWithBuildMethod_ReturnsConfigurationWithContentTypeSet()
    {
        var validContentType = "application/json";
        var sut = GetSutWithBaseUrlSet();
        sut.WithContentType(validContentType);

        ApiClientConfiguration result = sut.Build();

        Assert.NotNull(result.ContentType);
        Assert.Contains(validContentType, result.ContentType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void WithContentType_WithInvalidContentType_ThrowsArgumentException(string contentType)
    {
        var sut = GetSutWithBaseUrlSet();

        var shouldThrow = () => sut.WithContentType(contentType);

        AssertThrowsArgumentException(() => shouldThrow());
    }

    /*
     * Helper Methods
     */
    public ApiClientConfigurationBuilder GetSutWithBaseUrlSet(string baseUrl = BASE_URL)
    {
        return _sut.WithBaseUrl(baseUrl);
    }

    public void AssertThrowsArgumentException(Action methodUnderTest)
    {
        var exception = Record.Exception(methodUnderTest);
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }
}
