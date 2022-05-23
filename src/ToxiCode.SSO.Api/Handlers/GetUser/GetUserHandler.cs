using MediatR;
using Microsoft.Extensions.Options;
using ToxiCode.SSO.Api.DataLayer;
using ToxiCode.SSO.Api.DataLayer.Cmds;
using ToxiCode.SSO.Api.Infrastructure;
using ToxiCode.SSO.Api.Infrastructure.Extensions;

namespace ToxiCode.SSO.Api.Handlers.GetUser;

public class GetUserHandler : IRequestHandler<GetUserCommand, AuthenticateResponse>
{
    private readonly AuthRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IOptions<AuthOptions> _options;

    public GetUserHandler(AuthRepository repository, IConfiguration configuration, IOptions<AuthOptions> options)
    {
        _repository = repository;
        _configuration = configuration;
        _options = options;
    }

    public async Task<AuthenticateResponse> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var cmd = new GetUserByIdCmd(request.UserId);
        var user = await _repository.GetUserByIdAsync(cmd, cancellationToken);
        
        var jwtToken = user.GenerateJwtToken(_configuration, _options);
            
        var refreshToken = request.IpAddress.GenerateRefreshToken(request.IsExtended);
        refreshToken.UserId = user.Id;
            
        await _repository.InsertRefreshTokenAsync(new InsertRefreshTokenCmd(refreshToken), cancellationToken);

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }
}