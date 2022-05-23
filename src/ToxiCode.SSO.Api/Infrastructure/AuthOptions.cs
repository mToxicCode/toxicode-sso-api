namespace ToxiCode.SSO.Api.Infrastructure;

public class AuthOptions
{
    public double TokenLifeTimeMinutes { get; set; }

    public string? AuthKey { get; set; }
}