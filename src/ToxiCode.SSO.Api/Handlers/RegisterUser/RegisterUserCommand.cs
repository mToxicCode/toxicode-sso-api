using System.Text.Json.Serialization;
using MediatR;

namespace ToxiCode.SSO.Api.Handlers.RegisterUser
{
    public record RegisterUserCommand
        (string Username, string Password, bool IsExtended = false) : IRequest<AuthenticateResponse>
    {
        [JsonIgnore] 
        public string IpAddress { get; set; } = "0.0.0.0";
    }
}