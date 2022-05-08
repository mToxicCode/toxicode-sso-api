using Dapper;
using ToxiCode.SSO.Api.DataLayer.Infrastructure;

namespace ToxiCode.SSO.Api.DataLayer;

public class DbExecuteWrapper
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbExecuteWrapper(IDbConnectionFactory connectionFactory)
        => _connectionFactory = connectionFactory;
        
    public async Task<T> ExecuteQueryAsync<T>(string query, object? parameters = null, CancellationToken? cancellationToken = default)
    {
        await using var db = _connectionFactory.CreateDatabase(cancellationToken);
        return await db.Connection.QueryFirstOrDefaultAsync<T>(db.CreateCommand(query, parameters));
    }
        
    public async Task ExecuteQueryAsync(string query, object? parameters = null, CancellationToken? cancellationToken = default)
    {
        await using var db = _connectionFactory.CreateDatabase(cancellationToken);
        await db.Connection.QueryAsync(query, parameters);
    }
        
    public async Task<T[]> MultipleExecuteQueryAsync<T>(string query, object? parameters = null, CancellationToken? cancellationToken = default)
    {
        await using var db = _connectionFactory.CreateDatabase(cancellationToken);
        return (await db.Connection.QueryAsync<T>(db.CreateCommand(query, parameters))).ToArray();
    }

    public async Task ExecuteMultipliedQueryAsync(string query, object? parameters = null,
        CancellationToken? cancellationToken = default)
    {
        await using var db = _connectionFactory.CreateDatabase(cancellationToken);
        await db.Connection.ExecuteAsync(db.CreateCommand(query, parameters));
    }

}