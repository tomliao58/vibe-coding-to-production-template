using System.Data;

namespace src.Api.Data
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
    }
}