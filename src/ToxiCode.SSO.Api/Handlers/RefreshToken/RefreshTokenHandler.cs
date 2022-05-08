using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using ToxiCode.SSO.Api.DataLayer;
using ToxiCode.SSO.Api.DataLayer.Cmds;
using ToxiCode.SSO.Api.Infrastructure;
using ToxiCode.SSO.Api.Infrastructure.Exceptions;
using ToxiCode.SSO.Api.Infrastructure.Extensions;

namespace ToxiCode.SSO.Api.Handlers.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthenticateResponse>
{
    private readonly AuthRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IOptions<AuthOptions> _options;

    public RefreshTokenHandler(AuthRepository repository, 
        IMapper mapper, 
        IConfiguration configuration,
        IOptions<AuthOptions> options)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
        _options = options;
    } 
        
    public async Task<AuthenticateResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByRefreshTokenAsync(new GetUserByRefreshTokenCmd(request.Token), cancellationToken);
        if (user == null)
            throw new NoUserException("No user with this refreshing token");

        var oldToken = await _repository.GetRefreshToken(new GetRefreshTokenCmd(request.Token), cancellationToken);
        if (oldToken is not { IsActive: true })
            throw new TokenExpiredException("Token is no more active");

        var newRefreshToken = request.IpAddress.GenerateRefreshToken();
        newRefreshToken.UserId = user.Id;
           
        var insertCmd = new InsertRefreshTokenCmd(newRefreshToken);
        await _repository.InsertRefreshTokenAsync(insertCmd, cancellationToken);
           
        var updateCmd = _mapper.Map<UpdateRefreshTokenCmd>(request, opt
            => opt.AfterMap((_, dest) =>
            {
                dest.RevokedAt = DateTime.UtcNow;
                dest.ReplacedByToken = newRefreshToken.Token;
            }));
        await _repository.UpdateRefreshTokenAsync(updateCmd, cancellationToken);
           
        var newJwtToken = user.GenerateJwtToken(_configuration, _options);

        return new AuthenticateResponse(user, newJwtToken, newRefreshToken.Token);
    }
}