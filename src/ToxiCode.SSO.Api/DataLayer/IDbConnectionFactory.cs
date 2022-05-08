namespace ToxiCode.SSO.Api.DataLayer.Infrastructure
{
    public interface IDbConnectionFactory
    {
        DatabaseWrapper CreateDatabase(CancellationToken? cancellationToken = default);
    }
}