namespace ToxiCode.SSO.Api.DataLayer.Cmds;

public record UpdateRefreshTokenCmd
{
    public string Token { get; set; } = null!;
    public DateTime RevokedAt { get; set; }
    public string RevokedByIp { get; set; } = null!;
    public string ReplacedByToken { get; set; } = null!;
}