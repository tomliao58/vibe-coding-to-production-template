namespace src.Api.Models;

public sealed record AlbumDetailDto(
    int AlbumId,
    string Title,
    int ArtistId,
    string ArtistName
);
