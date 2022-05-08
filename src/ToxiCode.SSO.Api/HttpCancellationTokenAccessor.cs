namespace ToxiCode.SSO.Api;

public class HttpCancellationTokenAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCancellationTokenAccessor(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public CancellationToken Token
        => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
}