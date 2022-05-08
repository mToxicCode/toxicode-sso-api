using System.Text.Json.Serialization;
using MediatR;

namespace ToxiCode.SSO.Api.Handlers.Authenticate
{
    public record AuthenticateCommand(string Username, string Password, bool IsExtended = false) : IRequest<AuthenticateResponse>
    {
        [JsonIgnore] 
        public string IpAddress { get; set; } = "0.0.0.0";
    }
}