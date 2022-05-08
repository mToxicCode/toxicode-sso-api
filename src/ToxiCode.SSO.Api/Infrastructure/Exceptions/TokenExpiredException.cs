namespace ToxiCode.SSO.Api.Infrastructure.Exceptions;

public class TokenExpiredException : Exception
{
    public TokenExpiredException(string message) : base(message)
    {
    }
}