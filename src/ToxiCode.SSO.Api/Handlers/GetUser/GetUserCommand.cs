using MediatR;

namespace ToxiCode.SSO.Api.Handlers.GetUser;

public record GetUserCommand(Guid UserId) : IRequest<GetUserResponse>;