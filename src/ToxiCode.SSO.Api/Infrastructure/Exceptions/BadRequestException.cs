namespace ToxiCode.SSO.Api.Infrastructure.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}