using ToxiCode.SSO.Api.DataLayer.Infrastructure;

namespace ToxiCode.SSO.Api.DataLayer.Extensions
{
    public static class DataInfrastructureExtension
    {
        public static IServiceCollection AddDatabaseInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDataOptions(configuration)
                .AddScoped<IDbConnectionFactory, DbConnectionFactory>()
                .AddScoped<DbExecuteWrapper>()
                .AddRepositories()
                .AddMigrator();

        private static IServiceCollection AddRepositories(this IServiceCollection services)
            => services.AddScoped<AuthRepository>();
    }
}