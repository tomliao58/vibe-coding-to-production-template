using Npgsql;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres") 
        ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");
    
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    return dataSourceBuilder.Build();
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => {
    return Results.Ok(new
    {
        name = "Vibe Coding to Production API",
        milestone = "M0",
        status = "running"
    });
});

app.MapGet("/health", async(NpgsqlDataSource dataSource, CancellationToken ct) => {
    await using var connection = await dataSource.OpenConnectionAsync(ct);
    await using var command = new NpgsqlCommand("SELECT 1", connection);
    var result = await command.ExecuteScalarAsync(ct);

    return Results.Ok(new
    {
        api = "ok",
        db = result?.ToString() == "1" ? "ok" : "unknown"
    });
});

app.Run();
