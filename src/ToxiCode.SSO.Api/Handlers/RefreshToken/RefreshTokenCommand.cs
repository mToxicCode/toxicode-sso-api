using MediatR;

namespace ToxiCode.SSO.Api.Handlers.RefreshToken
{
    public record RefreshTokenCommand(string Token, string IpAddress) : IRequest<AuthenticateResponse>;
}