using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToxiCode.SSO.Api.Dtos;

namespace ToxiCode.SSO.Api.Infrastructure.Extensions;

public static class AuthTokenExtensions
{
    public static string GenerateJwtToken(this User user,
        IConfiguration configuration, 
        IOptions<AuthOptions> options)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = configuration[EnvironmentConstants.AuthKey] is null 
            ? "defaultKeyqweqweqweqweqweqweqweqweqweqwe".GetSymmetricSecurityKey()
            : configuration[EnvironmentConstants.AuthKey].GetSymmetricSecurityKey();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("id", user.Id.ToString()),
                new("role", user.Role),
                new("username", user.Username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(options.Value.TokenLifeTimeMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static RefreshToken GenerateRefreshToken(this string ipAddress, bool isExtended = false)
    {
        using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAt = isExtended
                ? DateTime.UtcNow.AddDays(7)
                : DateTime.UtcNow.AddHours(1),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }
}