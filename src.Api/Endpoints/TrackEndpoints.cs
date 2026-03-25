using src.Api.Services;

namespace src.Api.Endpoints;

public static class TrackEndpoints
{
    public static IEndpointRouteBuilder MapTrackEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tracks").WithTags("Tracks");

        group.MapGet("/search", async(
            string? q,
            int? limit,
            ICatalogQueries queries,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return Results.BadRequest(new
                {
                    error = "Query parameter 'q' is required."
                });
            }

            var safeLimit = Math.Clamp(limit ?? 20, 1, 100);
            var result = await queries.SearchTracksAsync(q.Trim(), safeLimit, cancellationToken);
            return Results.Ok(result);
        
        })
        .WithName("SearchTracks");

        return app;
    }
}