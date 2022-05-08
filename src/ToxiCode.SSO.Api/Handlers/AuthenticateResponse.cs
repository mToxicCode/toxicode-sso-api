using ToxiCode.SSO.Api.Dtos;

namespace ToxiCode.SSO.Api.Handlers
{
    public record AuthenticateResponse(User User, string JwtToken, string RefreshToken);
}