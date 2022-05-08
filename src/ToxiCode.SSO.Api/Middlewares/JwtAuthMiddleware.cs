using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToxiCode.SSO.Api.Dtos;
using ToxiCode.SSO.Api.Infrastructure;

namespace ToxiCode.SSO.Api.Middlewares;

public class  JwtAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AuthOptions _options;

    public JwtAuthMiddleware(RequestDelegate next, IOptions<AuthOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var first = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = first?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, token);

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.AuthKey!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            var username = jwtToken.Claims.First(x => x.Type == "username").Value;
            var role = jwtToken.Claims.First(x => x.Type == "role").Value;

            context.Items["User"] = new User
            {
                Id = userId,
                Username = username,
                Role = role
            };
        }
        catch
        {
            // ignored
        }
    }
}