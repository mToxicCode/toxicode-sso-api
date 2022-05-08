#region Services
///////////////////////////////////////////////////////
// Application services/DI Container configures ///////
///////////////////////////////////////////////////////

using MediatR;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ToxiCode.SSO.Api;
using ToxiCode.SSO.Api.DataLayer.Extensions;
using ToxiCode.SSO.Api.Infrastructure;
using ToxiCode.SSO.Api.Infrastructure.Extensions;
using ToxiCode.SSO.Api.Middlewares;

var builder = WebApplication
    .CreateBuilder(args)
    .WithLocalConfiguration();
builder.Host.UseSerilog();
var services = builder.Services;

services.AddControllers();
services.AddMediatR(typeof(Program));
services.AddAutoMapper(typeof(Program));
services.AddDatabaseInfrastructure(builder.Configuration);
services.AddHttpContextAccessor();
services.AddSingleton<HttpCancellationTokenAccessor>();
services.AddEndpointsApiExplorer();
services.AddSerilogLogger();
services.AddOptions<AuthOptions>()
    .Configure(opt => builder.Configuration
        .GetSection(nameof(AuthOptions))
        .Bind(opt));

services.AddSwagger();
services.AddSingleton(new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = 
        builder.Configuration[EnvironmentConstants.AuthKey]?.GetSymmetricSecurityKey()
        ?? builder.Configuration.GetChildren()?.FirstOrDefault(x => x.Key == "AuthOptions:AuthKey")?.Value?.GetSymmetricSecurityKey(),
    ClockSkew = TimeSpan.Zero
});

#endregion

#region App
///////////////////////////////////////////////////////
// Application middlewares and entrypoint /////////////
///////////////////////////////////////////////////////

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<JwtAuthMiddleware>();
app.MapControllers();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (args.Contains("migrate"))
    app.Migrate();
else
    await app.RunAsync();


#endregion
