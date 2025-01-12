namespace ApiClient.Configuration.Auth;

public record ApiKeyAuthConfiguration
{
    public required string Key { get; init; }
    public required string Secret { get; init; }
}
