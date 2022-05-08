namespace ToxiCode.SSO.Api.Client;

public record AuthenticateResponse(User User, string JwtToken, string RefreshToken);