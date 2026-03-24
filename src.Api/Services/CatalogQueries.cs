using Dapper;
using src.Api.Data;
using src.Api.Models;

namespace src.Api.Services;

public sealed class CatalogQueries : ICatalogQueries
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CatalogQueries(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ArtistDto>> GetArtistsAsync(int limit, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT ""artist_id"" AS ""ArtistId"", ""name"" AS ""Name""  
            FROM ""artist"" 
            ORDER BY ""artist_id"" 
            LIMIT @limit;";
        
        using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var rows = await connection.QueryAsync<ArtistDto>(
            new CommandDefinition(
                sql, 
                new { Limit = limit }, 
                cancellationToken: cancellationToken)
                );
        
        return rows.ToList();
    }

    public async Task<AlbumDetailDto?> GetAlbumByIdAsync(int albumId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                a.""album_id"" AS ""AlbumId"", 
                a.""title"" AS ""Title"", 
                ar.""artist_id"" AS ""ArtistId"", 
                ar.""name"" AS ""ArtistName""
            FROM ""album"" a
            INNER JOIN ""artist"" ar ON a.""artist_id"" = ar.""artist_id""
            WHERE a.""album_id"" = @albumId;";

        using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<AlbumDetailDto>(
            new CommandDefinition(
                sql,
                new { AlbumId = albumId },
                cancellationToken: cancellationToken));
            
    }

    public async Task<IReadOnlyList<TrackSearchResultDto>> SearchTracksAsync(string query, int limit, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                t.""track_id"" AS ""TrackId"", 
                t.""name"" AS ""Name"", 
                a.""title"" AS ""AlbumTitle"", 
                ar.""name"" AS ""ArtistName"", 
                t.""composer"" AS ""Composer"",
                t.""milliseconds"" AS ""Milliseconds"",
                t.""unit_price"" AS ""UnitPrice""
            FROM ""track"" t
            INNER JOIN ""album"" a ON t.""album_id"" = a.""album_id""
            INNER JOIN ""artist"" ar ON a.""artist_id"" = ar.""artist_id""
            WHERE t.""name"" ILIKE @Pattern
            ORDER BY t.""track_id""
            LIMIT @limit;";

        using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var rows = await connection.QueryAsync<TrackSearchResultDto>(
            new CommandDefinition(
                sql,
                new { Pattern = $"%{query}%", Limit = limit },
                cancellationToken: cancellationToken));

        return rows.ToList();
    }
}