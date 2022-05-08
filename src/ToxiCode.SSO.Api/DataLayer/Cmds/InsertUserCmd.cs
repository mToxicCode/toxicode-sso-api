namespace ToxiCode.SSO.Api.DataLayer.Cmds;

public record InsertUserCmd(Guid UserId, string Username, string Password);