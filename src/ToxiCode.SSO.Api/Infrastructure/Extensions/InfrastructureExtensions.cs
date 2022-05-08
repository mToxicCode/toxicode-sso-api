using Serilog;
using Serilog.Events;

namespace ToxiCode.SSO.Api.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddSerilogLogger(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .CreateLogger();
        return services.AddSingleton(Log.Logger);
    }

    public static WebApplicationBuilder WithLocalConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
        return builder;
    }
}