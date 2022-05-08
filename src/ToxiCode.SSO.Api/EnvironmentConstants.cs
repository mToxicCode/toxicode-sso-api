namespace ToxiCode.SSO.Api;

public static class EnvironmentConstants
{
    public const string IsRunningInContainer = "DOTNET_RUNNING_IN_CONTAINER";
    
    public const string AuthKey = "AUTH_TOKEN";

    public const string DatabaseUser = "DB_USER";
    public const string DatabasePassword = "DB_PASSWORD";
    public const string DatabaseHost = "DB_HOST";
    public const string DatabasePort = "DB_PORT";
    public const string DatabaseName = "DB_NAME";
    public const string DatabasePooling = "DB_POOLING";
    public const string DatabaseMinPoolSize = "DB_MIN_POOL_SIZE";
    public const string DatabaseMaxPoolSize = "DB_MAX_POOL_SIZE";
    public const string DatabaseConnectionLifetime = "DB_CONNECTION_LIFETIME";
}