using System.Data;
using System.Data.Common;
using Dapper;

namespace ToxiCode.SSO.Api.DataLayer.Infrastructure;

public class DatabaseWrapper : IAsyncDisposable
{
    private const int DefaultTimeout = 15;
    private readonly CancellationToken _cancellationToken;

    public DatabaseWrapper(DbConnection connection, CancellationToken cancellationToken)
    {
        Connection = connection;
        _cancellationToken = cancellationToken;
    }
        
    public DbConnection Connection { get; }

    public CommandDefinition CreateCommand(
        string commandText,
        object? parameters = null,
        CommandType? commandType = null,
        int? commandTimeout = null,
        IDbTransaction? transaction = null)
        => new(
            commandText,
            parameters,
            commandTimeout: commandTimeout ?? DefaultTimeout,
            commandType: commandType ?? CommandType.Text,
            transaction: transaction,
            cancellationToken: _cancellationToken);
        
    public ValueTask DisposeAsync()
        => Connection.DisposeAsync();
}