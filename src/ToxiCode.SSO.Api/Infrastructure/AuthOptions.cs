namespace ToxiCode.SSO.Api.Infrastructure;

public class AuthOptions
{
    public double TokenLifeTimeMinutes { get; init; }

    public string? AuthKey { get; init; }
}