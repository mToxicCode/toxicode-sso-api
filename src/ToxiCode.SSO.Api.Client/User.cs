namespace ToxiCode.SSO.Api.Client;

public class User
{
    public Guid Id { get; set; }
        
    public string Username { get; set; } = null!;
        
    public string Role { get; set; } = null!;
        
    public Role RoleMetaData { get; set; } = null!;
}