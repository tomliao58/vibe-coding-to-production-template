// using Npgsql;
using Scalar.AspNetCore;
using src.Api.Data;
using src.Api.Endpoints;
using src.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddOpenApi();

// builder.Services.AddSingleton(sp =>
// {
//     var connectionString = builder.Configuration.GetConnectionString("Postgres") 
//         ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");
    
//     var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
//     return dataSourceBuilder.Build();
// });

// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ICatalogQueries, CatalogQueries>();

var app = builder.Build();

// app.MapOpenApi();
// app.MapScalarApiReference();

// app.MapGet("/", () => {
//     return Results.Ok(new
//     {
//         name = "Vibe Coding to Production API",
//         milestone = "M0",
//         status = "running"
//     });
// });

// app.MapGet("/health", async(NpgsqlDataSource dataSource, CancellationToken ct) => {
//     await using var connection = await dataSource.OpenConnectionAsync(ct);
//     await using var command = new NpgsqlCommand("SELECT 1", connection);
//     var result = await command.ExecuteScalarAsync(ct);

//     return Results.Ok(new
//     {
//         api = "ok",
//         db = result?.ToString() == "1" ? "ok" : "unknown"
//     });
// });

// OpenAPI + Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Existing health endpint
app.MapGet("/health", async(IDbConnectionFactory connectionFactory, CancellationToken cancellationToken) =>
{
    try
    {
        using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return Results.Ok(new { api ="ok", db = "ok"});
    }
    catch
    {
        return Results.Json(new {api = "ok", db = "fail"}, statusCode: 503);
    }
})
.WithTags("System");

// M2 endpoints
app.MapArtistEndpoints();
app.MapAlbumEndpoints();
app.MapTrackEndpoints();

app.Run();
