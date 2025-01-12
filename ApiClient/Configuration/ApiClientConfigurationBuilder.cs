using ApiClient.Configuration.Auth;

namespace ApiClient.Configuration;

public class ApiClientConfigurationBuilder
{
    private string _baseUrl;
    private BearerAuthConfiguration? _bearerAuthConfiguration;
    private ApiKeyAuthConfiguration? _apiKeyAuthConfiguration;

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
            ApiKey = _apiKeyAuthConfiguration,
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
    /// Bearer tokens can be used to authenticate with a shared secret or
    /// JWT, and is applied within the request header.
    /// </summary>
    /// <param name="secret">The bearer token secret</param>
    /// <returns>The current builder instance with the bearer token set</returns>
    public ApiClientConfigurationBuilder WithBearerToken(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("Provided secret is empty or null.");
        }

        _bearerAuthConfiguration = new BearerAuthConfiguration { Secret = secret };
        return this;
    }

    /// <summary>
    /// Adds an API Key to the ApiClientConfiguration
    /// API Keys can be added as key:value within the authentication header, or as a
    /// single value with the default key X-API-KEY.
    /// </summary>
    /// <param name="key">The API Key</param>
    /// <param name="secret">The API Key Secret</param>
    /// <returns>The current builder instance with the API Key set</returns>
    public ApiClientConfigurationBuilder WithApiKey(string key, string secret)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Provided key is empty or null.");
        }

        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("Provided secret is empty or null.");
        }

        _apiKeyAuthConfiguration = new ApiKeyAuthConfiguration { Key = key, Secret = secret };

        return this;
    }

    public ApiClientConfigurationBuilder WithApiKey(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("Provided secret is empty or null.");
        }

        return WithApiKey("X-API-KEY", secret);
    }

    public ApiClientConfigurationBuilder WithContentType(string contentType)
    {
        return null;
    }
}
