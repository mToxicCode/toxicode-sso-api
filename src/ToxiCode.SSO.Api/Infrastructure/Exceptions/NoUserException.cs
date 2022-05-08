namespace ToxiCode.SSO.Api.Infrastructure.Exceptions;

public class NoUserException : Exception
{
    public NoUserException(string message) : base(message)
    {
    }
}