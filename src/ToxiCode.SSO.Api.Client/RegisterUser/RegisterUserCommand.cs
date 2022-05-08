namespace ToxiCode.SSO.Api.Client.RegisterUser;

public record RegisterUserCommand (string Username, string Password, bool IsExtended = false);