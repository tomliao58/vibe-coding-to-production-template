using src.Api.Services;

namespace src.Api.Endpoints;

public static class ArtistEndpoints
{
    public static IEndpointRouteBuilder MapArtistEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/artists").WithTags("Artists");

        group.MapGet("/", async(
            int? limit, 
            ICatalogQueries queries,
            CancellationToken cancellationToken) =>
        {
            var safeLimit = Math.Clamp(limit ?? 20, 1, 100);
            var result = await queries.GetArtistsAsync(safeLimit, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetArtists");

        return app;
    }
}