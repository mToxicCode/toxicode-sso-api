using MediatR;
using ToxiCode.SSO.Api.DataLayer;
using ToxiCode.SSO.Api.DataLayer.Cmds;
using ToxiCode.SSO.Api.Handlers.Authenticate;
using ToxiCode.SSO.Api.Infrastructure.Exceptions;

namespace ToxiCode.SSO.Api.Handlers.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthenticateResponse>
    {
        private readonly AuthRepository _repository;
        private readonly IMediator _mediator;

        public RegisterUserHandler(AuthRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        
        public async Task<AuthenticateResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.NewGuid();
            
            var user = await _repository.GetUserByUsernameAsync(new GetUserByUsernameCmd(request.Username), cancellationToken);
            if (user != null)
                throw new UserAlreadyExistsException($"Имя пользователя: {request.Username} занято");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = await _repository.InsertNewUserAsync(new InsertUserCmd(userId, request.Username, hashedPassword), cancellationToken);

            var newRequest = new AuthenticateCommand(newUser.Username, request.Password, request.IsExtended)
            {
                IpAddress = request.IpAddress
            };

            return await _mediator.Send(newRequest, cancellationToken);
        }
    }
}