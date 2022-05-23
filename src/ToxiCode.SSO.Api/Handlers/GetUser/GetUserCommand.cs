using MediatR;

namespace ToxiCode.SSO.Api.Handlers.GetUser;

public record GetUserCommand(Guid UserId, string IpAddress, bool IsExtended) : IRequest<AuthenticateResponse>;