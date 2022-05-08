using System.Text.Json.Serialization;

namespace ToxiCode.SSO.Api.Dtos
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; } = null!;
        
        public string Role { get; set; } = null!;
        
        public Role RoleMetaData { get; set; } = null!;

        [JsonIgnore]
        public string Password { get; set; } = null!;
        
        [JsonIgnore] 
        public List<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}