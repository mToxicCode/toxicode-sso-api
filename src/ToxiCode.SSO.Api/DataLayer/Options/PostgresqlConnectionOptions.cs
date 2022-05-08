using System.ComponentModel.DataAnnotations;

namespace ToxiCode.SSO.Api.DataLayer.Infrastructure.Options;

public class PostgresqlConnectionOptions
{
    [Required] 
    public string Username { get; set; } = null!;
    [Required] 
    public string Password { get; set; } = null!;
    [Required] 
    public string Host { get; set; } = null!;
    public string Port { get; set; } = "5432";
    [Required]
    public string Database { get; set; } = null!;
    public string Pooling { get; set; } = "true";
    public string MinPoolSize { get; set; } = "0";
    public string MaxPoolSize { get; set; } = "100";
    public string ConnectionLifetime { get; set; } = "0";

    public string BuildConnectionString() 
    => $"UserID={Username};Password={Password};Host={Host};Port={Port};" +
       $"Database={Database};Pooling={Pooling};MinPoolSize={MinPoolSize};" +
       $"MaxPoolSize={MaxPoolSize};ConnectionLifetime={ConnectionLifetime}";
}