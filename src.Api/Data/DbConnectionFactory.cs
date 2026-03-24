using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace src.Api.Data;

public sealed class DbConnectionFactory:IDbConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor of the DbConnectionFactory class, which initializes the connection string from the configuration.
    /// </summary>
    /// <param name="configuration">The configuration from which to retrieve the connection string.</param>
    /// <exception cref="InvalidOperationException">Thrown if the connection string is not found in the configuration.</exception>
    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres") 
            ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");
    }

    /// <summary>
    /// Asynchronously creates and opens a new database connection using the connection string.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the open database connection.</returns>
    public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}