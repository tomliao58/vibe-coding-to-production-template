namespace src.Api.Models;

public sealed record TrackSearchResultDto(
    int TrackId,
    string Name,
    string AlbumTitle,
    string ArtistName,
    string? Composer,
    int Milliseconds,
    decimal UnitPrice
);
