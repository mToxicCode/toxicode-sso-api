using MediatR;
using ToxiCode.SSO.Api.DataLayer;
using ToxiCode.SSO.Api.DataLayer.Cmds;

namespace ToxiCode.SSO.Api.Handlers.GetUser;

public class GetUserHandler : IRequestHandler<GetUserCommand, GetUserResponse>
{
    private readonly AuthRepository _repository;

    public GetUserHandler(AuthRepository repository) 
        => _repository = repository;

    public async Task<GetUserResponse> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var cmd = new GetUserByIdCmd(request.UserId);
        var result = await _repository.GetUserByIdAsync(cmd, cancellationToken);
        return new GetUserResponse(result);
    }
}