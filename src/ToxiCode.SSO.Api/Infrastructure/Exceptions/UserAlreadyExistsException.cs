namespace ToxiCode.SSO.Api.Infrastructure.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message)
    {
    }
}