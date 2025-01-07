namespace ApiClient.Configuration;

public record ApiClientConfiguration
{
    public required string BaseUrl { get; init; }
}