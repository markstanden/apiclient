namespace ApiClient.Configuration;

public class ApiClientConfigurationBuilder
{
    private string _baseUrl;
    
    public ApiClientConfigurationBuilder()
    {
    }

    public ApiClientConfiguration Build()
    {
        if (string.IsNullOrEmpty(_baseUrl)) throw new InvalidOperationException("Base url not set");
        return new ApiClientConfiguration
        {
            BaseUrl = _baseUrl
        };
    }

    /// <summary>
    /// Adds a base url to the ApiClientConfiguration
    /// </summary>
    /// <param name="baseUrl">The base url of the request</param>
    /// <returns></returns>
    public ApiClientConfigurationBuilder WithBaseUrl(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException();
        }

        _baseUrl = baseUrl;
        return this;
    }
}