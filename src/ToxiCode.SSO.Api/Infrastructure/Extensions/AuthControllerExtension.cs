namespace ToxiCode.SSO.Api.Infrastructure.Extensions;

public static class AuthControllerExtension
{
    public static HttpResponse SetTokenCookie(this HttpResponse response, string token, bool isExtended = false)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = isExtended
                ? DateTime.UtcNow.AddDays(7)
                : DateTime.UtcNow.AddHours(1)
        };
        response.Cookies.Append("refreshToken", token, cookieOptions);
        response.Cookies.Append("isExtended", "yes", cookieOptions);
        return response;
    }

    public static string GetIpAddress(this HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            return context.Request.Headers["X-Forwarded-For"];
        return context.Connection.RemoteIpAddress!.MapToIPv4().ToString();
    }
}