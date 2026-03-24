using src.Api.Models;

namespace src.Api.Services;

public interface ICatalogQueries
{
    Task<IReadOnlyList<ArtistDto>> GetArtistsAsync(int limit, CancellationToken cancellationToken);
    Task<AlbumDetailDto?> GetAlbumByIdAsync(int albumId, CancellationToken cancellationToken);
    Task<IReadOnlyList<TrackSearchResultDto>> SearchTracksAsync(string query, int limit, CancellationToken cancellationToken);
}