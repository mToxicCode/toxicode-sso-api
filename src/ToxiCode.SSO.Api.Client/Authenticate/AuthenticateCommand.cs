using System.Text.Json.Serialization;

namespace ToxiCode.SSO.Api.Client.Authenticate
{
    public record AuthenticateCommand(string Username, string Password, bool IsExtended = false)
    {
        [JsonIgnore] 
        public string IpAddress { get; set; } = "0.0.0.0";
    }
}