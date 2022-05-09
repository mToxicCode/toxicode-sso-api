using ToxiCode.SSO.Api.DataLayer.Cmds;
using ToxiCode.SSO.Api.DataLayer.Infrastructure;
using ToxiCode.SSO.Api.Dtos;

namespace ToxiCode.SSO.Api.DataLayer;

public class AuthRepository
{
    private readonly DbExecuteWrapper _dbExecuteWrapper;

    public AuthRepository(DbExecuteWrapper dbExecuteWrapper)
        => _dbExecuteWrapper = dbExecuteWrapper;

    public async Task<User> InsertNewUserAsync(InsertUserCmd insertCmd, CancellationToken cancellationToken)
    {
        var insertQuery = @"INSERT INTO users
                             (id, username, password, role)
                                    VALUES(@UserId, @Username, @Password, 'user');";
        var getQuery = @"SELECT * FROM users
                            WHERE username = @Username;";

        await _dbExecuteWrapper.ExecuteQueryAsync(insertQuery,
            new
            {
                insertCmd.UserId,
                insertCmd.Username,
                insertCmd.Password
            },
            cancellationToken);

        return await _dbExecuteWrapper.ExecuteQueryAsync<User>(getQuery,
            new
            {
                insertCmd.Username
            },
            cancellationToken);
    }

    public async Task<User> GetUserByUsernameAsync(GetUserByUsernameCmd getCmd, CancellationToken cancellationToken)
    {
        var query = $@"SELECT * FROM users
                            WHERE username = @Username;";

        return await _dbExecuteWrapper.ExecuteQueryAsync<User>(query,
            new {getCmd.Username},
            cancellationToken);
    }

    public async Task<User> GetUserByRefreshTokenAsync(GetUserByRefreshTokenCmd getCmd,
        CancellationToken cancellationToken)
    {
        var query = $@"SELECT ut.* FROM users ut
                            INNER JOIN refresh_tokens rtt ON rtt.user_id = ut.id
                            WHERE rtt.token = @RefreshToken;";

        return await _dbExecuteWrapper.ExecuteQueryAsync<User>(
            query,
            new {getCmd.RefreshToken},
            cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(GetUserByIdCmd cmd, CancellationToken cancellationToken) 
    {
        var query = $@"SELECT id, username, role FROM users
                            WHERE id = @UserId;";

        return await _dbExecuteWrapper.ExecuteQueryAsync<User>(query,
            new {cmd.UserId},
            cancellationToken);
    }

    public async Task<RefreshToken> GetRefreshToken(GetRefreshTokenCmd getCmd, CancellationToken cancellationToken)
    {
        var query = $@"SELECT id Id, user_id UserId, token Token, expires_at ExpiresAt, 
                            created_at CreatedAt, revoked_at RevokedAt, revoked_by_ip RevokedByIp, 
                            replaced_by_token ReplacedByToken FROM refresh_tokens
                            WHERE token = @Token;";

        return await _dbExecuteWrapper.ExecuteQueryAsync<RefreshToken>(query,
            new {getCmd.Token},
            cancellationToken);
    }

    public async Task UpdateRefreshTokenAsync(UpdateRefreshTokenCmd updateCmd, CancellationToken cancellationToken)
    {
        var query = $@"UPDATE refresh_tokens
                            SET revoked_at = @RevokedAt,
                                replaced_by_token = @ReplacedByToken,
                                revoked_by_ip = @RevokedByIp
                            WHERE token = @Token;";

        await _dbExecuteWrapper.ExecuteQueryAsync(
            query,
            new
            {
                updateCmd.RevokedAt,
                updateCmd.ReplacedByToken,
                updateCmd.RevokedByIp,
                updateCmd.Token
            },
            cancellationToken);
    }

    public async Task InsertRefreshTokenAsync(InsertRefreshTokenCmd insertCmd,
        CancellationToken cancellationToken)
    {
        var query = $@"INSERT INTO refresh_tokens
                            (user_id, token, expires_at, created_by_ip)
                            VALUES(@UserId, @Token, @ExpiresAt, @CreatedByIp);";

        await _dbExecuteWrapper.ExecuteQueryAsync(
            query,
            new
            {
                insertCmd.RefreshToken.UserId,
                insertCmd.RefreshToken.Token,
                insertCmd.RefreshToken.ExpiresAt,
                insertCmd.RefreshToken.CreatedByIp
            },
            cancellationToken);
    }
}