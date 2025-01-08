namespace ApiClient.Configuration.Auth;

public record BearerAuthConfiguration
{
    public required string Token { get; init; }
}
