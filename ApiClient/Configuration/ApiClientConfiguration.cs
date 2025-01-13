using ApiClient.Configuration.Auth;
using ApiClient.Configuration.Headers;

namespace ApiClient.Configuration;

public record ApiClientConfiguration
{
    public required string BaseUrl { get; init; }
    public BearerAuthConfiguration? BearerToken { get; init; }
    public ApiKeyAuthConfiguration? ApiKey { get; init; }
    public ContentType? ContentType { get; init; }
}
