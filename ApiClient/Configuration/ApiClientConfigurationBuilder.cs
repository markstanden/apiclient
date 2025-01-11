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
            throw new ArgumentException("Provided baseUrl is empty or null.");
        }

        _baseUrl = baseUrl;

        return this;
    }

    /// <summary>
    /// Adds a bearer token to the ApiClientConfiguration
    /// </summary>
    /// <param name="secret">The bearer token secret</param>
    /// <returns>The current builder instance with baseUrl set</returns>
    public ApiClientConfigurationBuilder WithBearerToken(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("Provided secret is empty or null.");
        }

        _bearerAuthConfiguration = new BearerAuthConfiguration { Secret = secret };
        return this;
    }
}
