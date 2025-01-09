namespace ApiClient.Configuration.Auth;

public record BearerAuthConfiguration
{
    public required string Secret { get; init; }
}
