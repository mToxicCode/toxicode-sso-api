using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using ToxiCode.SSO.Api.DataLayer;
using ToxiCode.SSO.Api.DataLayer.Cmds;
using ToxiCode.SSO.Api.Infrastructure;
using ToxiCode.SSO.Api.Infrastructure.Exceptions;
using ToxiCode.SSO.Api.Infrastructure.Extensions;

namespace ToxiCode.SSO.Api.Handlers.Authenticate;

public class AuthenticateHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
{
    private readonly AuthRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IOptions<AuthOptions> _options;

    public AuthenticateHandler(AuthRepository repository, 
        IMapper mapper,
        IConfiguration configuration,
        IOptions<AuthOptions> options)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
        _options = options;
    }
        
    public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByUsernameAsync(new GetUserByUsernameCmd(request.Username), cancellationToken);
            
        if (user == null)
            throw new NoUserException("User with login:password pair can't be found");
        if (!BCrypt.Net.BCrypt.Verify(request.Password,user.Password))
            throw new NoUserException("User with login:password pair can't be found");

        var jwtToken = user.GenerateJwtToken(_configuration, _options);
            
        var refreshToken = request.IpAddress.GenerateRefreshToken(request.IsExtended);
        refreshToken.UserId = user.Id;
            
        await _repository.InsertRefreshTokenAsync(new InsertRefreshTokenCmd(refreshToken), cancellationToken);

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }
}