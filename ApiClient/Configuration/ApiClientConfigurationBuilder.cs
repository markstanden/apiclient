using ApiClient.Configuration.Auth;

namespace ApiClient.Configuration;

public class ApiClientConfigurationBuilder
{
    private string _baseUrl;
    private BearerAuthConfiguration? _bearerAuthConfiguration;

    /// <summary>
    /// Builds the configuration
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Throws if Base URL is not set</exception>
    public ApiClientConfiguration Build()
    {
        if (string.IsNullOrEmpty(_baseUrl))
            throw new InvalidOperationException("Base url not set");
        return new ApiClientConfiguration
        {
            BaseUrl = _baseUrl,
            BearerToken = _bearerAuthConfiguration,
        };
    }

    /// <summary>
    /// Adds a base url to the ApiClientConfiguration
    /// </summary>
    /// <param name="baseUrl">The base url of the request</param>
    /// <returns>The current builder instance with baseUrl set</returns>
    public ApiClientConfigurationBuilder WithBaseUrl(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException();
        }

        _baseUrl = baseUrl;
        return this;
    }

    public ApiClientConfigurationBuilder WithBearerToken(string token)
    {
        _bearerAuthConfiguration = new BearerAuthConfiguration { Token = token };
        return this;
    }
}
