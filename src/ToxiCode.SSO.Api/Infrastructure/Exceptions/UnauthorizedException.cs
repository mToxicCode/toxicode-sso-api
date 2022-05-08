namespace ToxiCode.SSO.Api.Infrastructure.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}