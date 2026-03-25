using System.Security.Cryptography.X509Certificates;
using src.Api.Services;

namespace src.Api.Endpoints;

public static class AlbumEndpoints
{
    public static IEndpointRouteBuilder MapAlbumEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/albums").WithTags("Albums");

        group.MapGet("/{id:int}", async(
            int id, 
            ICatalogQueries queries,
            CancellationToken cancellationToken) =>
        {
            var result = await queries.GetAlbumByIdAsync(id, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetAlbumById");

        return app;
    }
}